using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BossRoundManager;

public class BossDoor : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] BossType targetBoss;
    
    [SerializeField] GameObject colliderObject;
    [SerializeField] GameObject safeObject;
    [SerializeField] ParticleSystem particleSystem;

    public float nearbyDistance = 5;

    public enum DoorState
    {
        IDLE,
        NEARBY,
        OPEN
    }
    public bool hasKey => GameManager.main.hasAcquiredKey[(int)targetBoss-2];
    bool nearby => (Player.main.tf.position - transform.position).magnitude <= 5;
    public DoorState state;


    void Update()
    {
        if (state != DoorState.OPEN)
        {
            if(nearby)
            {
                if(state != DoorState.NEARBY)
                {
                    OnNearby();
                }
            }
            else
            {
                if(state != DoorState.IDLE)
                {
                    OnLeave();
                }
            }
        }
    }

    public void Open()
    {
        if(animator != null) animator.SetTrigger("Open"); 

        colliderObject.SetActive(false);
        safeObject.SetActive(false);
        particleSystem.Play();
        
        state = DoorState.OPEN;   
    }

    public void OnNearby()
    {
        if (state != DoorState.OPEN)
        // {
        //     Open();
        // }
        // else 
        {
            OnDeny();    
        }
    }

    public void OnDeny()
    {
        if(animator != null) animator.SetTrigger("Near");   
        state = DoorState.NEARBY;
    }

    public void OnLeave()
    {
        if(animator != null) animator.SetTrigger("Leave");   
        state = DoorState.IDLE;
    }
}
