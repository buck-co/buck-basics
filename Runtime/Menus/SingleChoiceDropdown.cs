// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    /// <summary>
    /// Renders a single-choice list as a TMP_Dropdown. Options are rebuilt when the provider signals LabelsChanged.
    /// </summary>
    [AddComponentMenu("BUCK/UI/Single Choice (Dropdown)")]
    public class SingleChoiceDropdown : MonoBehaviour
    {
        [Header("Provider")]
        [SerializeField, Tooltip("A component that implements ISingleChoiceProvider.")]
        Component m_providerComponent;

        [Header("Dropdown")]
        [SerializeField, Tooltip("Dropdown to populate. If null, a default TMP_Dropdown will be created here.")]
        TMP_Dropdown m_dropdown;

        ISingleChoiceProvider m_provider;
        readonly List<string> m_ids = new();

        void Awake()
        {
            if (!m_dropdown)
                m_dropdown = EnsureDropdownOn(gameObject);

            EnsureTemplateScrollerHook(m_dropdown);
        }

        void OnEnable()
        {
            m_provider = (ISingleChoiceProvider)m_providerComponent ?? GetComponent<ISingleChoiceProvider>();
            if (m_provider == null)
            {
                Debug.LogError($"{nameof(SingleChoiceDropdown)} requires an ISingleChoiceProvider.", this);
                return;
            }

            m_provider.Initialize();
            m_provider.LabelsChanged += RebuildOptions;

            m_dropdown.onValueChanged.AddListener(OnValueChanged);
            RebuildOptions();
        }

        void OnDisable()
        {
            if (m_provider != null)
                m_provider.LabelsChanged -= RebuildOptions;

            if (m_dropdown != null)
                m_dropdown.onValueChanged.RemoveListener(OnValueChanged);
        }

        void OnValueChanged(int index)
        {
            if (index < 0 || index >= m_ids.Count) return;
            m_provider.SelectById(m_ids[index]);
        }

        void RebuildOptions()
        {
            if (m_provider == null || m_dropdown == null) return;

            var currentId = m_provider.GetCurrentId();
            var ids = m_provider.GetIds();

            m_ids.Clear();
            m_ids.AddRange(ids);

            var options = new List<TMP_Dropdown.OptionData>(ids.Count);
            for (int i = 0; i < ids.Count; i++)
                options.Add(new TMP_Dropdown.OptionData(m_provider.GetLabel(ids[i]) ?? string.Empty));

            m_dropdown.options = options;

            var selectedIndex = Mathf.Max(0, ids.IndexOf(currentId));
            m_dropdown.SetValueWithoutNotify(selectedIndex);
            m_dropdown.RefreshShownValue();
        }

        static TMP_Dropdown EnsureDropdownOn(GameObject go)
        {
            var dd = go.GetComponent<TMP_Dropdown>();
            if (dd) return dd;

            dd = go.AddComponent<TMP_Dropdown>();
            return dd;
        }
        
        static void EnsureTemplateScrollerHook(TMP_Dropdown dd)
        {
            if (!dd || !dd.template) return;
            var sr = dd.template.GetComponentInChildren<ScrollRect>(true);
            if (!sr) return;

            var helper = sr.GetComponent<TMPDropdownEnsureVisible>();
            if (!helper) helper = sr.gameObject.AddComponent<TMPDropdownEnsureVisible>();
            helper.Bind(dd, sr);
        }
    }
}
