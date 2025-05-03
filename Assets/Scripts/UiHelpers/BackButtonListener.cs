using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BackButtonListener : MonoBehaviour, IBackButtonListener
{
    [SerializeField] private UnityEvent unityEvent;
    [SerializeField] private bool notPressedThisFrame;
    public void OnBackButton(bool pressedThisFrame)
    {
        if(pressedThisFrame != notPressedThisFrame && gameObject.activeInHierarchy){
            unityEvent.Invoke();
        }
    }

}
