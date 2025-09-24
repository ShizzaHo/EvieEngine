using System;
using UnityEngine;

namespace EvieEngine
{
    public class EvieEngine : MonoBehaviour
    {
        public bool DontDestroyOnLoad = true;

        private void Awake()
        {
            if (DontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
