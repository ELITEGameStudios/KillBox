using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackRune : MonoBehaviour
{
    public bool ended;
    public int damage;
    public GameObject[] aetherEnemyPrefabs;

    void Awake()
    {
        
    }

    public void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 7.5f, LayerMask.GetMask("Player", "Enemy"));

        for (int i = colliders.Length - 1; i >= 0; i--)
        {
            Collider2D col = colliders[i];

            if (col.gameObject == Player.main.obj)
            {
                Player.main.health.TakeDmg(damage, 0.5f);
            }
            // else if (col.gameObject.GetComponent<EnemyProfile>() != null)
            // {
            //     GameObject newEnemy = Instantiate(aetherEnemyPrefabs[Random.Range(0, aetherEnemyPrefabs.Length)], col.transform);
            //     newEnemy.transform.SetParent(null);
            //     col.gameObject.GetComponent<EnemyProfile>().Retire();
            // }
        }
    }

    public void EndProcess()
    {
        Destroy(gameObject);
    }
}
