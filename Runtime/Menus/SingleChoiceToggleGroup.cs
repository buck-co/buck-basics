// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Renders a single-choice list as a ToggleGroup. Spawns toggles under a root.
    /// The provider supplies IDs and labels; this component ensures single-selection semantics.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Single Choice (Toggle Group)")]
    public class SingleChoiceToggleGroup : MonoBehaviour
    {
        [Header("Provider")]
        [SerializeField, Tooltip("A component that implements ISingleChoiceProvider.")]
        Component m_providerComponent;

        [Header("Spawn")]
        [SerializeField, Tooltip("Parent for the ToggleGroup and its toggles. If null, uses this transform.")]
        RectTransform m_spawnRoot;

        [SerializeField, Tooltip("Prefab with a Toggle and a TextMeshProUGUI label. If null, a simple default is created.")]
        Toggle m_togglePrototype;

        ISingleChoiceProvider m_provider;
        ToggleGroup m_group;
        readonly Dictionary<string, Toggle> m_idToToggle = new();

        void Awake()
        {
            if (!m_spawnRoot) m_spawnRoot = transform as RectTransform;
        }

        void OnEnable()
        {
            m_provider = (ISingleChoiceProvider)m_providerComponent ?? GetComponent<ISingleChoiceProvider>();
            if (m_provider == null)
            {
                Debug.LogError($"{nameof(SingleChoiceToggleGroup)} requires an ISingleChoiceProvider.", this);
                return;
            }

            m_provider.Initialize();
            m_provider.LabelsChanged += HandleLabelsChanged;

            Build();
        }

        void OnDisable()
        {
            if (m_provider != null)
                m_provider.LabelsChanged -= HandleLabelsChanged;
        }

        void HandleLabelsChanged() => RefreshLabels();

        void Build()
        {
            ClearChildren();

            // Ensure a ToggleGroup exists under the spawn root.
            m_spawnRoot.gameObject.AddComponent(typeof(ToggleGroup));
            m_group = m_spawnRoot.GetComponent<ToggleGroup>();
            m_group.allowSwitchOff = false;

            m_idToToggle.Clear();

            var ids = m_provider.GetIds();
            var currentId = m_provider.GetCurrentId();

            foreach (var id in ids)
            {
                var toggle = CreateToggle(m_spawnRoot);
                toggle.group = m_group;

                // Label binding (preferred: provider adds LocalizeStringEvent; fallback to literal)
                var label = toggle.GetComponentInChildren<TMP_Text>(true);
                if (label)
                {
                    bool bound = false;
                    try { m_provider.BindLabelTo(id, label); bound = true; }
                    catch { /* provider may not override this; fall back below */ }

                    if (!bound) label.SetText(m_provider.GetLabel(id) ?? string.Empty);
                }

                // Wire selection -> provider.SelectById(id)
                toggle.onValueChanged.AddListener(on =>
                {
                    if (on) m_provider.SelectById(id);
                });

                toggle.SetIsOnWithoutNotify(id == currentId);
                m_idToToggle[id] = toggle;
            }

            // Ensure one toggle is on in case provider returned a non-listed currentId.
            if (!string.IsNullOrEmpty(currentId) && m_idToToggle.TryGetValue(currentId, out var cur))
            {
                cur.SetIsOnWithoutNotify(true);
            }
            else if (ids.Count > 0 && m_idToToggle.TryGetValue(ids[0], out var first))
            {
                first.SetIsOnWithoutNotify(true);
                m_provider.SelectById(ids[0]);
            }
        }

        void RefreshLabels()
        {
            if (m_provider == null) return;
            foreach (var kvp in m_idToToggle)
            {
                var id = kvp.Key;
                var label = kvp.Value ? kvp.Value.GetComponentInChildren<TMP_Text>(true) : null;
                if (!label) continue;

                // Try to re-bind through provider (for LocalizeStringEvent scenarios).
                // If provider uses LocalizeStringEvent, it already updates automatically; this call is harmless.
                try { m_provider.BindLabelTo(id, label); }
                catch { label.SetText(m_provider.GetLabel(id) ?? string.Empty); }
            }
        }

        void ClearChildren()
        {
            if (!m_spawnRoot) return;
            for (int i = m_spawnRoot.childCount - 1; i >= 0; --i)
                Destroy(m_spawnRoot.GetChild(i).gameObject);
        }

        Toggle CreateToggle(Transform parent)
        {
            if (m_togglePrototype)
                return Instantiate(m_togglePrototype, parent);

            // Create a minimal default: Toggle with a background + checkmark + TMP label.
            var root = new GameObject("Choice Toggle", typeof(RectTransform), typeof(Toggle));
            var rt   = root.GetComponent<RectTransform>();
            rt.SetParent(parent, false);
            rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot     = new Vector2(0.5f, 0.5f);

            var bgGO = new GameObject("Background", typeof(RectTransform), typeof(Image));
            var bgRT = bgGO.GetComponent<RectTransform>();
            bgRT.SetParent(rt, false);
            bgRT.anchorMin = new Vector2(0f, 0.5f);
            bgRT.anchorMax = new Vector2(0f, 0.5f);
            bgRT.pivot     = new Vector2(0.5f, 0.5f);
            bgRT.anchoredPosition = Vector2.zero;
            bgRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 20f);
            bgRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   20f);

            var ckGO = new GameObject("Checkmark", typeof(RectTransform), typeof(Image));
            var ckRT = ckGO.GetComponent<RectTransform>();
            ckRT.SetParent(bgRT, false);
            ckRT.anchorMin = ckRT.anchorMax = new Vector2(0.5f, 0.5f);
            ckRT.pivot     = new Vector2(0.5f, 0.5f);
            ckRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 14f);
            ckRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   14f);

            var lblGO = new GameObject("Label", typeof(RectTransform));
            var lblRT = lblGO.GetComponent<RectTransform>();
            lblRT.SetParent(rt, false);
            lblRT.anchorMin = new Vector2(0f, 0.5f);
            lblRT.anchorMax = new Vector2(0f, 0.5f);
            lblRT.pivot     = new Vector2(0f, 0.5f);
            lblRT.anchoredPosition = new Vector2(16 + 8 + 4, 0f);

            var lbl = lblGO.AddComponent<TextMeshProUGUI>();
            lbl.text = "Option";

            var imgBG = bgGO.GetComponent<Image>();
            var imgCK = ckGO.GetComponent<Image>();
            var toggle = root.GetComponent<Toggle>();
            toggle.targetGraphic = imgBG;
            toggle.graphic       = imgCK;

            return toggle;
        }
    }
}
