using System;
using TriInspector;
using UnityEngine;

namespace EvieEngine
{
    public class EvieEngineGlobalObject : MonoBehaviour
    {
        [Title("Не удалять объект при смене сцены")]
        public bool DontDestroyOnLoad = true;
        [Title("Инициализировать EvieFS")]
        public bool InitializeEvieFileSystem = false;

        private void Awake()
        {
            if (DontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            if (InitializeEvieFileSystem)
            {
                FileSystem.Initialize();
            }
        }
    }
}
