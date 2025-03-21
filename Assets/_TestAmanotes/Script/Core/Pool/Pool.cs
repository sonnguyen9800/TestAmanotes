using System.Collections.Generic;
using UnityEngine;

namespace TestAmanotes
{
    public class ObjectPool : MonoBehaviour
    {
        [System.Serializable]
        public class Pool
        {
            public string tag;
            public GameObject prefab;
            public int size;
        }

        public static ObjectPool Instance;

        private void Awake()
        {
            Instance = this;
        }

        public List<Pool> pools;
        private Dictionary<string, Queue<GameObject>> poolDictionary;

        public GameObject GetPrefabByTag(string tag)
        {
            foreach (var pool in pools)
            {
                if (pool.tag == tag)
                {
                    return pool.prefab;
                }
            }

            return null;
        }
        private void Start()
        {
            poolDictionary = new Dictionary<string, Queue<GameObject>>();

            foreach (var pool in pools)
            {
                Queue<GameObject> objectQueue = new Queue<GameObject>();

                for (int i = 0; i < pool.size; i++)
                {
                    GameObject obj = Instantiate(pool.prefab);
                    obj.SetActive(false);
                    objectQueue.Enqueue(obj);
                }

                poolDictionary.Add(pool.tag, objectQueue);
            }
        }

        public GameObject SpawnFromPool(string tag, Vector3 position, Transform parent,Quaternion rotation)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return null;
            }

            GameObject objectToSpawn = poolDictionary[tag].Dequeue();

            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;

            objectToSpawn.transform.SetParent(parent);
            IPoolable pooledObj = objectToSpawn.GetComponent<IPoolable>();
            pooledObj?.OnObjectSpawn();

            poolDictionary[tag].Enqueue(objectToSpawn);
            return objectToSpawn;
        }
        
        public void DespawnToPool(string tag, GameObject obj)
        {
            if (!poolDictionary.ContainsKey(tag))
            {
                Debug.LogWarning($"Pool with tag {tag} doesn't exist.");
                return;
            }

            IPoolable pooledObj = obj.GetComponent<IPoolable>();
            pooledObj.OnObjectDisabled();
            obj.SetActive(false);
            poolDictionary[tag].Enqueue(obj);
        }
    }
}