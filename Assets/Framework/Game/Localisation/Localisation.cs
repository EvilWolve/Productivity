/// Base implementation taken from NGUI: Next-Gen UI kit by Tasharen Entertainment Inc

using UnityEngine;
using System.Collections.Generic;

namespace framework.localisation
{
    /// <summary>
    /// Localisation manager is able to parse localisation information from text assets.
    /// Using it is simple: text = Localisation.Get(key), or just add a BaseLocalisedText script to your labels.
    /// You can switch the language by using Localisation.language = "French", for example.
    /// This will attempt to load the file called "French.txt" in the Resources folder,
    /// or a column "French" from the Localisation.csv file in the Resources folder.
    /// If going down the TXT language file route, it's expected that the file is full of key = value pairs, like so:
    /// 
    /// LABEL1 = Hello
    /// LABEL2 = Music
    /// Info = Localisation Example
    /// 
    /// In the case of the CSV file, the first column should be the "KEY". Other columns
    /// should be your localised text values, such as "French" for the first row:
    /// 
    /// KEY,English,French
    /// LABEL1,Hello,Bonjour
    /// LABEL2,Music,Musique
    /// Info,"Localisation Example","Par exemple la localisation"
    /// </summary>
    static public class Localisation
    {
        public delegate byte[] LoadFunction(string path);
        public delegate void OnLocaliseNotification();

        /// <summary>
        /// Want to have Localisation loading be custom instead of just Resources.Load? Set this function.
        /// </summary>

        static public LoadFunction loadFunction;

        /// <summary>
        /// Notification triggered when the localisation data gets changed, such as when changing the language.
        /// If you want to make modifications to the localisation data after it was loaded, this is the place.
        /// </summary>

        static public OnLocaliseNotification onLocalise;

        /// <summary>
        /// Whether the localisation dictionary has been loaded.
        /// </summary>

        static public bool localisationHasBeenSet = false;

        // Loaded languages, if any
        static string[] languages = null;

        // Key = Value dictionary (single language)
        static Dictionary<string, string> oldDictionary = new Dictionary<string, string> ();

        // Key = Values dictionary (multiple languages)
        static Dictionary<string, string[]> dictionary = new Dictionary<string, string[]> ();

        // Replacement dictionary forces a specific value instead of the existing entry
        static Dictionary<string, string> replacement = new Dictionary<string, string> ();

        // Index of the selected language within the multi-language dictionary
        static int languageIndex = -1;

        // Currently selected language
        static string language;

        /// <summary>
        /// Localisation dictionary. Dictionary key is the localisation key.
        /// Dictionary value is the list of localised values (columns in the CSV file).
        /// </summary>

        static public Dictionary<string, string[]> Dictionary
        {
            get
            {
                if (!localisationHasBeenSet)
                    LoadDictionary (PlayerPrefs.GetString ("Language", "English"));
                return dictionary;
            }
            set
            {
                localisationHasBeenSet = (value != null);
                dictionary = value;
            }
        }

        /// <summary>
        /// List of loaded languages. Available if a single Localisation.csv file was used.
        /// </summary>

        static public string[] knownLanguages
        {
            get
            {
                if (!localisationHasBeenSet)
                    LoadDictionary (PlayerPrefs.GetString ("Language", "English"));
                return languages;
            }
        }

        /// <summary>
        /// Name of the currently active language.
        /// </summary>

        static public string Language
        {
            get
            {
                if (string.IsNullOrEmpty (language))
                {
                    language = PlayerPrefs.GetString ("Language", "English");
                    LoadAndSelect (language);
                }
                return language;
            }
            set
            {
                if (language != value)
                {
                    language = value;
                    LoadAndSelect (value);
                }
            }
        }

        /// <summary>
        /// Load the specified localisation dictionary.
        /// </summary>

        static bool LoadDictionary(string value)
        {
            // Try to load the Localisation CSV
            byte[] bytes = null;

            if (!localisationHasBeenSet)
            {
                if (loadFunction == null)
                {
                    TextAsset asset = Resources.Load<TextAsset> ("Localisation/Main");
                    if (asset != null)
                        bytes = asset.bytes;
                }
                else
                    bytes = loadFunction ("Localisation");
                localisationHasBeenSet = true;
            }

            // Try to load the localisation file
            if (LoadCSV (bytes))
                return true;
            if (string.IsNullOrEmpty (value))
                value = language;

            // If this point was reached, the localisation file was not present
            if (string.IsNullOrEmpty (value))
                return false;

            // Not a referenced asset -- try to load it dynamically
            if (loadFunction == null)
            {
                TextAsset asset = Resources.Load<TextAsset> (value);
                if (asset != null)
                    bytes = asset.bytes;
            }
            else
                bytes = loadFunction (value);

            if (bytes != null)
            {
                Set (value, bytes);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Load the specified language.
        /// </summary>

        static bool LoadAndSelect(string value)
        {
            if (!string.IsNullOrEmpty (value))
            {
                if (dictionary.Count == 0 && !LoadDictionary (value))
                    return false;
                if (SelectLanguage (value))
                    return true;
            }

            // Old style dictionary
            if (oldDictionary.Count > 0)
                return true;

            // Either the language is null, or it wasn't found
            oldDictionary.Clear ();
            dictionary.Clear ();
            if (string.IsNullOrEmpty (value))
                PlayerPrefs.DeleteKey ("Language");
            return false;
        }

        /// <summary>
        /// Load the specified asset and activate the localisation.
        /// </summary>

        static public void Load(TextAsset asset)
        {
            ByteReader reader = new ByteReader (asset);
            Set (asset.name, reader.ReadDictionary ());
        }

        /// <summary>
        /// Set the localisation data directly.
        /// </summary>

        static public void Set(string languageName, byte[] bytes)
        {
            ByteReader reader = new ByteReader (bytes);
            Set (languageName, reader.ReadDictionary ());
        }

        /// <summary>
        /// Forcefully replace the specified key with another value.
        /// </summary>

        static public void ReplaceKey(string key, string val)
        {
            if (!string.IsNullOrEmpty (val))
                replacement[key] = val;
            else
                replacement.Remove (key);
        }

        /// <summary>
        /// Clear the replacement values.
        /// </summary>

        static public void ClearReplacements() { replacement.Clear (); }

        /// <summary>
        /// Load the specified CSV file.
        /// </summary>

        static public bool LoadCSV(TextAsset asset, bool merge = false) { return LoadCSV (asset.bytes, asset, merge); }

        /// <summary>
        /// Load the specified CSV file.
        /// </summary>

        static public bool LoadCSV(byte[] bytes, bool merge = false) { return LoadCSV (bytes, null, merge); }

        static bool mMerging = false;

        /// <summary>
        /// Whether the specified language is present in the localisation.
        /// </summary>

        static bool HasLanguage(string languageName)
        {
            for (int i = 0, imax = languages.Length; i < imax; ++i)
                if (languages[i] == languageName)
                    return true;
            return false;
        }

        /// <summary>
        /// Load the specified CSV file.
        /// </summary>

        static bool LoadCSV(byte[] bytes, TextAsset asset, bool merge = false)
        {
            if (bytes == null)
                return false;
            ByteReader reader = new ByteReader (bytes);

            // The first line should contain "KEY", followed by languages.
            BetterList<string> header = reader.ReadCSV ();

            // There must be at least two columns in a valid CSV file
            if (header.size < 2)
                return false;
            header.RemoveAt (0);

            string[] languagesToAdd = null;
            if (string.IsNullOrEmpty (language))
                localisationHasBeenSet = false;

            // Clear the dictionary
            if (!localisationHasBeenSet || (!merge && !mMerging) || languages == null || languages.Length == 0)
            {
                dictionary.Clear ();
                languages = new string[header.size];

                if (!localisationHasBeenSet)
                {
                    language = PlayerPrefs.GetString ("Language", header[0]);
                    localisationHasBeenSet = true;
                }

                for (int i = 0; i < header.size; ++i)
                {
                    languages[i] = header[i];
                    if (languages[i] == language)
                        languageIndex = i;
                }
            }
            else
            {
                languagesToAdd = new string[header.size];
                for (int i = 0; i < header.size; ++i)
                    languagesToAdd[i] = header[i];

                // Automatically resize the existing languages and add the new language to the mix
                for (int i = 0; i < header.size; ++i)
                {
                    if (!HasLanguage (header[i]))
                    {
                        int newSize = languages.Length + 1;
                        System.Array.Resize (ref languages, newSize);
                        languages[newSize - 1] = header[i];

                        Dictionary<string, string[]> newDict = new Dictionary<string, string[]> ();

                        foreach (KeyValuePair<string, string[]> pair in dictionary)
                        {
                            string[] arr = pair.Value;
                            System.Array.Resize (ref arr, newSize);
                            arr[newSize - 1] = arr[0];
                            newDict.Add (pair.Key, arr);
                        }
                        dictionary = newDict;
                    }
                }
            }

            Dictionary<string, int> languageIndices = new Dictionary<string, int> ();
            for (int i = 0; i < languages.Length; ++i)
                languageIndices.Add (languages[i], i);

            // Read the entire CSV file into memory
            for (; ; )
            {
                BetterList<string> temp = reader.ReadCSV ();
                if (temp == null || temp.size == 0)
                    break;
                if (string.IsNullOrEmpty (temp[0]))
                    continue;
                AddCSV (temp, languagesToAdd, languageIndices);
            }

            if (!mMerging && onLocalise != null)
            {
                mMerging = true;
                OnLocaliseNotification note = onLocalise;
                onLocalise = null;
                note ();
                onLocalise = note;
                mMerging = false;
            }
            return true;
        }

        /// <summary>
        /// Helper function that adds a single line from a CSV file to the localisation list.
        /// </summary>

        static void AddCSV(BetterList<string> newValues, string[] newLanguages, Dictionary<string, int> languageIndices)
        {
            if (newValues.size < 2)
                return;
            string key = newValues[0];
            if (string.IsNullOrEmpty (key))
                return;
            string[] copy = ExtractStrings (newValues, newLanguages, languageIndices);

            if (dictionary.ContainsKey (key))
            {
                dictionary[key] = copy;
                if (newLanguages == null)
                    Debug.LogWarning ("Localisation key '" + key + "' is already present");
            }
            else
            {
                try
                {
                    dictionary.Add (key, copy);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError ("Unable to add '" + key + "' to the Localisation dictionary.\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Used to merge separate localisation files into one.
        /// </summary>

        static string[] ExtractStrings(BetterList<string> added, string[] newLanguages, Dictionary<string, int> languageIndices)
        {
            if (newLanguages == null)
            {
                string[] values = new string[languages.Length];
                for (int i = 1, max = Mathf.Min (added.size, values.Length + 1); i < max; ++i)
                    values[i - 1] = added[i];
                return values;
            }
            else
            {
                string[] values;
                string s = added[0];

                if (!dictionary.TryGetValue (s, out values))
                    values = new string[languages.Length];

                for (int i = 0, imax = newLanguages.Length; i < imax; ++i)
                {
                    string language = newLanguages[i];
                    int index = languageIndices[language];
                    values[index] = added[i + 1];
                }
                return values;
            }
        }

        /// <summary>
        /// Select the specified language from the previously loaded CSV file.
        /// </summary>

        static bool SelectLanguage(string language)
        {
            languageIndex = -1;

            if (dictionary.Count == 0)
                return false;

            for (int i = 0, imax = languages.Length; i < imax; ++i)
            {
                if (languages[i] == language)
                {
                    oldDictionary.Clear ();
                    languageIndex = i;
                    Localisation.language = language;
                    PlayerPrefs.SetString ("Language", Localisation.language);
                    if (onLocalise != null)
                        onLocalise ();
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Load the specified asset and activate the localisation.
        /// </summary>

        static public void Set(string languageName, Dictionary<string, string> dictionary)
        {
            language = languageName;
            PlayerPrefs.SetString ("Language", language);
            oldDictionary = dictionary;
            localisationHasBeenSet = true;
            languageIndex = -1;
            languages = new string[] { languageName };
            if (onLocalise != null)
                onLocalise ();
        }

        /// <summary>
        /// Change or set the localisation value for the specified key.
        /// Note that this method only supports one fallback language, and should
        /// ideally be called from within Localisation.onLocalise.
        /// To set the multi-language value just modify Localisation.dictionary directly.
        /// </summary>

        static public void Set(string key, string value)
        {
            if (oldDictionary.ContainsKey (key))
                oldDictionary[key] = value;
            else
                oldDictionary.Add (key, value);
        }

        /// <summary>
        /// Localise the specified value.
        /// </summary>

        static public string Get(string key, bool warnIfMissing = true)
        {
            if (string.IsNullOrEmpty (key))
                return null;

            // Ensure we have a language to work with
            if (!localisationHasBeenSet)
                LoadDictionary (PlayerPrefs.GetString ("Language", "English"));

            if (languages == null)
            {
                Debug.LogError ("No localisation data present");
                return null;
            }

            string lang = Language;

            if (languageIndex == -1)
            {
                for (int i = 0; i < languages.Length; ++i)
                {
                    if (languages[i] == lang)
                    {
                        languageIndex = i;
                        break;
                    }
                }
            }

            if (languageIndex == -1)
            {
                languageIndex = 0;
                language = languages[0];
                Debug.LogWarning ("Language not found: " + lang);
            }

            string val;
            string[] vals;

            if (replacement.TryGetValue (key, out val))
                return val;

            if (languageIndex != -1 && dictionary.TryGetValue (key, out vals))
            {
                if (languageIndex < vals.Length)
                {
                    string s = vals[languageIndex];
                    if (string.IsNullOrEmpty (s))
                        s = vals[0];
                    return s;
                }
                return vals[0];
            }
            if (oldDictionary.TryGetValue (key, out val))
                return val;

#if UNITY_EDITOR
            if (warnIfMissing)
                Debug.LogWarning ("Localisation key not found: '" + key + "' for language " + lang);
#endif
            return key;
        }

        /// <summary>
        /// Localise the specified value and format it.
        /// </summary>

        static public string Format(string key, params object[] parameters) { return string.Format (Get (key), parameters); }

        [System.Obsolete ("Localisation is now always active. You no longer need to check this property.")]
        static public bool isActive { get { return true; } }

        [System.Obsolete ("Use Localisation.Get instead")]
        static public string Localise(string key) { return Get (key); }

        /// <summary>
        /// Returns whether the specified key is present in the localisation dictionary.
        /// </summary>

        static public bool Exists(string key)
        {
            // Ensure we have a language to work with
            if (!localisationHasBeenSet)
                Language = PlayerPrefs.GetString ("Language", "English");

#if UNITY_IPHONE || UNITY_ANDROID
            string mobKey = key + " Mobile";
            if (dictionary.ContainsKey (mobKey))
                return true;
            else if (oldDictionary.ContainsKey (mobKey))
                return true;
#endif
            return dictionary.ContainsKey (key) || oldDictionary.ContainsKey (key);
        }

        /// <summary>
        /// Add a new entry to the localisation dictionary.
        /// </summary>

        static public void Set(string language, string key, string text)
        {
            // Check existing languages first
            string[] kl = knownLanguages;

            if (kl == null)
            {
                languages = new string[] { language };
                kl = languages;
            }

            for (int i = 0, imax = kl.Length; i < imax; ++i)
            {
                // Language match
                if (kl[i] == language)
                {
                    string[] vals;

                    // Get all language values for the desired key
                    if (!dictionary.TryGetValue (key, out vals))
                    {
                        vals = new string[kl.Length];
                        dictionary[key] = vals;
                        vals[0] = text;
                    }

                    // Assign the value for this language
                    vals[i] = text;
                    return;
                }
            }

            // Expand the dictionary to include this new language
            int newSize = languages.Length + 1;
            System.Array.Resize (ref languages, newSize);
            languages[newSize - 1] = language;

            Dictionary<string, string[]> newDict = new Dictionary<string, string[]> ();

            foreach (KeyValuePair<string, string[]> pair in dictionary)
            {
                string[] arr = pair.Value;
                System.Array.Resize (ref arr, newSize);
                arr[newSize - 1] = arr[0];
                newDict.Add (pair.Key, arr);
            }
            dictionary = newDict;

            // Set the new value
            string[] values;

            if (!dictionary.TryGetValue (key, out values))
            {
                values = new string[kl.Length];
                dictionary[key] = values;
                values[0] = text;
            }
            values[newSize - 1] = text;
        }
    }
}