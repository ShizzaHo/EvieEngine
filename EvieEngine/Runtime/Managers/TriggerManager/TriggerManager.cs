using System.Collections.Generic;
using UnityEngine;

namespace EvieEngine.Triggers
{
    public class TriggerManager : MonoBehaviour
    {
        public static TriggerManager Instance { get; private set; }

        private Dictionary<string, bool> triggers = new Dictionary<string, bool>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        public void AddTrigger(string triggerName, bool initialState = false)
        {
            if (!triggers.ContainsKey(triggerName))
            {
                triggers[triggerName] = initialState;
            }
        }

        public void SetTriggerState(string triggerName, bool state)
        {
            if (triggers.ContainsKey(triggerName))
            {
                triggers[triggerName] = state;
            }
        }

        public bool GetTriggerState(string triggerName)
        {
            if (triggers.ContainsKey(triggerName))
            {
                return triggers[triggerName];
            }
            else
            {
                return false;
            }
        }

        public List<string> GetAllTriggers()
        {
            return new List<string>(triggers.Keys);
        }
    }
}