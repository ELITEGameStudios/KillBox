using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpawnScript : MonoBehaviour
{
    public GameObject Enemy;
    public Transform spawnpoint;
    public bool allow, instant;
    public GameManager gameManager;
    public float SpawnTime;

    void Start()
    {
        
    }

    void Update()
    {

    }

    public void StartSpawnSequence()
    {
        allow = true;

        StartCoroutine(spawning());
        if (instant)
        {
            Instantiate(Enemy, spawnpoint.position, spawnpoint.rotation);
        }
    }
    void ReIterate()
    {
        if(allow)
        {
            StartCoroutine(spawning());
        }
    }

    IEnumerator spawning()
    {
        yield return new WaitForSeconds(SpawnTime);
        Instantiate(Enemy, spawnpoint.position, spawnpoint.rotation);
        ReIterate();
    }
}