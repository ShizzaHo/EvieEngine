using TMPro;
using UnityEngine;

namespace EvieEngine.Arhion.TMP
{
    public class TMPPreprocessorManager : MonoBehaviour
    {
        private Preprocessor preprocessor;
        
        public bool localizationEnabled = false;

        private void Awake()
        {
            if (localizationEnabled)
            {
                Localization.Initialize("ru");
            }
            
            preprocessor = new Preprocessor();
            ApplyPreprocessorToAllTMP();
        }

        private void ApplyPreprocessorToAllTMP()
        {
            var allTMPText = FindObjectsOfType<TMP_Text>();

            foreach (var tmpText in allTMPText)
            {
                tmpText.textPreprocessor = preprocessor;
            }
        }

        private void OnEnable()
        {
            ApplyPreprocessorToAllTMP();
        }
    }
}