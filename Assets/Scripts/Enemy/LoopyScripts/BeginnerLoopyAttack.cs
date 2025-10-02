

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BeginnerLoopyAttack : BossStateData
{
    public LoopyBoss loopyBoss;
    public GameObject[] prefabs;
    public List<GameObject> spawnedEnemies;

    public int iterations;
    public int quantity;
    public float offset; // in 0-1
    public float spawningInterval; 
    public float iterationInterval; 
    public float startDistance; 
    public float duration, timer; 
    public bool endsByDuration {get { return duration > 0; }}
    public bool destroySpawnedEnemies; 

    public BeginnerLoopyAttack(LoopyBoss bossBase, GameObject[] prefabs, int quantity = 12, int iterations = 8, float startDistance = 0, float spawningInterval = 0, float iterationInterval = 1, float offset = 0.5f, float maxDuration = -1, bool destroySpawnedEnemies = false) : base(bossBase) // Always include super(bossBase) in any child class constructors
    {
        loopyBoss = bossBase;

        this.prefabs = prefabs;
        this.quantity = quantity;
        this.iterations = iterations;
        this.offset = offset;
        this.startDistance = startDistance;
        this.spawningInterval = spawningInterval;
        this.iterationInterval = iterationInterval;
        this.destroySpawnedEnemies = destroySpawnedEnemies;

        duration = maxDuration;
        timer = 0;

        OnReset();
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
        timer = 0;
        spawnedEnemies = new List<GameObject>();
    }

    // Called When the state object becomes active
    public override void Start()
    {
        loopyBoss.StartCoroutine(nameof(SpawnEnemies));
    }

    public IEnumerator SpawnEnemies()
    {
        for (int performedIterations = 0; performedIterations < iterations; performedIterations++)
        {
            for (int i = 0; i < quantity; i++)
            {
                float angle = (360f / quantity * i) + (360f / quantity * offset * performedIterations); //offset);
                Vector2 orientation = new Vector2(
                    Mathf.Cos( (angle+90) * Mathf.Deg2Rad),
                    Mathf.Sin( (angle+90) * Mathf.Deg2Rad)
                );
                Vector2 startPos = orientation * startDistance;

                GameObject prefab = performedIterations < prefabs.Length ? prefabs[performedIterations] : prefabs[^1];
                GameObject newObject = loopyBoss.SpawnObject(prefab, (Vector2)transform.position + startPos, Quaternion.Euler(0, 0, angle));
                newObject.transform.SetParent(null);
                spawnedEnemies.Add(newObject);

                if (spawningInterval > 0)
                {
                    yield return new WaitForSeconds(spawningInterval);
                }
            }

            yield return new WaitForSeconds(iterationInterval);
        }

        if (!endsByDuration){
            End();
        }
    }


    // Called every frame while the object is active
    public override void Update()
    {
        if (endsByDuration) {
            if(timer >= duration){ End(true); }
        }
        timer += Time.deltaTime;
    }    
    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        if(interrupted){ loopyBoss.StopSpawnEnemiesCoroutine(); }

        if (destroySpawnedEnemies)
        {
            for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
            {
                try
                {
                    if (spawnedEnemies[i].GetComponent<EnemyHealth>() != null)
                    {
                        spawnedEnemies[i].GetComponent<EnemyHealth>().Die(false);
                    }

                    else if (spawnedEnemies[i].GetComponent<EnemyProfile>() != null)
                    {
                        spawnedEnemies[i].GetComponent<EnemyProfile>().Retire();
                    }

                    Object.Destroy(spawnedEnemies[i]);
                }
                catch (MissingReferenceException) { spawnedEnemies.RemoveAt(i);  continue; }
            }
        }

        finished = true;
    }
}