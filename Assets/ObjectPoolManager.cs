using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public List<PoolData> objectPools;
    public static ObjectPoolManager instance {get; private set;}

    [Header("Global Objects (Use if callers dont have a reference)")]
    public GameObject tokenPrefab;
    public GameObject explosionObject;





    public static List<PoolData> GetAllPools(){ return instance.objectPools; } 

    [System.Serializable]
    public struct PoolData
    {
        public ObjectPool pool;
        public GameObject instancedObject;
        public string name;
        public int amountToPool;
    }

    void Awake()
    {
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
    }

    public GameObject InstantiateFromPool(GameObject gameObject, Transform transform)
    {
        GameObject newObject = GetObjectFromPool(gameObject);

        newObject.transform.SetParent(transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localEulerAngles = Vector3.zero;

        return newObject;
    }

    public GameObject InstantiateFromPool(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = GetObjectFromPool(gameObject);

        newObject.transform.SetParent(null);
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;

        return newObject;
    }

    public static GameObject GetObjectFromPool(GameObject requestedGameobject)
    {
        GameObject retrievedObject;
        
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].name == requestedGameobject.name)
            {
                // Return from existing pool if pool exists
                retrievedObject = instance.objectPools[i].pool.GetPooledObject();
                retrievedObject.SetActive(true);
                return retrievedObject;
            }
        }

        // Otherwise return from a new pool
        retrievedObject = instance.CreatePool(requestedGameobject).pool.GetPooledObject();
        retrievedObject.SetActive(true);
        return retrievedObject;
    }

    public static List<GameObject> GetObjectsFromPool(GameObject requestedGameobject, int amount)
    {
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].name == requestedGameobject.name)
            {
                // Return from existing pool if pool exists
                return instance.objectPools[i].pool.GetPooledObjects(amount);
            }
        }

        // Otherwise return from a new pool
        return instance.CreatePool(requestedGameobject).pool.GetPooledObjects(amount);
    }

    public PoolData CreatePool(GameObject newObject, int amountToPool = -1)
    {
        PoolData newPool;
        newPool.pool = gameObject.AddComponent<ObjectPool>();
        newPool.instancedObject = newObject;
        newPool.amountToPool = amountToPool;
        newPool.name = gameObject.name;

        objectPools.Add(newPool);
        return newPool;
    }

    public static bool PoolExists(string name)
    {
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].name == name)
            {
                return true;
            }
        }
        
        return false;
    }

    public static bool PoolExists(GameObject gameObject){
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].instancedObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    public static void ResetAllPools(){
        foreach (PoolData pool in instance.objectPools){
            foreach(GameObject obj in pool.pool.AllPooledObjects()){
                if(obj != null){
                    Destroy(obj);
                }
                // obj.SetActive(false);
            }
            pool.pool.ClearPool();
        }
    }
}
