using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility.Patterns;

public class ObjectPooler<T> : Singleton<ObjectPooler<T>> where T : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        public bool toBeInitialized = false;

        public List<T> objectPool;
    }

    [SerializeField] List<Pool> pools;
    public Dictionary<string, Pool> poolDictionary;

    protected override void Awake()
    {
        base.Awake();
        PopulatePools();
    }

    private void PopulatePools()
    {
        poolDictionary = new Dictionary<string, Pool>();

        foreach (Pool pool in pools)
        {
            pool.objectPool = new List<T>(pool.size);
            poolDictionary.Add(pool.tag, pool);

            if (!pool.toBeInitialized) continue;

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                obj.SetActive(false);
                pool.objectPool.Add(obj.GetComponent<T>());
            }
        }
    }

    public static void DeactivateAll()
    {
        foreach (var pool in _instance.pools)
        {
            foreach (var obj in pool.objectPool)
                obj.gameObject.SetActive(false);
        }
    }

    public static T Spawn(string tag, Vector3 position, Quaternion rotation, Transform parent)
    {
        return _instance.SpawnOrInstantiate(tag, position, rotation, false, parent);
    }

    private T SpawnOrInstantiate(string tag, Vector3 position, Quaternion rotation, bool local, Transform parent = null)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
#if UNITY_EDITOR
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
#else
            throw new System.NullReferenceException("Pool with tag " + tag + " doesn't exist.");
#endif
        }

        T objectToSpawn = GetInactiveFrom(poolDictionary[tag]);

        if (objectToSpawn == null)
            objectToSpawn = AddTo(poolDictionary[tag]);

        if (local)
            objectToSpawn.transform.localPosition = position;
        else
            objectToSpawn.transform.position = position;

        objectToSpawn.transform.rotation = rotation;

        objectToSpawn.gameObject.SetActive(true);

        objectToSpawn.transform.parent = parent ? parent : null;

        return objectToSpawn;


        T GetInactiveFrom(Pool pool)
        {
            foreach (var obj in pool.objectPool)
            {
                if (!obj.gameObject.activeInHierarchy)
                    return obj;
            }
            return null;
        }

        T AddTo(Pool pool)
        {
            var obj = Instantiate(pool.prefab, transform).GetComponent<T>();
            pool.objectPool.Add(obj);

            return obj;
        }
    }
}
