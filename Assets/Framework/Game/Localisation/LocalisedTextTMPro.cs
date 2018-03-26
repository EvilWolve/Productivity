// Base implementation taken from NGUI: Next-Gen UI kit by Tasharen Entertainment Inc

using UnityEngine;
using TMPro;

namespace framework.localisation
{
    /// <summary>
    /// Simple script that lets you localise a TextMeshPro text.
    /// </summary>
    [ExecuteInEditMode]
    [RequireComponent (typeof (TextMeshPro))]
    public class LocalisedTextTMPro : BaseLocalisedText
    {
        TextMeshPro cachedText;

        protected override void UpdateText(string value)
        {
            if (this.cachedText == null)
            {
                this.cachedText = this.GetComponent<TextMeshPro> ();
            }

            if (this.cachedText != null)
            {
                this.cachedText.text = value;
            }
        }
    }
}