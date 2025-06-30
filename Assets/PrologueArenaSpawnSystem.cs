using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueArenaSpawnSystem : MonoBehaviour
{

    public static PrologueArenaSpawnSystem instance {get; private set;}
    [SerializeField] private List<Transform> spawnTransforms;

    void Awake()
    {
        if (instance == null) { instance = this; }
        else if(instance != this){ Destroy(this); }
    }

    public void SpawnEnemies(GameObject prefab, float cycles, float time, float startAngle, bool clockwise = true)
    {
        StartCoroutine(SpawnCoroutine(prefab, cycles, time, startAngle, clockwise));
    }

    IEnumerator SpawnCoroutine(GameObject prefab, float cycles, float time, float startAngle, bool clockwise = true)
    {
        // Setup
        int spawns = (int)(spawnTransforms.Count * cycles);
        float interval = time / spawns;

        int currentIndex = 0;
        float winningDiff = Mathf.Abs(startAngle - Vector2.Angle(Vector2.zero, spawnTransforms[0].position));

        for (int i = 0; i < spawnTransforms.Count; i++)
        {
            float difference = Mathf.Abs(startAngle - Vector2.Angle(Vector2.zero, spawnTransforms[i].position));
            if (difference < winningDiff)
            {
                currentIndex = i;
                winningDiff = difference;
            }
        }

        // Spawning Operation
        while (spawns > 0)
        {
            GameObject newEnemy = Instantiate(prefab, spawnTransforms[currentIndex]);
            newEnemy.transform.SetParent(null);

            spawns--;

            if (clockwise)
            {
                currentIndex++;
                if (currentIndex >= spawnTransforms.Count) { currentIndex = 0; }
            }
            else
            {
                currentIndex--;
                if (currentIndex < 0) { currentIndex = spawnTransforms.Count-1; }
            }

            yield return new WaitForSeconds(interval);
        }
    }
}
