using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosExplosion : MonoBehaviour
{
    // [SerializeField] private float radius;
    [SerializeField] private List<ChaosTargetEntry> targets;

    void Awake(){
        targets = new List<ChaosTargetEntry>();
    }

    void Update(){
        for(int i = 0; i < targets.Count; i++){
            if(targets[i].obj == null) {continue;}
            targets[i].Feed(Time.deltaTime);
        }
    }


    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.GetComponent<EnemyHealth>() != null){
            targets.Add(new ChaosTargetEntry(col.gameObject, transform));
        }
        return;
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.GetComponent<EnemyHealth>() != null){
            targets.Add(new ChaosTargetEntry(col.gameObject, transform));
        }
        return;
    }
}
