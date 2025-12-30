using System.Collections.Generic;
using UnityEngine;

namespace EvieEngine.Utils
{
    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool Instance { get; private set; }

        [System.Serializable]
        public class PoolCategory
        {
            public string key;
            public GameObject prefab;
            public int initialSize = 10;
        }

        public List<PoolCategory> categories = new List<PoolCategory>();

        private Dictionary<string, Queue<GameObject>> poolMap = new Dictionary<string, Queue<GameObject>>();
        private Dictionary<GameObject, string> reverseMap = new Dictionary<GameObject, string>();

        private void Awake()
        {
            if (Instance == null) Instance = this;

            foreach (var category in categories)
            {
                var queue = new Queue<GameObject>();
                for (int i = 0; i < category.initialSize; i++)
                {
                    var obj = CreateNewObject(category.key, category.prefab);
                    obj.SetActive(false);
                    queue.Enqueue(obj);
                }

                poolMap[category.key] = queue;
            }
        }

        private GameObject CreateNewObject(string key, GameObject prefab)
        {
            var obj = Instantiate(prefab, transform);
            reverseMap[obj] = key;
            return obj;
        }

        public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
        {
            if (!poolMap.ContainsKey(key))
            {
                Debug.LogError($"Pool with key '{key}' not found!");
                return null;
            }

            GameObject obj;
            if (poolMap[key].Count > 0)
            {
                obj = poolMap[key].Dequeue();
            }
            else
            {
                var cat = categories.Find(c => c.key == key);
                if (cat == null)
                {
                    Debug.LogError($"No prefab found for pool key '{key}'");
                    return null;
                }

                obj = CreateNewObject(key, cat.prefab);
            }

            obj.transform.SetPositionAndRotation(position, rotation);
            obj.SetActive(true);
            return obj;
        }
        
        public void Despawn(GameObject obj)
        {
            obj.SetActive(false);

            if (reverseMap.TryGetValue(obj, out string key))
            {
                poolMap[key].Enqueue(obj);
            }
            else
            {
                Destroy(obj); 
            }
        }
    }
}