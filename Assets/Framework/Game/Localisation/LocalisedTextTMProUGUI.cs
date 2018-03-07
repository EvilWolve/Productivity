// Base implementation taken from NGUI: Next-Gen UI kit by Tasharen Entertainment Inc

using UnityEngine;
using TMPro;

/// <summary>
/// Simple script that lets you localise a TextMeshPro UGUI text.
/// </summary>

[ExecuteInEditMode]
[RequireComponent (typeof (TextMeshProUGUI))]
public class LocalisedTextTMProUGUI : BaseLocalisedText
{
    TextMeshProUGUI cachedText;

    protected override void UpdateText(string value)
    {
        if (this.cachedText == null)
        {
            this.cachedText = this.GetComponent<TextMeshProUGUI> ();
        }

        if (this.cachedText != null)
        {
            this.cachedText.text = value;
        }
    }
}
