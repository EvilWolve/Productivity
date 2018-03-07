// Base implementation taken from NGUI: Next-Gen UI kit by Tasharen Entertainment Inc

using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple script that lets you localise a UGUI text.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(Text))]
public class LocalisedTextUGUI : BaseLocalisedText
{
    Text cachedText;

    protected override void UpdateText(string value)
    {
        if (this.cachedText == null)
        {
            this.cachedText = this.GetComponent<Text> ();
        }

        if (this.cachedText != null)
        {
            this.cachedText.text = value;
        }
    }
}
