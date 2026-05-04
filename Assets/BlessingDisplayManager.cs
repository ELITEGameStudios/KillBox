using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlessingDisplayManager : MonoBehaviour
{
    // public FadeManager shardGraphics, cutterGraphics, aetherGraphics, guardianGraphics;
    public Animator animator => animatorList[(int)currentType];
    public Animator[] animatorList;
    public BlessingType currentType;
    public GameObject bgObject;
    public FlipbookUISystem[] flipbooks;
    public string introAnimTrigger, outroAnimTrigger;
    public float introTime = 1, outroTime = 1;
    // public delegate void OnFinished();
    public Action Finished;
    
    public static BlessingDisplayManager instance {get; private set;}

    public enum BlessingType
    {
        SHARD,
        CUTTER,
        SPECIAL,
        GUARDIANS,
        FINAL,
        NONE,
    }

    void Awake()
    {
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
    }

    public void BeginBlessingSequence(BlessingType blessingType)
    {
        bgObject.SetActive(true);
        currentType = blessingType;
        
        if(animator != null)
        {
            animator.SetTrigger(introAnimTrigger);
        }

        if(currentType != BlessingType.NONE)
        {
            flipbooks[(int)blessingType].gameObject.SetActive(true);
        }

        Invoke(nameof(OnIntroTime), introTime);
    }

    public void OnIntroTime()
    {
        flipbooks[(int)currentType].Begin();
    }
    public void OnOutroTime()
    {
        flipbooks[(int)currentType].gameObject.SetActive(false);
        currentType = BlessingType.NONE;

        Finished.Invoke();
        bgObject.SetActive(false);
    }

    public void EndBlessingSequence()
    {
        if(animator != null)
        {
            animator.SetTrigger(outroAnimTrigger);
        }

        Invoke(nameof(OnOutroTime), outroTime);
    }

}
