using System;
using System.Collections.Generic;
using EvieEngine.messages;
using TriInspector;
using UnityEngine;
using UnityEngine.Events;

namespace EvieEngine.trigger
{

    [Serializable]
    public class TriggerState
    {
        public string triggerName;
        public bool isActive;
    }

    public class TriggerZone : MonoBehaviour
    {
        [Title("Генеральные настройки")]
        [SerializeField]
        private bool isActive = true;

        [Title("Настройка тегов")]
        [SerializeField]
        private List<string> validTags = new List<string>();

        [Header("Настройка проверки триггеров")]
        [Header("Настройка входа в триггер (Entering)")]
        [SerializeField]
        private bool CheckWhenEntering = false;

        [Header("Настройка выхода из триггера (Exiting)")]
        [SerializeField]
        private bool CheckWhenExiting = false;

        [Title("Действия при активации триггера")]
        [Header("Выполнять действия через Action")]
        [SerializeField]
        private bool executeAction = false;

        [SerializeField]
        [ShowIf("executeAction")]
        private UnityEvent executeActionEnter;

        [SerializeField]
        [ShowIf("executeAction")]
        private UnityEvent executeActionExit;

        [Title("Отправка сообщений")]
        [SerializeField]
        private bool sendMessage = false;

        [SerializeField]
        [ShowIf("sendMessage")]
        private string messageEnter;

        [SerializeField]
        [ShowIf("sendMessage")]
        private string messageExit;

        [Header("Проверка необходимых триггеров")]
        [SerializeField]
        private List<TriggerState> needTriggersState = new List<TriggerState>();

        private void OnTriggerEnter(Collider other)
        {
            if (!isActive || !IsValidTag(other.tag)) return;

            // Check flag for entering trigger
            if (CheckWhenEntering)
            {
                if (AreAllTriggersMatching())
                {
                    ExecuteTriggerLogic(other, true);
                }
            }
            else
            {
                ExecuteTriggerLogic(other, true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // Check flag for exiting trigger
            if (CheckWhenExiting)
            {
                if (AreAllTriggersMatching())
                {
                    ExecuteTriggerLogic(other, false);
                }
            }
            else
            {
                ExecuteTriggerLogic(other, false);
            }
        }

        private bool IsValidTag(string tag)
        {
            if (validTags.Count == 0) return true; // If no tags are specified, allow all tags
            return validTags.Contains(tag);
        }

        private bool AreAllTriggersMatching()
        {
            foreach (var trigger in needTriggersState)
            {
                bool globalState = TriggerManager.Instance.GetTriggerState(trigger.triggerName);
            }
            return true;
        }

        private void ExecuteTriggerLogic(Collider other, bool isEnter)
        {
            if (sendMessage)
            {
                if (isEnter && !string.IsNullOrEmpty(messageEnter))
                {
                    MessageManager.Instance.SendMessage(messageEnter);
                }
                else if (!isEnter && !string.IsNullOrEmpty(messageExit))
                {
                    MessageManager.Instance.SendMessage(messageExit);
                }
            }

            if (executeAction)
            {
                if (isEnter)
                {
                    executeActionEnter?.Invoke();
                }
                else
                {
                    executeActionExit?.Invoke();
                }
            }
        }
    }
}