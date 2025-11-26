using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> pooledObjects;
    public GameObject objectToPool;
    public int amountToPool;

    void Awake()
    {
        if(pooledObjects == null) pooledObjects = new();
    }
 
    void Start(){
        // pooledObjects = new List<GameObject>();
        //GameObject tmp;
        //for(int i = 0; i < amountToPool; i++){
        //    tmp = Instantiate(objectToPool);
        //    tmp.SetActive(false);
        //    pooledObjects.Add(tmp);
        //}
    }
    public void InitPool()
    {
        pooledObjects = new();
    }
    
    public void ClearPool(){
        pooledObjects = new List<GameObject>();
    }

    public GameObject GetPooledObject()
    {
        ClearMissingOrNull();

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

    public void ClearMissingOrNull()
    {
        for (int i = pooledObjects.Count-1; i >= 0; i--){
            try{
                if(pooledObjects[i] != null){continue;}
                pooledObjects.RemoveAt(i); // Object was null
            }
            catch (MissingReferenceException){
                pooledObjects.RemoveAt(i); // Object was missing
            }
        }
    }

    public List<GameObject> AllPooledObjects(){ClearMissingOrNull(); return pooledObjects;}
    public List<GameObject> GetPooledObjects(int count = 1)
    {
        ClearMissingOrNull();

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
}
