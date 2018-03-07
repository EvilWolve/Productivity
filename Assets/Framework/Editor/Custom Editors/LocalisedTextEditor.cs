// Base implementation taken from NGUI: Next-Gen UI kit by Tasharen Entertainment Inc

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CanEditMultipleObjects]
[CustomEditor(typeof(BaseLocalisedText), true)]
public class LocalisedTextEditor : Editor
{
	List<string> keys;

	void OnEnable ()
	{
		Dictionary<string, string[]> dict = Localisation.Dictionary;

		if (dict.Count > 0)
		{
			this.keys = new List<string>();

			foreach (KeyValuePair<string, string[]> pair in dict)
			{
				if (pair.Key == "KEY") continue;
                this.keys.Add(pair.Key);
			}
            this.keys.Sort(delegate (string left, string right) { return left.CompareTo(right); });
		}
	}

	public override void OnInspectorGUI ()
	{
		this.serializedObject.Update();
		
		GUILayout.Space(6f);

		GUILayout.BeginHorizontal();
        // Key not found in the localisation file -- draw it as a text field
        SerializedProperty sp = this.serializedObject.FindProperty ("key");
        EditorGUILayout.PropertyField (sp);

		string myKey = sp.stringValue;
		bool isPresent = (this.keys != null) && this.keys.Contains(myKey);
		GUI.color = isPresent ? Color.green : Color.red;
		GUILayout.BeginVertical(GUILayout.Width(22f));
		GUILayout.Space(2f);
		GUILayout.Label(isPresent? "\u2714" : "\u2718", "TL SelectionButtonNew", GUILayout.Height(20f));
		GUILayout.EndVertical();
		GUI.color = Color.white;
		GUILayout.EndHorizontal();

		if (isPresent)
		{
			string[] languages = Localisation.knownLanguages;
			string[] translations;

			if (Localisation.Dictionary.TryGetValue(myKey, out translations))
			{
				if (languages.Length != translations.Length)
				{
					EditorGUILayout.HelpBox("Number of keys doesn't match the number of values! Did you modify the dictionaries by hand at some point?", MessageType.Error);
				}
				else
				{
					for (int i = 0; i < languages.Length; ++i)
					{
						GUILayout.BeginHorizontal();
						GUILayout.Label(languages[i], GUILayout.Width(66f));

						if (GUILayout.Button(translations[i], "TextArea", GUILayout.MinWidth(80f), GUILayout.MaxWidth(Screen.width - 110f)))
						{
							(this.target as BaseLocalisedText).value = translations[i];
							GUIUtility.hotControl = 0;
							GUIUtility.keyboardControl = 0;
						}
						GUILayout.EndHorizontal();
					}
				}
			}
			else
            {
                GUILayout.Label("No preview available");
            }
        }
		else if (this.keys != null && !string.IsNullOrEmpty(myKey))
		{
			GUILayout.BeginHorizontal();
			GUILayout.Space(80f);
			GUILayout.BeginVertical();
			GUI.backgroundColor = new Color(1f, 1f, 1f, 0.35f);

			int matches = 0;

			for (int i = 0, imax = this.keys.Count; i < imax; ++i)
			{
				if (this.keys[i].StartsWith(myKey, System.StringComparison.OrdinalIgnoreCase) || this.keys[i].Contains(myKey))
				{
					if (GUILayout.Button(this.keys[i] + " \u25B2", "CN CountBadge"))
					{
						sp.stringValue = this.keys[i];
						GUIUtility.hotControl = 0;
						GUIUtility.keyboardControl = 0;
					}
					
					if (++matches == 8)
					{
						GUILayout.Label("...and more");
						break;
					}
				}
			}
			GUI.backgroundColor = Color.white;
			GUILayout.EndVertical();
			GUILayout.Space(22f);
			GUILayout.EndHorizontal();
		}
		
		this.serializedObject.ApplyModifiedProperties();
	}
}
