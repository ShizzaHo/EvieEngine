using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

//DEPRECATED
namespace EvieEngine.TMPPreprocessor
{
    public class tmp_localizator : Ipreprocess
    {
        public string Format(string text)
        {
            var regex = new Regex(@"\{localize:(.*?)\}");
            var result = regex.Replace(text, match =>
            {
                string key = match.Groups[1].Value;
                string localizedText = Localization.GetLocalizedText(key);
                return localizedText;
            });

            return result;
        }
    }
    
    public static class Localization
    {
        private static Dictionary<string, string> localizedTexts = new Dictionary<string, string>();
        private static string currentLanguage;

        public static void Initialize(string langName)
        {
            SetLanguage(langName);
        }

        public static void SetLanguage(string langName)
        {
            currentLanguage = langName;
            localizedTexts.Clear();

            TextAsset jsonFile = Resources.Load<TextAsset>($"EvieEngine/Langs/{langName}");

            if (jsonFile != null)
            {
                localizedTexts = JsonUtility.FromJson<LocalizationData>(jsonFile.text).ToDictionary();
            }
        }

        public static string GetLocalizedText(string key)
        {
            if (localizedTexts.TryGetValue(key, out string localizedText))
            {
                return localizedText;
            }
            else
            {
                Debug.LogWarning($"Key '{key}' not found in localization for language '{currentLanguage}'.");
                return key;
            }
        }

        [System.Serializable]
        private class LocalizationData
        {
            public List<LocalizationEntry> entries;

            public Dictionary<string, string> ToDictionary()
            {
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var entry in entries)
                {
                    dict[entry.code] = entry.text;
                }
                return dict;
            }
        }

        [System.Serializable]
        private class LocalizationEntry
        {
            public string code;
            public string text;
        }
    }
}
