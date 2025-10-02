

using System.Collections;
using UnityEngine;

[System.Serializable]
public class BeginnerLoopyAttack : BossStateData
{
    public LoopyBoss loopyBoss;
    public GameObject[] prefabs;

    public int iterations, performedIterations;
    public int quantity;
    public float offset; // in 0-1
    public float spawningInterval; 
    public float iterationInterval; 
    public float startDistance; 

    public BeginnerLoopyAttack(LoopyBoss bossBase, GameObject[] prefabs, int quantity = 12, int iterations = 8, float startDistance = 0, float spawningInterval = 0, float iterationInterval = 1, float offset = 0.5f) : base(bossBase) // Always include super(bossBase) in any child class constructors
    {
        loopyBoss = bossBase;

        this.prefabs = prefabs;
        this.quantity = quantity;
        this.iterations = iterations;
        this.offset = offset;
        this.startDistance = startDistance;
        this.spawningInterval = spawningInterval;
        this.iterationInterval = iterationInterval;

        OnReset();
    }


    // public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    // {
    //     base.OnReset();
    // }

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
                float angle = (360 / quantity * i) + (360 / quantity * offset);
                Vector2 orientation = new Vector2(
                    Mathf.Cos(angle) * Mathf.Rad2Deg,
                    Mathf.Sin(angle) * Mathf.Rad2Deg
                );
                Vector2 startPos = orientation * startDistance;

                GameObject prefab = i < prefabs.Length ? prefabs[i] : prefabs[^1];
                GameObject newObject = loopyBoss.SpawnObject(prefab, (Vector2)transform.position + startPos, Quaternion.Euler(0, 0, angle));
                newObject.transform.SetParent(null);

                yield return new WaitForSeconds(spawningInterval);
            }

            yield return new WaitForSeconds(iterationInterval);
        }

        End();
    }


    public override void Update(){} // Called every frame while the object is active
    public override void FixedUpdate() { } // Called every physics frame while the object is active

    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        // if(finished){ return; }
        finished = true;
    }
}