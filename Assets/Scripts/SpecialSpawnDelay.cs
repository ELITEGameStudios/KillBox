using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialSpawnDelay : MonoBehaviour
{
    [SerializeField] private float delay;
    [SerializeField] private Vector3 size;
    [SerializeField] private GameObject entityToSpawn;
    [SerializeField] private bool customSize;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(delay <= 0){
            entityToSpawn = Instantiate(entityToSpawn, transform);
            entityToSpawn.transform.SetParent(null);
            entityToSpawn.transform.position = transform.position;
            
            if(customSize){
                entityToSpawn.transform.localScale = size;
            }

            Destroy(gameObject);
        }
        delay -= Time.deltaTime;
    }
}
