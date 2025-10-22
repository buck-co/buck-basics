// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
#if BUCK_BASICS_ENABLE_LOCALIZATION
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
#endif

namespace Buck
{
    /// <summary>
    /// ISingleChoiceProvider for Unity Localization locales.
    /// Exposes locale IDs (e.g., "en", "en-US", "fr") with optional exclusion.
    /// Labels can be bound via LocalizeStringEvent (string table + prefix) or fall back to native culture names.
    /// </summary>
    [AddComponentMenu("BUCK/Localization/Locale Choice Provider")]
    public class LocaleChoiceProvider : MonoBehaviour, ISingleChoiceProvider
    {
        [Header("Filter")]
        [SerializeField, Tooltip("Exclude locales by Identifier.Code (e.g., \"en-US\", \"fr\").")]
        List<string> m_excludedIds = new();

#if BUCK_BASICS_ENABLE_LOCALIZATION
        [Header("Label Localization")]
        [SerializeField, Tooltip("If assigned, option labels use this string table with keys = Entry Prefix + <localeId>.")]
        LocalizedStringTable m_labelStringTable;

        [SerializeField, Tooltip("Key prefix applied before the locale ID (e.g., \"locale.\")")]
        string m_labelEntryPrefix = "locale.";
#endif

        readonly List<string> m_ids = new();
        public event Action LabelsChanged;

        public void Initialize()
        {
            BuildIdList();

#if BUCK_BASICS_ENABLE_LOCALIZATION
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
            LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
#endif
        }

        void OnDestroy()
        {
#if BUCK_BASICS_ENABLE_LOCALIZATION
            LocalizationSettings.SelectedLocaleChanged -= OnSelectedLocaleChanged;
#endif
        }

        public IReadOnlyList<string> GetIds() => m_ids;

        public string GetCurrentId()
        {
#if BUCK_BASICS_ENABLE_LOCALIZATION
            var cur = LocalizationSettings.SelectedLocale;
            var id  = cur != null ? cur.Identifier.Code : null;
            if (!string.IsNullOrEmpty(id) && m_ids.Contains(id)) return id;
            return m_ids.Count > 0 ? m_ids[0] : string.Empty;
#else
            return m_ids.Count > 0 ? m_ids[0] : string.Empty;
#endif
        }

        public void SelectById(string id)
        {
#if BUCK_BASICS_ENABLE_LOCALIZATION
            var loc = FindLocale(id);
            if (loc != null && loc != LocalizationSettings.SelectedLocale)
            {
                LocalizationSettings.SelectedLocale = loc;
                // TMP_Dropdown options need a manual refresh; toggles using LocalizeStringEvent update automatically.
                LabelsChanged?.Invoke();
            }
#endif
        }

        public string GetLabel(string id)
        {
#if BUCK_BASICS_ENABLE_LOCALIZATION
            // If a table is assigned, resolve synchronously through the String Database.
            if (m_labelStringTable.TableReference.ReferenceType != TableReference.Type.Empty)
            {
                var key = $"{m_labelEntryPrefix}{id}";
                try
                {
                    return LocalizationSettings.StringDatabase.GetLocalizedString(m_labelStringTable.TableReference, key);
                }
                catch
                {
                    // fall through to culture-based label
                }
            }
#endif
            return CultureNativeNameOrCode(id);
        }

        public void BindLabelTo(string id, TMP_Text target)
        {
            if (!target) return;

#if BUCK_BASICS_ENABLE_LOCALIZATION
            if (m_labelStringTable.TableReference.ReferenceType != TableReference.Type.Empty)
            {
                var lse = target.GetComponent<LocalizeStringEvent>() ?? target.gameObject.AddComponent<LocalizeStringEvent>();
                lse.StringReference = new UnityEngine.Localization.LocalizedString
                {
                    TableReference = m_labelStringTable.TableReference,
                    TableEntryReference = $"{m_labelEntryPrefix}{id}"
                };
                lse.OnUpdateString.RemoveAllListeners();
                lse.OnUpdateString.AddListener(target.SetText);
                lse.RefreshString();
                return;
            }
            else
            {
                var existing = target.GetComponent<LocalizeStringEvent>();
                if (existing) Destroy(existing);
            }
#endif
            target.SetText(CultureNativeNameOrCode(id));
        }

        static string CultureNativeNameOrCode(string id)
        {
            try
            {
                // CultureInfo supports "en-US"-style IDs. NativeName is localized to the culture itself.
                var ci = new CultureInfo(id);
                return ci.NativeName;
            }
            catch
            {
                return id;
            }
        }

        void BuildIdList()
        {
            m_ids.Clear();
            var excluded = new HashSet<string>(m_excludedIds ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);

#if BUCK_BASICS_ENABLE_LOCALIZATION
            var all = LocalizationSettings.AvailableLocales.Locales;

            if (all != null)
            {
                foreach (var loc in all)
                {
                    if (loc == null) continue;
                    var code = loc.Identifier.Code;
                    if (string.IsNullOrEmpty(code)) continue;
                    if (excluded.Contains(code)) continue;
                    m_ids.Add(code);
                }
            }
#endif
        }

#if BUCK_BASICS_ENABLE_LOCALIZATION
        static Locale FindLocale(string id)
        {
            var list = LocalizationSettings.AvailableLocales;
            if (list == null) return null;

            // Match exact code first, then the "language" only (e.g., "en" matches "en-US")
            var exact = list.GetLocale(id);
            if (exact) return exact;

            var dash = id.IndexOf('-');
            if (dash > 0)
            {
                var lang = id.Substring(0, dash);
                return list.GetLocale(lang);
            }

            return null;
        }

        void OnSelectedLocaleChanged(Locale _)
        {
            // When locale changes, dropdown text needs rebuilding.
            LabelsChanged?.Invoke();
        }
#endif
    }
}
