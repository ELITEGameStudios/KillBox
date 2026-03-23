using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public List<PoolData> objectPools;
    public static ObjectPoolManager instance {get; private set;}

    [Header("Global Objects (Use if callers dont have a reference)")]
    
    public GameObject explosionObject;
    public GameObject playerBullet;



    public static List<PoolData> GetAllPools(){ return instance.objectPools; } 

    [System.Serializable]
    public struct PoolData
    {
        public ObjectPool pool;
        public GameObject instancedObject;
        public string poolName;
        public int amountToPool;
    }

    void Awake()
    {
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}

        if(objectPools == null){objectPools = new();}
        else
        {
            // Not sure why i have to do this but apparently the language doesnt like common sense
            PoolData[] array = objectPools.ToArray();

            for (int i = 0; i < array.Length; i++)
            {
                array[i].pool = InitObjectPool(objectPools[i]);
            }

            // Not sure why i have to do this but apparently the language doesnt like common sense
            objectPools = array.ToList();
        }
    }

    public GameObject InstantiateFromPool(GameObject gameObject, Transform transform)
    {
        GameObject newObject = GetObjectFromPool(gameObject);

        newObject.transform.SetParent(transform);
        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localEulerAngles = Vector3.zero;
        newObject.SetActive(false); newObject.SetActive(true);
        return newObject;
    }

    public GameObject InstantiateFromPool(GameObject gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = GetObjectFromPool(gameObject);

        newObject.transform.SetParent(null);
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.SetActive(false); newObject.SetActive(true);
        return newObject;
    }

    public GameObject InstantiateFromPool(string gameObject, Vector3 position, Quaternion rotation)
    {
        GameObject newObject = GetObjectFromPool(gameObject);

        newObject.transform.SetParent(null);
        newObject.transform.position = position;
        newObject.transform.rotation = rotation;
        newObject.SetActive(false); newObject.SetActive(true);
        return newObject;
    }

    public static GameObject GetObjectFromPool(GameObject requestedGameobject)
    {
        GameObject retrievedObject;
        
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].poolName == requestedGameobject.name)
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

    public static GameObject GetObjectFromPool(string requestedGameobject)
    {
        GameObject retrievedObject;
        
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].poolName == requestedGameobject)
            {
                // Return from existing pool if pool exists
                retrievedObject = instance.objectPools[i].pool.GetPooledObject();
                retrievedObject.SetActive(true);
                return retrievedObject;
            }
        }

        // Otherwise return null. This method should only be used on pre-initialized pools since this cant create a new pool
        return null;
    }

    public static List<GameObject> GetObjectsFromPool(GameObject requestedGameobject, int amount)
    {
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].poolName == requestedGameobject.name)
            {
                // Return from existing pool if pool exists
                return instance.objectPools[i].pool.GetPooledObjects(amount);
            }
        }

        // Otherwise return from a new pool
        return instance.CreatePool(requestedGameobject).pool.GetPooledObjects(amount);
    }

    public static List<GameObject> GetObjectsFromPool(string requestedName, int amount)
    {
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].poolName == requestedName)
            {
                // Return from existing pool if pool exists
                return instance.objectPools[i].pool.GetPooledObjects(amount);
            }
        }

        // Otherwise return null. This method should only be used on pre-initialized pools since this cant create a new pool
        return null;
    }

    public PoolData CreatePool(GameObject newObject, int amountToPool = -1)
    {
        PoolData newPool;
        newPool.instancedObject = newObject;
        newPool.amountToPool = amountToPool;
        newPool.poolName = newObject.name;
        
        newPool.pool = gameObject.AddComponent<ObjectPool>();
        newPool.pool.objectToPool = newObject;
        newPool.pool.amountToPool = amountToPool;
        newPool.pool.InitPool();

        objectPools.Add(newPool);
        return newPool;
    }

    public ObjectPool InitObjectPool(PoolData data)
    {
        ObjectPool pool = gameObject.AddComponent<ObjectPool>();
        pool.amountToPool = data.amountToPool;
        pool.objectToPool = data.instancedObject;
        pool.InitPool();
        return pool;
    }

    public static ObjectPool GetPool(string poolName) // Please only use if the pool is initialized in the unity inspector
    {
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].poolName == poolName)
            {
                // Return from existing pool if pool exists
                return instance.objectPools[i].pool;
            }
        }
        
        return null;
    }

    public static bool PoolExists(string name)
    {
        for (int i = 0; i < instance.objectPools.Count; i++)
        {
            if(instance.objectPools[i].poolName == name)
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
