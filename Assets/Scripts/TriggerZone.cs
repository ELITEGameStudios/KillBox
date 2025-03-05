using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerZone : MonoBehaviour
{
    public UnityEvent OnEnter, OnExit;
    public bool HasEntered;

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.name == "Player")
        {
            OnEnter.Invoke();
            HasEntered = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if(HasEntered)
        {
            OnExit.Invoke();
            HasEntered = false;
        }
    }
}
