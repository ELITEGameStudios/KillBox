using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerColliderTracker : MonoBehaviour
{
    [SerializeField] private List<Collider2D> colliders = new List<Collider2D>();
    public List<Collider2D> GetColliders() { return colliders; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!colliders.Contains(other)) { colliders.Add(other); }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        colliders.Remove(other);
    }

    void Update(){
        for(int i = 0; i < colliders.Count; i++){
            if(colliders[i] == null){
                colliders.RemoveAt(i);
            }
        }
    }
}
