// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace Buck
{
    /// <summary>
    /// ISingleChoiceProvider for desktop display resolutions. Produces stable IDs ("<width>x<height>").
    /// Works with SingleChoiceDropdown or SingleChoiceToggleGroup.
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

        [Header("Display")]
        [SerializeField, Tooltip("Show labels as \"1920 × 1080\". When enabled, aspect ratio suffix (e.g., 16:9) is appended.")]
        bool m_showAspectRatio = true;

        [Header("Auto-Select")]
        [SerializeField, Tooltip("Ideal horizontal padding below native width when Windowed for the auto algorithm.")]
        int m_windowedIdealPad = 64;

        readonly List<string> m_ids = new();
        readonly Dictionary<string, Vector2Int> m_idToSize = new(StringComparer.Ordinal);
        string m_lastManualId;

        public event Action LabelsChanged;

        public void Initialize()
        {
            BuildList();
            // Match current screen on startup
            m_lastManualId = FindIdForSize(new Vector2Int(Screen.width, Screen.height));
        }

        public IReadOnlyList<string> GetIds() => m_ids;

        public string GetCurrentId()
        {
            // Prefer the current physical screen size; fall back to last manual selection if not present
            var current = new Vector2Int(Screen.width, Screen.height);
            var id = FindIdForSize(current);
            if (!string.IsNullOrEmpty(id)) return id;
            if (!string.IsNullOrEmpty(m_lastManualId) && m_ids.Contains(m_lastManualId)) return m_lastManualId;
            return m_ids.Count > 0 ? m_ids[0] : string.Empty;
        }

        public void SelectById(string id)
        {
            if (string.IsNullOrEmpty(id)) return;
            if (!m_idToSize.TryGetValue(id, out var size)) return;

            ApplyResolution(size, IsFullscreen());
            m_lastManualId = id;

            // Keep the presenter selection in sync (e.g., dropdown index).
            LabelsChanged?.Invoke();
        }

        public string GetLabel(string id)
        {
            if (!m_idToSize.TryGetValue(id, out var sz))
                return id ?? string.Empty;

            if (!m_showAspectRatio)
                return $"{sz.x} × {sz.y}";

            var gcd = GCD(sz.x, sz.y);
            var arW = sz.x / gcd;
            var arH = sz.y / gcd;
            return $"{sz.x} × {sz.y}  ({arW}:{arH})";
        }

        public void BindLabelTo(string id, TMP_Text target)
        {
            if (!target) return;
            target.SetText(GetLabel(id) ?? string.Empty);
        }

        /// <summary>
        /// Recompute and apply a best-fit size based on the current fullscreen mode and display metrics.
        /// Also updates the current selection for presenters.
        /// </summary>
        public void ApplyAuto()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            if (m_ids.Count == 0) return;

            var full = IsFullscreen();
            var native = GetNativeDisplaySize();
            var best = BestFit(native, full);
            ApplyResolution(best, full);

            // Reflect new size in selection.
            m_lastManualId = FindIdForSize(best);
            LabelsChanged?.Invoke();
#endif
        }

        /// <summary>Apply the current dropdown/toggle selection again (useful after toggling fullscreen).</summary>
        public void ReapplyCurrentSelectionOrClosest()
        {
            var id = GetCurrentId();
            if (!string.IsNullOrEmpty(id) && m_idToSize.TryGetValue(id, out var sz))
            {
                ApplyResolution(sz, IsFullscreen());
            }
            else if (m_ids.Count > 0)
            {
                SelectById(m_ids[0]);
            }
        }

        /// <summary>Set fullscreen/windowed mode without changing width/height.</summary>
        public void ApplyFullscreen(bool fullscreen)
        {
            ApplyResolution(new Vector2Int(Screen.width, Screen.height), fullscreen);
            LabelsChanged?.Invoke();
        }

        // ---- internal ----

        void BuildList()
        {
            m_ids.Clear();
            m_idToSize.Clear();

#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE_OSX || UNITY_STANDALONE_LINUX
            IEnumerable<Vector2Int> sizes = m_useSystemResolutions
                ? Screen.resolutions
                    .Select(r => new Vector2Int(r.width, r.height))
                    .Distinct()
                : m_supportedResolutions.Distinct();

            // Sort descending by area, then width
            sizes = sizes
                .Where(s => s.x > 0 && s.y > 0)
                .OrderByDescending(s => s.x * s.y)
                .ThenByDescending(s => s.x);

            foreach (var s in sizes)
            {
                var id = $"{s.x}x{s.y}";
                if (m_idToSize.ContainsKey(id)) continue;
                m_idToSize[id] = s;
                m_ids.Add(id);
            }
#else
            // Non-desktop platforms typically don't expose resolution switching.
#endif
        }

        static int GCD(int a, int b)
        {
            while (b != 0) (a, b) = (b, a % b);
            return Mathf.Max(1, a);
        }

        static Vector2Int GetNativeDisplaySize()
        {
            var w = Display.main != null ? Display.main.systemWidth : Screen.currentResolution.width;
            var h = Display.main != null ? Display.main.systemHeight : Screen.currentResolution.height;
            if (w <= 0 || h <= 0) { w = Screen.width; h = Screen.height; }
            return new Vector2Int(w, h);
        }

        bool IsFullscreen()
        {
            var mode = Screen.fullScreenMode;
            return mode == FullScreenMode.ExclusiveFullScreen || mode == FullScreenMode.FullScreenWindow;
        }

        void ApplyResolution(Vector2Int size, bool fullscreen)
        {
            var targetMode = fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;

            // Ensure the mode toggles even if width/height don't change
            if (Screen.fullScreenMode != targetMode)
                Screen.fullScreenMode = targetMode;

            Screen.fullScreen = fullscreen;

            // Set size for windowed; harmless for FullScreenWindow (uses native)
            Screen.SetResolution(size.x, size.y, targetMode);
        }

        string FindIdForSize(Vector2Int size)
        {
            var id = $"{size.x}x{size.y}";
            return m_idToSize.ContainsKey(id) ? id : string.Empty;
        }

        Vector2Int BestFit(Vector2Int native, bool fullscreen)
        {
            // Find candidate with closest aspect ratio; tie-break by width closeness policy.
            float nativeAR = (float)native.x / native.y;
            Vector2Int best = default;
            float bestArDiff = float.PositiveInfinity;
            int bestWidthScore = int.MinValue;

            foreach (var kv in m_idToSize)
            {
                var s = kv.Value;
                if (s.x > native.x || s.y > native.y)
                    continue; // don't exceed native desktop size

                float ar = (float)s.x / s.y;
                float diff = Mathf.Abs(ar - nativeAR);

                // Windowed: prefer widths close to a small pad below native (e.g., 64 px).
                // Fullscreen: prefer the largest width under native.
                int widthScore = fullscreen
                    ? s.x
                    : -Mathf.Abs((native.x - m_windowedIdealPad) - s.x);

                if (diff < bestArDiff - 0.0001f ||
                    (Mathf.Abs(diff - bestArDiff) <= 0.0001f && widthScore > bestWidthScore))
                {
                    best = s;
                    bestArDiff = diff;
                    bestWidthScore = widthScore;
                }
            }

            // Fallback to largest available if nothing matched the <= native rule.
            if (best == default && m_ids.Count > 0)
                best = m_idToSize[m_ids[0]];

            return best;
        }
    }
}
