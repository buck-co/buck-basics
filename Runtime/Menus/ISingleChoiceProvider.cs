// MIT License - Copyright (c) 2025 BUCK Design LLC - https://github.com/buck-co

using System;
using System.Collections.Generic;
using TMPro;

namespace Buck
{
    /// <summary>
    /// Pluggable source of single-choice items for UI presenters.
    /// IDs are stable string keys; labels come from GetLabel(...) and/or BindLabelTo(...).
    /// </summary>
    public interface ISingleChoiceProvider
    {
        /// <summary>Prepare internal state and subscribe to required signals.</summary>
        void Initialize();

        /// <summary>Ordered, stable IDs for all available choices.</summary>
        IReadOnlyList<string> GetIds();

        /// <summary>Return the current selection's ID (should be one of GetIds()).</summary>
        string GetCurrentId();

        /// <summary>Commit selection by ID.</summary>
        void SelectById(string id);

        /// <summary>Resolve a display label for an ID. Used by dropdowns and as a fallback for toggles.</summary>
        string GetLabel(string id);

        /// <summary>
        /// Bind the label text for an ID to a TextMeshPro target.
        /// Providers may attach a LocalizeStringEvent or set literal text.
        /// Presenters fall back to GetLabel(...) when not implemented.
        /// </summary>
        void BindLabelTo(string id, TMP_Text target);

        /// <summary>
        /// Raised when labels should be refreshed (e.g., locale changed).
        /// Dropdown presenters use this to rebuild option strings.
        /// </summary>
        event Action LabelsChanged;
    }
}