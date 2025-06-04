using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn3 : MonoBehaviour
{

    public GameObject[] cubos;
    public Transform spawnpoint;
    public int[] Rate, RateStep, RateStart, RateRound, clamp, falloffRnd, FalloffRate;
    public int EnemyIndex, CubosArrayLength, RateArrayLength;
    public float e, g, f;
    public GameManager gameManager;

    void Start()
    {

    }
    void Update()
    {
        for(int i = 0; i <= CubosArrayLength; i++)
        {
            RateRound[i] = gameManager.LvlCount - RateStart[i];
            Rate[i] = RateRound[i] * RateStep[i];
            if (Rate[i] > 100)
                Rate[i] = 100;
            Rate[i] = Mathf.Clamp(Rate[i], 0, clamp[i]);
            int FRxFR = RateRound[i] - falloffRnd[i];
            if (RateRound[i] > falloffRnd[i])
                Rate[i] -= FalloffRate[i] * FRxFR;
        }
    }
    public void InstantSpawn()
    {
        e = Random.Range(0, 100);

        for (int ii = 0; ii <= RateArrayLength; ii++)
        {
            if (e <= Rate[ii])
                EnemyIndex = ii + 1;
        }

        Instantiate(cubos[EnemyIndex], spawnpoint.position, spawnpoint.rotation);
        EnemyIndex = 0;
    }
}