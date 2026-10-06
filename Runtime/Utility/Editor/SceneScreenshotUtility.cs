#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using Unity.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Buck
{
    /// <summary>
    /// Project hooks for the Scene Screenshot Utility (Tools > Scene Screenshot Utility), which captures
    /// everything rendered in the open scenes at a fixed pixels-per-world-unit scale. A project can keep
    /// debug or oversized renderers out of the capture and set up shader globals or editor prefs around it.
    /// </summary>
    public static class SceneScreenshotUtility
    {
        /// <summary>Return false to leave a renderer out: it is disabled while capturing and ignored for the bounds.</summary>
        public static Func<Renderer, bool> IncludeRenderer;

        /// <summary>Raised before the capture. The utility then waits a few frames so the changes take effect.</summary>
        public static event Action BeforeCapture;

        /// <summary>Raised once the capture has finished or failed, to undo whatever BeforeCapture did.</summary>
        public static event Action AfterCapture;

        internal static bool Includes(Renderer renderer) => IncludeRenderer == null || IncludeRenderer(renderer);
        internal static void RaiseBeforeCapture() => BeforeCapture?.Invoke();
        internal static void RaiseAfterCapture() => AfterCapture?.Invoke();
    }

    public class SceneScreenshotUtilityWindow : EditorWindow
    {
        [Serializable]
        class Settings
        {
            public LayerMask captureLayers = ~0;
            public bool includeInactive = true;
            public float paddingWorldUnits = 0f;
            public int pixelsPerWorldUnit = 16;
            public int maxTextureSize = 4096;
            public bool transparentBackground = false;
            public Color backgroundColor = new Color(.25f, .25f, .25f, 1f);
            public string outputFolder = "SceneScreenshots";
            public string fileNamePrefix = "";
        }

        struct SavedTile
        {
            public string path;
            public Rect worldRect;
            public int widthPx;
            public int heightPx;
        }

        // Player-loop updates to run after BeforeCapture so hooks (shader globals, hidden backgrounds) show up.
        const int k_settleFrames = 3;

        [SerializeField] Settings m_settings = new Settings();
        Vector2 m_scroll;
        bool m_capturing;
        int m_settleFramesLeft;
        readonly List<(Renderer renderer, bool wasEnabled)> m_disabledRenderers = new();
        readonly HashSet<Renderer> m_excluded = new();

        static string PrefsKey => "Buck.SceneScreenshotUtility." + PlayerSettings.productGUID;

        int MaxTextureSize => Mathf.Min(SystemInfo.maxTextureSize, m_settings.maxTextureSize);
        string OutputDirectory => Path.GetFullPath(Path.Combine(Application.dataPath, "..", m_settings.outputFolder));

        [MenuItem("Tools/Scene Screenshot Utility")]
        static void OpenWindow()
        {
            var window = GetWindow<SceneScreenshotUtilityWindow>("Scene Screenshot Utility");
            window.minSize = new Vector2(340, 440);
            window.Show();
        }

        void OnEnable()
        {
            string json = EditorPrefs.GetString(PrefsKey, "");
            if (!string.IsNullOrEmpty(json))
                JsonUtility.FromJsonOverwrite(json, m_settings);
        }

        void OnDisable()
        {
            if (!m_capturing)
                return;
            EditorApplication.update -= Tick;
            FinishCapture();
        }

        void OnGUI()
        {
            m_scroll = EditorGUILayout.BeginScrollView(m_scroll);
            EditorGUI.BeginChangeCheck();

            GUILayout.Label("What to Capture", EditorStyles.boldLabel);
            m_settings.captureLayers = LayerMaskField(new GUIContent("Capture Layers", "Only renderers on these layers are included."), m_settings.captureLayers);
            m_settings.includeInactive = EditorGUILayout.Toggle(new GUIContent("Include Inactive", "Include disabled renderers and inactive GameObjects when calculating bounds."), m_settings.includeInactive);
            m_settings.paddingWorldUnits = Mathf.Max(0f, EditorGUILayout.FloatField(new GUIContent("Padding (World Units)", "Extra world-space padding around the bounds."), m_settings.paddingWorldUnits));

            EditorGUILayout.Space();
            GUILayout.Label("Scale & Output", EditorStyles.boldLabel);
            m_settings.pixelsPerWorldUnit = EditorGUILayout.IntSlider(new GUIContent("Pixels Per World Unit", "Pixels per world unit in the exported image. Use the same value across scenes for uniform texel density."), m_settings.pixelsPerWorldUnit, 1, 256);
            m_settings.maxTextureSize = EditorGUILayout.IntSlider(new GUIContent("Max Texture Size", "Largest image dimension before the capture is split into tiles. 16384 is the most Unity and most hardware support."), m_settings.maxTextureSize, 64, 16384);
            m_settings.transparentBackground = EditorGUILayout.Toggle(new GUIContent("Transparent Background", "Transparent background if on; otherwise filled with Background Color."), m_settings.transparentBackground);
            using (new EditorGUI.DisabledScope(m_settings.transparentBackground))
                m_settings.backgroundColor = EditorGUILayout.ColorField(new GUIContent("Background Color"), m_settings.backgroundColor);
            m_settings.outputFolder = EditorGUILayout.TextField(new GUIContent("Output Folder", "Relative to the project root (one level above Assets)."), m_settings.outputFolder);
            m_settings.fileNamePrefix = EditorGUILayout.TextField(new GUIContent("File Name Prefix", "Optional prefix (e.g. a build number) added to the file name."), m_settings.fileNamePrefix);

            if (EditorGUI.EndChangeCheck())
                EditorPrefs.SetString(PrefsKey, JsonUtility.ToJson(m_settings));

            EditorGUILayout.Space();
            GUILayout.Label("Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Max Texture Size", MaxTextureSize.ToString());
            EditorGUILayout.LabelField("Output Directory", OutputDirectory, EditorStyles.wordWrappedLabel);

            EditorGUILayout.Space();
            using (new EditorGUI.DisabledScope(m_capturing))
            {
                if (GUILayout.Button(m_capturing ? "Capturing..." : "Capture Scene", GUILayout.Height(32)))
                    StartCapture();
            }
            if (GUILayout.Button("Open Screenshot Folder", GUILayout.Height(32)))
                OpenFolder(OutputDirectory);

            EditorGUILayout.EndScrollView();
        }

        static LayerMask LayerMaskField(GUIContent label, LayerMask mask)
        {
            int concatenated = InternalEditorUtility.LayerMaskToConcatenatedLayersMask(mask);
            concatenated = EditorGUILayout.MaskField(label, concatenated, InternalEditorUtility.layers);
            return InternalEditorUtility.ConcatenatedLayersMaskToLayerMask(concatenated);
        }

        static void OpenFolder(string absolutePath)
        {
            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);
            EditorUtility.OpenWithDefaultApp(absolutePath);
        }

        void StartCapture()
        {
            if (m_capturing)
                return;
            m_capturing = true;
            EditorUtility.DisplayProgressBar("Capturing Scene Screenshot", "Preparing scene...", 0.1f);
            SceneScreenshotUtility.RaiseBeforeCapture();
            DisableExcludedRenderers();
            m_settleFramesLeft = k_settleFrames;
            EditorApplication.update += Tick;
        }

        void Tick()
        {
            EditorApplication.QueuePlayerLoopUpdate();
            if (--m_settleFramesLeft > 0)
                return;
            EditorApplication.update -= Tick;
            try
            {
                Capture();
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                FinishCapture();
            }
        }

        void FinishCapture()
        {
            foreach (var (renderer, wasEnabled) in m_disabledRenderers)
                if (renderer != null)
                    renderer.enabled = wasEnabled;
            m_disabledRenderers.Clear();
            m_excluded.Clear();
            SceneScreenshotUtility.RaiseAfterCapture();
            EditorUtility.ClearProgressBar();
            m_capturing = false;
            Repaint();
        }

        void DisableExcludedRenderers()
        {
            foreach (var renderer in FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (SceneScreenshotUtility.Includes(renderer))
                    continue;
                m_excluded.Add(renderer);
                m_disabledRenderers.Add((renderer, renderer.enabled));
                renderer.enabled = false;
            }
        }

        void Capture()
        {
            EditorUtility.DisplayProgressBar("Capturing Scene Screenshot", "Gathering bounds...", 0.2f);

            Bounds bounds = GetSceneBounds();
            if (bounds.size == Vector3.zero)
            {
                Debug.LogError("SceneScreenshotUtility: No eligible renderers found on the selected layers.");
                return;
            }

            Rect worldRect = ToXYRect(bounds, m_settings.paddingWorldUnits);
            int finalW = Mathf.CeilToInt(worldRect.width * m_settings.pixelsPerWorldUnit);
            int finalH = Mathf.CeilToInt(worldRect.height * m_settings.pixelsPerWorldUnit);

            bool savedAny;
            string outputDir = OutputDirectory;

            if (finalW <= MaxTextureSize && finalH <= MaxTextureSize)
            {
                EditorUtility.DisplayProgressBar("Capturing Scene Screenshot", "Capturing image...", 0.5f);
                savedAny = CaptureRectToPng(worldRect, bounds, finalW, finalH, BuildOutputPath(finalW, finalH, -1, -1), trimTransparentPixels: true);
            }
            else
            {
                // Tiles preserve the requested pixels per unit when the whole scene would exceed the texture size
                Debug.LogWarning($"SceneScreenshotUtility: Target size {finalW}x{finalH} exceeds the maximum texture size ({MaxTextureSize}). Falling back to tiles.");
                savedAny = CaptureTiles(worldRect, bounds, finalW, finalH);
            }

            EditorUtility.DisplayProgressBar("Capturing Scene Screenshot", "Finalizing...", 0.95f);
            Debug.Log($"SceneScreenshotUtility: Done. SavedAny={savedAny}, PPWU={m_settings.pixelsPerWorldUnit}, " +
                      $"Bounds(WxH)={worldRect.width:0.###}x{worldRect.height:0.###}, Pixels={finalW}x{finalH}, Output='{outputDir}'.");
        }

        bool CaptureTiles(Rect worldRect, Bounds bounds, int finalW, int finalH)
        {
            int tileSize = MaxTextureSize;
            int cols = Mathf.CeilToInt((float)finalW / tileSize);
            int rows = Mathf.CeilToInt((float)finalH / tileSize);
            float tileWorld = tileSize / (float)m_settings.pixelsPerWorldUnit;
            var savedTiles = new List<SavedTile>();

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    int curW = c == cols - 1 ? finalW - c * tileSize : tileSize;
                    int curH = r == rows - 1 ? finalH - r * tileSize : tileSize;
                    var tileRect = new Rect(worldRect.xMin + c * tileWorld, worldRect.yMin + r * tileWorld,
                        curW / (float)m_settings.pixelsPerWorldUnit, curH / (float)m_settings.pixelsPerWorldUnit);

                    float progress = Mathf.Min(0.5f + 0.5f * ((r * cols + c) / (float)(rows * cols)), 0.9f);
                    EditorUtility.DisplayProgressBar("Capturing Scene Screenshot", $"Capturing tile {r + 1},{c + 1}...", progress);

                    // Tiles are not trimmed so they still line up with each other
                    string path = BuildOutputPath(finalW, finalH, r, c);
                    if (CaptureRectToPng(tileRect, bounds, curW, curH, path, trimTransparentPixels: false))
                        savedTiles.Add(new SavedTile { path = path, worldRect = tileRect, widthPx = curW, heightPx = curH });
                }
            }

            if (savedTiles.Count != 1)
                return savedTiles.Count > 0;

            // Only one tile had content: re-save it trimmed under the single-image name
            SavedTile tile = savedTiles[0];
            if (!CaptureRectToPng(tile.worldRect, bounds, tile.widthPx, tile.heightPx, BuildOutputPath(finalW, finalH, -1, -1), trimTransparentPixels: true))
                return true;
            try
            {
                if (File.Exists(tile.path))
                    File.Delete(tile.path);
            }
            catch (Exception e)
            {
                Debug.LogWarning($"SceneScreenshotUtility: Could not delete temporary tile '{tile.path}': {e.Message}");
            }
            return true;
        }

        Bounds GetSceneBounds()
        {
            var renderers = FindObjectsByType<Renderer>(m_settings.includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude, FindObjectsSortMode.None);
            Bounds? bounds = null;
            foreach (var renderer in renderers)
            {
                if (((1 << renderer.gameObject.layer) & m_settings.captureLayers.value) == 0)
                    continue;
                if (m_excluded.Contains(renderer))
                    continue;
                if (!renderer.enabled && !m_settings.includeInactive)
                    continue;

                if (bounds == null)
                    bounds = renderer.bounds;
                else
                {
                    Bounds b = bounds.Value;
                    b.Encapsulate(renderer.bounds);
                    bounds = b;
                }
            }
            return bounds ?? new Bounds(Vector3.zero, Vector3.zero);
        }

        static Rect ToXYRect(Bounds b, float padding)
            => new Rect(b.min.x - padding, b.min.y - padding, b.size.x + padding * 2f, b.size.y + padding * 2f);

        /// <summary>Renders a world rect to a PNG. Returns true only if a non-empty image was saved.</summary>
        bool CaptureRectToPng(Rect worldRect, Bounds fullBounds, int widthPx, int heightPx, string path, bool trimTransparentPixels)
        {
            var go = new GameObject("~SceneCaptureCamera") { hideFlags = HideFlags.HideAndDontSave };
            var cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.clear;
            cam.cullingMask = m_settings.captureLayers;

            const float zPad = 10f;
            float camZ = fullBounds.center.z - (fullBounds.extents.z + zPad);
            Vector3 camPos = new Vector3(worldRect.center.x, worldRect.center.y, camZ);
            if (m_settings.pixelsPerWorldUnit > 0)
            {
                // Snap to the pixel grid so neighbouring tiles share texel alignment
                float snap = 1f / m_settings.pixelsPerWorldUnit;
                camPos.x = Mathf.Round(camPos.x / snap) * snap;
                camPos.y = Mathf.Round(camPos.y / snap) * snap;
            }
            go.transform.position = camPos;
            cam.nearClipPlane = 0.01f;
            cam.farClipPlane = (fullBounds.max.z - camZ) + zPad;
            cam.orthographicSize = worldRect.height * 0.5f;
            cam.aspect = widthPx / (float)heightPx;

            // Render Graph wants a depth buffer on camera output textures
            var rt = new RenderTexture(widthPx, heightPx, 24, RenderTextureFormat.ARGB32)
            {
                useMipMap = false,
                autoGenerateMips = false,
                antiAliasing = Mathf.Max(1, QualitySettings.antiAliasing),
                name = "~SceneCaptureRT"
            };
            RenderTexture prevActive = RenderTexture.active;
            bool saved = false;

            try
            {
                cam.targetTexture = rt;
                RenderTexture.active = rt;
                cam.Render();

                var tex = new Texture2D(widthPx, heightPx, TextureFormat.RGBA32, false, false);
                tex.ReadPixels(new Rect(0, 0, widthPx, heightPx), 0, 0, false);
                tex.Apply(false, false);

                if (!TryGetOpaqueBounds(tex.GetRawTextureData<Color32>(), widthPx, heightPx, out RectInt opaque))
                {
                    DestroyImmediate(tex); // entirely transparent
                    return false;
                }

                Texture2D toSave = tex;
                if (trimTransparentPixels)
                {
                    int pad = (int)(m_settings.paddingWorldUnits * m_settings.pixelsPerWorldUnit);
                    int x = Mathf.Max(0, opaque.x - pad);
                    int y = Mathf.Max(0, opaque.y - pad);
                    int w = Mathf.Min(widthPx - x, opaque.width + pad * 2);
                    int h = Mathf.Min(heightPx - y, opaque.height + pad * 2);
                    if (w != widthPx || h != heightPx)
                    {
                        Color[] cropped = tex.GetPixels(x, y, w, h);
                        toSave = new Texture2D(w, h, TextureFormat.RGBA32, false, false);
                        toSave.SetPixels(0, 0, w, h, cropped);
                        toSave.Apply(false, false);
                        DestroyImmediate(tex);
                    }
                }

                if (!m_settings.transparentBackground && m_settings.backgroundColor.a > 0f)
                {
                    Color[] pixels = toSave.GetPixels();
                    for (int i = 0; i < pixels.Length; i++)
                        pixels[i] = Color.Lerp(m_settings.backgroundColor, pixels[i], pixels[i].a);
                    var composited = new Texture2D(toSave.width, toSave.height, TextureFormat.RGBA32, false, false);
                    composited.SetPixels(pixels);
                    composited.Apply(false, false);
                    DestroyImmediate(toSave);
                    toSave = composited;
                }

                string dir = Path.GetDirectoryName(path);
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                File.WriteAllBytes(path, toSave.EncodeToPNG());
                DestroyImmediate(toSave);
                saved = true;
            }
            finally
            {
                RenderTexture.active = prevActive;
                cam.targetTexture = null;
                DestroyImmediate(rt);
                DestroyImmediate(go);
            }

            return saved;
        }

        string BuildOutputPath(int finalW, int finalH, int tileRow, int tileCol)
        {
            string scene = SceneManager.GetActiveScene().name;
            string timeStamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string baseName = $"{scene}_{m_settings.pixelsPerWorldUnit}ppwu_{finalW}x{finalH}_{timeStamp}";
            if (!string.IsNullOrEmpty(m_settings.fileNamePrefix))
                baseName = m_settings.fileNamePrefix + "_" + baseName;
            if (tileRow >= 0 && tileCol >= 0)
                baseName += $"_r{tileRow:D2}_c{tileCol:D2}";
            return Path.Combine(OutputDirectory, baseName + ".png");
        }

        /// <summary>Pixel bounds of every non-transparent pixel, so images can be trimmed.</summary>
        static bool TryGetOpaqueBounds(NativeArray<Color32> raw, int w, int h, out RectInt boundsPx)
        {
            int minX = w, minY = h, maxX = -1, maxY = -1;
            for (int y = 0; y < h; y++)
            {
                int row = y * w;
                for (int x = 0; x < w; x++)
                {
                    if (raw[row + x].a == 0)
                        continue;
                    if (x < minX) minX = x;
                    if (x > maxX) maxX = x;
                    if (y < minY) minY = y;
                    if (y > maxY) maxY = y;
                }
            }

            if (maxX >= minX && maxY >= minY)
            {
                boundsPx = new RectInt(minX, minY, maxX - minX + 1, maxY - minY + 1);
                return true;
            }
            boundsPx = default;
            return false;
        }
    }
}
#endif
