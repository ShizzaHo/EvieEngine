using System.Collections.Generic;
using UnityEngine;

namespace Eviecore
{
    public class StateManager : MonoBehaviour
    {
        public static StateManager Instance { get; private set; }

        [SerializeField]
        private List<string> states = new List<string>();

        private string currentState;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        private void Start()
        {
            SetState(states[0]);
        }

        public void SetState(string newState)
        {
            if (states.Contains(newState))
            {
                currentState = newState;
            }
            else
            {
                Debug.LogWarning($"[EVIECORE/STATEMANAGER/WARNING] The state '{newState}' is missing from the list of possible states.");
            }
        }

        public string GetCurrentState()
        {
            return currentState;
        }

        public bool IsCurrentState(string state)
        {
            return currentState == state;
        }

        public void AddState(string newState)
        {
            if (!states.Contains(newState))
            {
                states.Add(newState);
            }
            else
            {
                Debug.LogWarning($"[EVIECORE/STATEMANAGER/WARNING] The state '{newState}' already exists in the list.");
            }
        }
    }

}