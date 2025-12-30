using System;
using System.Collections.Generic;
using UnityEngine;

namespace EvieEngine.Messages
{
    public class MessageManager : MonoBehaviour
    {
        public static MessageManager Instance { get; private set; }

        private Dictionary<string, Delegate> messageDictionary = new Dictionary<string, Delegate>();

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Subscribe(string message, Action listener)
        {
            if (messageDictionary.ContainsKey(message))
            {
                if (messageDictionary[message] is Action existingAction)
                {
                    messageDictionary[message] = Delegate.Combine(existingAction, listener);
                }
            }
            else
            {
                messageDictionary[message] = listener;
            }
        }

        public void Unsubscribe(string message, Action listener)
        {
            if (messageDictionary.ContainsKey(message))
            {
                if (messageDictionary[message] is Action existingAction)
                {
                    messageDictionary[message] = Delegate.Remove(existingAction, listener);

                    if (messageDictionary[message] == null)
                    {
                        messageDictionary.Remove(message);
                    }
                }
            }
        }

        public void Subscribe<T>(string message, Action<T> listener)
        {
            if (messageDictionary.ContainsKey(message))
            {
                if (messageDictionary[message] is Action<T> existingAction)
                {
                    messageDictionary[message] = Delegate.Combine(existingAction, listener);
                }
            }
            else
            {
                messageDictionary[message] = listener;
            }
        }

        public void Unsubscribe<T>(string message, Action<T> listener)
        {
            if (messageDictionary.ContainsKey(message))
            {
                if (messageDictionary[message] is Action<T> existingAction)
                {
                    messageDictionary[message] = Delegate.Remove(existingAction, listener);

                    if (messageDictionary[message] == null)
                    {
                        messageDictionary.Remove(message);
                    }
                }
            }
        }

        public void SendMessage(string message)
        {
            if (messageDictionary.ContainsKey(message))
            {
                if (messageDictionary[message] is Action action)
                {
                    action.Invoke();
                }
            }
        }

        public void SendMessage<T>(string message, T arg)
        {
            if (messageDictionary.ContainsKey(message))
            {
                if (messageDictionary[message] is Action<T> action)
                {
                    action.Invoke(arg);
                }
            }
        }
    }

}