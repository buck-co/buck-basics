// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Buck
{
    /// <summary>
    /// ISingleChoiceProvider for desktop display resolutions.
    /// Responsibilities:
    /// - Expose stable IDs ("<width>x<height>") and labels for a presenter (dropdown or toggles).
    /// - Apply resolution size on selection.
    /// - Toggle fullscreen mode independently of size.
    /// - "Auto" sets size to native desktop resolution without changing fullscreen.
    /// </summary>
    [AddComponentMenu("BUCK/Display/Resolution Choice Provider")]
    public class ResolutionChoiceProvider : MonoBehaviour, ISingleChoiceProvider
    {
        [Header("Source")]
        [SerializeField, Tooltip("If true, list all unique WxH from Screen.resolutions; otherwise use the curated list below.")]
        bool m_useSystemResolutions = true;

        [SerializeField, Tooltip("Curated list used when Use System Resolutions is false.")]
        List<Vector2Int> m_supportedResolutions = new()
        {
            new Vector2Int(1280, 720),
            new Vector2Int(1600, 900),
            new Vector2Int(1920, 1080),
            new Vector2Int(2560, 1440),
            new Vector2Int(3840, 2160),
        };

        [SerializeField, Tooltip("Optional. Restricts the list to aspect ratios this game supports. " +
                                 "The current and native display sizes are resolved through this policy too, " +
                                 "so an unsupported ratio cannot reappear via startup selection or Auto.")]
        AspectRatioPolicy m_aspectRatioPolicy;

        [Header("Display")]
        [SerializeField] bool m_showAspectRatio = true;

        readonly List<string> m_ids = new();
        readonly Dictionary<string, Vector2Int> m_idToSize = new(StringComparer.Ordinal);
        string m_currentId;

        public event Action LabelsChanged;

        /// <summary>
        /// Optional aspect ratio rules applied to this provider's list. Null means no filtering.
        /// Exposed so callers that apply resolutions themselves, such as a save data restore path,
        /// can validate against the same rules.
        /// </summary>
        public AspectRatioPolicy AspectRatioPolicy => m_aspectRatioPolicy;

        public void Initialize()
        {
            BuildList();

            // Seed selection from the current physical size; ensure it's present in the list.
            // Resolved through the policy so an unsupported size isn't smuggled back in by FindOrAddId.
            var current = new Vector2Int(Screen.width, Screen.height);
            m_currentId = FindOrAddId(Resolve(current));
        }

        public IReadOnlyList<string> GetIds() => m_ids;

        public string GetCurrentId()
        {
            if (!string.IsNullOrEmpty(m_currentId) && m_ids.Contains(m_currentId))
                return m_currentId;
            return m_ids.Count > 0 ? m_ids[0] : string.Empty;
        }

        public void SelectById(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!m_idToSize.TryGetValue(id, out var size)) return;

            m_currentId = id;
            SetResolution(size);
            LabelsChanged?.Invoke(); // presenters (e.g., dropdown) reselect immediately
        }

        public string GetLabel(string id)
        {
            if (!m_idToSize.TryGetValue(id, out var sz))
                return id ?? string.Empty;

            if (!m_showAspectRatio)
                return $"{sz.x} × {sz.y}";

            var g = GCD(sz.x, sz.y);
            return $"{sz.x} × {sz.y}  ({sz.x / g}:{sz.y / g})";
        }

        public void BindLabelTo(string id, TMP_Text target)
        {
            if (!target) return;
            target.SetText(GetLabel(id) ?? string.Empty);
        }

        /// <summary>
        /// Set size to native desktop resolution (does not change fullscreen).
        /// If an Aspect Ratio Policy disallows the native ratio, the nearest supported size is used
        /// instead, so "Auto" means "native, or the closest thing to native this game supports".
        /// Adds the resulting size to the list if missing, then selects it.
        /// </summary>
        public void ApplyAuto()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            EnsureBuilt();
            var native = Resolve(GetNativeDisplaySize());
            var id = FindOrAddId(native);
            m_currentId = id;
            SetResolution(native);
            LabelsChanged?.Invoke();
#endif
        }

        /// <summary>
        /// Flip fullscreen mode only; width/height are preserved.
        /// </summary>
        public void ApplyFullscreen(bool fullscreen)
        {
            var mode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.fullScreenMode = mode;
            Screen.fullScreen = fullscreen;
            // Resolution choice and list are independent of mode; no LabelsChanged here.
        }

        /// <summary>
        /// Re-applies the current selection's size; useful if the caller wants to enforce the chosen size again.
        /// </summary>
        public void ReapplyCurrentSelectionOrClosest()
        {
            EnsureBuilt();
            if (!string.IsNullOrEmpty(m_currentId) && m_idToSize.TryGetValue(m_currentId, out var sz))
            {
                SetResolution(sz);
                LabelsChanged?.Invoke();
            }
        }

        /// <summary>Formats a size as this provider's stable ID, for example "1920x1080".</summary>
        public static string ToId(Vector2Int size) => $"{size.x}x{size.y}";

        /// <summary>
        /// Parses an ID produced by ToId. Returns false for anything else, including non-positive sizes.
        /// Useful for validating a persisted resolution ID before applying it.
        /// </summary>
        public static bool TryParseId(string id, out Vector2Int size)
        {
            size = default;
            if (string.IsNullOrEmpty(id)) return false;

            var parts = id.Split('x');
            if (parts.Length != 2) return false;

            if (!int.TryParse(parts[0], NumberStyles.None, CultureInfo.InvariantCulture, out var w)) return false;
            if (!int.TryParse(parts[1], NumberStyles.None, CultureInfo.InvariantCulture, out var h)) return false;
            if (w <= 0 || h <= 0) return false;

            size = new Vector2Int(w, h);
            return true;
        }

        // -------- internals --------

        void EnsureBuilt()
        {
            if (m_ids.Count == 0)
                Initialize();
        }

        void BuildList()
        {
            m_ids.Clear();
            m_idToSize.Clear();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            IEnumerable<Vector2Int> source = m_useSystemResolutions
                ? Screen.resolutions.Select(r => new Vector2Int(r.width, r.height))
                : m_supportedResolutions;

            // Always include the current and native sizes so Auto and startup selection are present.
            // Materialized because the aspect ratio fallback below may enumerate this a second time.
            var sizes = source
                .Concat(new[] { new Vector2Int(Screen.width, Screen.height), GetNativeDisplaySize() })
                .Where(s => s.x > 0 && s.y > 0)
                .Distinct()
                .OrderByDescending(s => s.x * s.y)
                .ThenByDescending(s => s.x)
                .ToList();

            AddSizes(sizes.Where(IsAllowed));

            // Never present an empty list. If the policy excluded everything, ignore it and say so.
            // This has to stay inside the platform check, or it would fire on every Initialize() for
            // targets where the whole block compiles out and the list is legitimately empty.
            if (m_ids.Count == 0)
            {
                if (m_aspectRatioPolicy && m_aspectRatioPolicy.IsActive)
                    Debug.LogWarning($"{nameof(ResolutionChoiceProvider)} on \"{name}\": the Aspect Ratio " +
                                     $"Policy \"{m_aspectRatioPolicy.name}\" excluded every available " +
                                     $"resolution, so it was ignored. Check its Mode and bounds.", this);

                AddSizes(sizes);
            }
#endif
        }

        void AddSizes(IEnumerable<Vector2Int> sizes)
        {
            foreach (var s in sizes)
            {
                var id = ToId(s);
                if (m_idToSize.ContainsKey(id)) continue;
                m_idToSize[id] = s;
                m_ids.Add(id);
            }
        }

        bool IsAllowed(Vector2Int size)
            => !m_aspectRatioPolicy || m_aspectRatioPolicy.IsAllowed(size);

        // Callers pass the result straight to FindOrAddId, which mutates m_idToSize. That's safe because
        // the candidate enumeration finishes while this is still being evaluated as an argument.
        Vector2Int Resolve(Vector2Int desired)
            => m_aspectRatioPolicy
                ? m_aspectRatioPolicy.ResolveNearestAllowed(desired, m_idToSize.Values)
                : desired;

        static int GCD(int a, int b) { while (b != 0) (a, b) = (b, a % b); return Mathf.Max(1, a); }

        static Vector2Int GetNativeDisplaySize()
        {
            var w = Display.main != null ? Display.main.systemWidth  : Screen.currentResolution.width;
            var h = Display.main != null ? Display.main.systemHeight : Screen.currentResolution.height;
            if (w <= 0 || h <= 0) { w = Screen.width; h = Screen.height; }
            return new Vector2Int(w, h);
        }

        // Callers resolve the size through the aspect ratio policy before calling this.
        string FindOrAddId(Vector2Int size)
        {
            var id = ToId(size);
            if (!m_idToSize.ContainsKey(id))
            {
                // Insert at front so native/current appear at the top if they weren’t in the source list.
                m_idToSize[id] = size;
                m_ids.Insert(0, id);
            }
            return id;
        }

        void SetResolution(Vector2Int size)
        {
            var mode = Screen.fullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            Screen.SetResolution(size.x, size.y, mode);
        }
    }
}
