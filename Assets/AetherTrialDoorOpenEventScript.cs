using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class AetherTrialDoorOpenEventScript : MonoBehaviour
{
    [SerializeField] private BossDoor bossDoor;
    [SerializeField] private Transform unlockedRuneTf;
    [SerializeField] private SpriteRenderer lockedRuneImg;
    [SerializeField] private SpriteRenderer unlockedRuneImg;
    [SerializeField] private Animator doorOpenEventAnimator;
    [SerializeField] float lerpValue, preOpenAnimationTime = 5;


    public static List<AetherTrialDoorOpenEventScript> instances {get; private set;}

    
    void Awake()
    {
        if(instances == null){instances = new();}
        if(!instances.Contains(this)) {instances.Add(this);}
    }

    void Update(){
        unlockedRuneTf.position = Vector3.Lerp(transform.position, bossDoor.transform.position, lerpValue);
    }

    public void OpenDoorEvent()
    {
        lockedRuneImg.enabled = false;
        unlockedRuneImg.enabled = true;
        doorOpenEventAnimator.SetTrigger("BeginOpenDoor");
        Invoke(nameof(TrueOpenDoor), preOpenAnimationTime);
    }

    void TrueOpenDoor()
    {
        bossDoor.Open();
    }

    public void CheckEvent()
    {
        print("Does bro have the key? " + bossDoor.hasKey);
        if(bossDoor.hasKey){OpenDoorEvent();}
    }
}
