using UnityEngine;
using Unity.VisualScripting;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using EvieEngine.Einferia.Effects;

namespace EvieEngine.Einferia
{
    public class EinferiaDialogManager : MonoBehaviour
    {
        public static EinferiaDialogManager Instance { get; private set; }

        public TMP_Text defaultDialogName;
        public TMP_Text defaultDialogText;
        public Animator defaultAnimator;
        public GameObject defaultDialogSkip;

        public GameObject answerList;
        public GameObject answerItem;

        [HideInInspector]
        public TypewriterEffect typewriterEffect;

        private AudioSource audioSource1;
        private AudioSource audioSource2;
        private bool useFirstAudioSource = true; // Флаг для чередования аудиосурсов
        
        public float typewriterTime = 0.05f;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            DontDestroyOnLoad(gameObject);

            typewriterEffect = gameObject.AddComponent<TypewriterEffect>();

            // Создаем два аудиосурса
            audioSource1 = gameObject.AddComponent<AudioSource>();
            audioSource2 = gameObject.AddComponent<AudioSource>();
        }

        void Start()
        {
            if (defaultDialogName != null) defaultDialogName.textPreprocessor = new EvieEngine.TMPPreprocessor.Preprocessor();
            if (defaultDialogText != null) defaultDialogText.textPreprocessor = new EvieEngine.TMPPreprocessor.Preprocessor();

            StartDialog("dialogID_0");
        }

        public void StartDialog(string dialogID)
        {
            EventBus.Trigger(EventNames.OnDialogStarted, dialogID);
        }

        public void GenerateAnswerListUI(List<string> answers, Action<string> onAnswerSelected)
        {
            foreach (Transform child in answerList.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (string answer in answers)
            {
                GameObject newAnswerItem = Instantiate(answerItem, answerList.transform);

                newAnswerItem.GetComponentInChildren<TMPro.TextMeshProUGUI>().textPreprocessor = new EvieEngine.TMPPreprocessor.Preprocessor();

                newAnswerItem.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = answer;

                newAnswerItem.GetComponent<Button>().onClick.AddListener(() =>
                {
                    onAnswerSelected?.Invoke(answer);
                });
            }
        }

        public void ClearAnswerListUI()
        {
            foreach (Transform child in answerList.transform)
            {
                Destroy(child.gameObject);
            }
        }
        
        public void AnimatorTrigger(string triggerName)
        {
            if (defaultAnimator != null)
            {
                defaultAnimator.SetTrigger(triggerName);
            }
        }

        public void PlayDialogSound(string filename)
        {
            string path = $"Einferia/DialogManager/Audio/{filename}";
            AudioClip clip = Resources.Load<AudioClip>(path);

            if (clip == null)
            {
                Debug.LogError($"Audio file not found at path: {path}");
                return;
            }

            AudioSource currentAudioSource = useFirstAudioSource ? audioSource1 : audioSource2;
            currentAudioSource.clip = clip;
            currentAudioSource.Play();

            useFirstAudioSource = !useFirstAudioSource; // Чередуем аудиосурсы
        }
        
        public void DialogSoundStopActive()
        {
            audioSource1.Stop();
            audioSource2.Stop();
        }
        
        public bool GetCurrentAudioSourceIsPlaying()
        {
            AudioSource currentAudioSource = !useFirstAudioSource ? audioSource1 : audioSource2;

            if (currentAudioSource != null)
            {
                // Проверяем, играет ли аудио или оно завершилось
                if (currentAudioSource.isPlaying || currentAudioSource.time < currentAudioSource.clip.length)
                {
                    return true;
                }
            }

            return false;
        }
        
        public void ChangeDialogSkipVisible(bool isVisible)
        {
            defaultDialogSkip.SetActive(isVisible);
        }
    }
}