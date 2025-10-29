using UnityEngine;
using UnityEngine.UI;

namespace Buck
{
    [CreateAssetMenu(fileName = "SelectableColorsProfile", menuName = "BUCK/Selectable Colors Profile")]
    public class SelectableColorsProfile : BaseScriptableObject
    {
        [Header("Pointer Mode (mouse/pen/touch)")]
        [SerializeField] ColorBlock m_pointer;
        public ColorBlock Pointer => m_pointer;

        [Header("Navigation Mode (gamepad/keyboard)")]
        [SerializeField] ColorBlock m_navigation;
        public ColorBlock Navigation => m_navigation;

        void Reset()
        {
            var def = ColorBlock.defaultColorBlock;

            // Pointer mode: strong hover, subtle selected.
            var pointer = def;
            pointer.fadeDuration     = 0.08f;
            pointer.normalColor      = Color.white;
            pointer.highlightedColor = new Color(1f, 0.92f, 0.4f, 1f);
            pointer.pressedColor     = new Color(1f, 0.92f, 0.4f, 1f);
            pointer.selectedColor    = pointer.normalColor; // selection is subdued in pointer mode
            pointer.disabledColor    = new Color(1f, 1f, 1f, 0.5f);
            m_pointer = pointer;

            // Nav mode: emphasis on "Selected"; Hover ~= Normal (users don't hover).
            var nav = def;
            nav.fadeDuration         = 0.08f;
            nav.normalColor          = Color.white;
            nav.highlightedColor     = nav.normalColor;
            nav.selectedColor        = new Color(1f, 0.92f, 0.4f, 1f);
            nav.pressedColor         = nav.selectedColor;
            nav.disabledColor        = new Color(1f, 1f, 1f, 0.5f);
            m_navigation = nav;
        }
    }
}