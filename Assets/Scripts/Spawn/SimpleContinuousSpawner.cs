using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleContinuousSpawner : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] private Transform[] transforms;
    [SerializeField] private GameObject[] spawnCandadites;
    [SerializeField] private bool randomPositions, randomCandadites;
    [SerializeField] private float interval;

    [Header("Runtime variables")]
    [SerializeField] private float currentTime;
    [SerializeField] private int nextPosIndex, nextEnemyIndex;

    // Update is called once per frame
    void Update()
    {
        if (currentTime > 0) { currentTime -= Time.deltaTime; }
        else{ Spawn();  currentTime = interval; }
    }

    void Spawn()
    {
        Transform currentTransform;
        GameObject candadite;

        if (randomPositions) { currentTransform = transforms[Random.Range(0, transforms.Length)]; }
        else
        {
            currentTransform = transforms[nextPosIndex];

            if (nextPosIndex + 1 == transforms.Length) { nextPosIndex = 0; }
            else { nextPosIndex++; }
        }

        if (randomCandadites) { candadite = spawnCandadites[Random.Range(0, spawnCandadites.Length)]; }
        else
        {
            candadite = spawnCandadites[nextEnemyIndex];

            if (nextEnemyIndex + 1 == spawnCandadites.Length) { nextEnemyIndex = 0; }
            else { nextEnemyIndex++; }
        }

        Instantiate(candadite, currentTransform.position, currentTransform.rotation);

    }
}
