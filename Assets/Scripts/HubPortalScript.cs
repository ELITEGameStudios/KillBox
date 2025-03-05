using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubPortalScript : MonoBehaviour
{
    [SerializeField]
    private float dist;

    [SerializeField]
    private GameManager manager;
    
    [SerializeField]
    private Transform Player;

    
    void Update()
    {
        dist = Vector3.Distance(Player.position, transform.position);
        if (dist < 0.75)
        {
            manager.InitHubMap();
        }
    }
}
