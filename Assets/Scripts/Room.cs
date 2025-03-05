using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private bool is_open, soft_lock_threat, chained;
    [SerializeField] private Collider2D room_collider;
    [SerializeField] private Transform soft_lock_guard_transform;
    [SerializeField] private List<Door> connected_doors;
    [SerializeField] private List<Room> connected_rooms;


    public bool Is_open { get => is_open; set{}}
    public bool Soft_lock_threat { get => soft_lock_threat; set{}}
    public bool Chained { get => chained; set{}}
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject == Player.main.obj){
            if(!is_open && soft_lock_threat){
                Player.main.tf.position = soft_lock_guard_transform.position;
            }
        }
    }
}
