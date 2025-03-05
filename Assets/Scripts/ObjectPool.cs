using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static List<ObjectPool> objectPools {get; private set;}
    [SerializeField] private List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake(){
        if(objectPools == null){
            objectPools = new List<ObjectPool>();
        }

        objectPools.Add(this);
    
    }
 
    void Start(){
        pooledObjects = new List<GameObject>();
        //GameObject tmp;
        //for(int i = 0; i < amountToPool; i++){
        //    tmp = Instantiate(objectToPool);
        //    tmp.SetActive(false);
        //    pooledObjects.Add(tmp);
        //}
    }
    void ClearPool(){
        pooledObjects = new List<GameObject>();
    }

    public GameObject GetPooledObject()
    {

        //Check if one is found
        for(int i = 0; i < pooledObjects.Count; i++){
            if(pooledObjects[i] == null){
                continue;
            }
            if(!pooledObjects[i].activeInHierarchy){
                return pooledObjects[i];
            }
        }

        // If none are found

        //if theres a limit and that limit is reached, return null
        if(amountToPool != -1){
            if(pooledObjects.Count >= amountToPool){
                return null;
            }
        }

        //Generates another object then passes it
        
        GameObject tmp;
            tmp = Instantiate(objectToPool);
            tmp.SetActive(false);

            pooledObjects.Add(tmp);

            return tmp;
    }

    public List<GameObject> AllPooledObjects(){return pooledObjects;}
    public List<GameObject> GetPooledObjects(int count = 1)
    {
        bool found = false;

        List<GameObject> result = new List<GameObject>();

        for(int a = 0; a < count; a++){

            //Check if one is found
            for(int i = 0; i < pooledObjects.Count; i++){
                if(!pooledObjects[i].activeInHierarchy){

                    found = true;

                    result.Add( pooledObjects[i] );
                    break;
                }
            }

            if(found){
                found = false;
                continue;
            }

            // If none are found

            //if theres a limit and that limit is reached, return null
            if(amountToPool != -1){
                if(pooledObjects.Count >= amountToPool){
                    break;
                }
            }

            //Generates another object then passes it

            GameObject tmp;
                tmp = Instantiate(objectToPool);
                tmp.SetActive(false);

                pooledObjects.Add(tmp);

                result.Add(tmp);
        }

        return result;
    }

    public static void ResetAllPools(){
        foreach (ObjectPool pool in objectPools){
            foreach(GameObject obj in pool.AllPooledObjects()){
                if(obj != null){
                    Destroy(obj);
                }
                // obj.SetActive(false);
            }
            pool.ClearPool();
        }
    }
}
