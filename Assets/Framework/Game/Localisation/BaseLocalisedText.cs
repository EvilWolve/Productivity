// Base implementation taken from NGUI: Next-Gen UI kit by Tasharen Entertainment Inc

using UnityEngine;

/// <summary>
/// Simple script that lets you localise any text component.
/// </summary>

[ExecuteInEditMode]
public abstract class BaseLocalisedText : MonoBehaviour
{
	/// <summary>
	/// Localisation key.
	/// </summary>

	public string key;

	/// <summary>
	/// Manually change the value of whatever the localisation component is attached to.
	/// </summary>

	public string value
	{
		set
		{
			if (!string.IsNullOrEmpty(value))
			{
                this.UpdateText (value);
			}
		}
	}

    protected abstract void UpdateText(string value);

	/// <summary>
	/// Localise the component on enable, but only if it has been started already.
	/// </summary>

	void OnEnable ()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        this.RegisterEvents ();
		this.OnLocalise ();
    }

    void RegisterEvents()
    {
        Localisation.onLocalise += this.OnLocalise;
    }

    void OnDisable()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            return;
        }
#endif

        this.UnregisterEvents ();
    }

    void UnregisterEvents()
    {
        Localisation.onLocalise -= this.OnLocalise;
    }

    void OnLocalise ()
	{
		if (!string.IsNullOrEmpty(this.key))
        {
            this.value = Localisation.Get (this.key);
        }
	}
}
