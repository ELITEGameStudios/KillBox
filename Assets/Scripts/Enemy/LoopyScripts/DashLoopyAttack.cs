

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DashLoopyAttack : BossStateData
{
    public LoopyBoss loopyBoss;
    public GameObject prefab;
    public List<GameObject> spawnedEnemies;

    public int iterations;
    public int quantity;
    public float distance;
    public float kP, stall;
    public Vector2[] targetPos;
    public AnimationCurve intervalOverTime; 

    public DashLoopyAttack(LoopyBoss bossBase, GameObject prefab, AnimationCurve intervalCurve, int quantity = 1, int iterations = 8, float distance = 7, float kP = 0.05f, float stall = 0.25f) : base(bossBase) 
    {
        loopyBoss = bossBase;

        this.prefab = prefab;
        this.quantity = quantity;
        this.iterations = iterations;
        this.distance = distance;
        this.kP = kP;
        this.stall = stall;
        intervalOverTime = intervalCurve;

        OnReset();
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
    }

    // Called When the state object becomes active
    public override void Start()
    {
        loopyBoss.Toggle(false);
        loopyBoss.StartCoroutine(MainCoroutine());
    }

    public IEnumerator MainCoroutine()
    {
        spawnedEnemies = new List<GameObject>();
        float angleOffset = 2 * Mathf.PI / quantity / 4;
        float angleStart = Random.Range(0, 2 * Mathf.PI);

        for(int i = 0; i < quantity; i++){
            
            Vector2 spawnPos = (Vector2)Player.main.tf.position +
                new Vector2(
                    Mathf.Cos(angleStart + angleOffset * i), 
                    Mathf.Sin(angleStart + angleOffset * i))
                    * distance;

            GameObject newObj = Object.Instantiate(prefab, (Vector3)spawnPos, transform.rotation);

            spawnedEnemies.Add( newObj );

            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(stall);
        yield return DashLoopCoroutine();
        
        End();        
    }

    public IEnumerator DashLoopCoroutine()
    {
        targetPos = new Vector2[spawnedEnemies.Count];
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 0; j < spawnedEnemies.Count; j++)
            {
                Vector2 currentPos = spawnedEnemies[j].transform.position;
                // Vector2 newPos = (Vector2)Player.main.tf.position;
                Vector2 newPos = (Vector2)Player.main.tf.position + (((Vector2)Player.main.tf.position - currentPos).normalized * (distance + Random.Range(-distance/5, distance/5)) );
                targetPos[j] = newPos;
            }

            yield return new WaitForSeconds(intervalOverTime.Evaluate(i/(float)iterations));
        }

    }

    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        loopyBoss.Toggle(true);
        targetPos = null;

        if(interrupted){loopyBoss.StopCoroutine(DashLoopCoroutine());}

        for (int i = spawnedEnemies.Count-1; i >= 0; i--)
        {
            Object.Destroy(spawnedEnemies[i]);
            spawnedEnemies.RemoveAt(i);
        }

        base.End(interrupted);
    }

    public override void Update()
    {
        if(targetPos == null) return;

        for (int i = 0; i < spawnedEnemies.Count; i++)
        {
            spawnedEnemies[i].transform.position = Vector2.Lerp(spawnedEnemies[i].transform.position, targetPos[i], kP);   
        }
    }
}