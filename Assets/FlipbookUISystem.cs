using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FlipbookUISystem : MonoBehaviour
{
    public FlipbookData[] flipbookList;
    public Animator animator;
    public UnityEvent startEvent, finishEvent, resetEvent;
    public int currentIndex;
    public bool finishOnLast, finished;

    [System.Serializable]
    public struct FlipbookData
    {
        public GameObject[] gameObjects;
        public string animTrigger;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Begin()
    {
        Reset();
        
        startEvent.Invoke();
        foreach (GameObject obj in flipbookList[currentIndex].gameObjects){obj.SetActive(true);}
        if(animator != null){animator.SetTrigger(flipbookList[currentIndex].animTrigger);}
    }

    public void Flip()
    {
        if(finished){return;}
        foreach (GameObject obj in flipbookList[currentIndex].gameObjects){obj.SetActive(false);}
        currentIndex++;


        if(currentIndex == flipbookList.Length){
            OnFinish();
            return;
        }
        else if(currentIndex == flipbookList.Length - 1 && finishOnLast)
        {
            OnFinish();
        }
        
        if(animator != null){animator.SetTrigger(flipbookList[currentIndex].animTrigger);}
        foreach (GameObject obj in flipbookList[currentIndex].gameObjects){obj.SetActive(true);}

    }
    public void Reset()
    {
        try{
            foreach (GameObject obj in flipbookList[currentIndex].gameObjects){obj.SetActive(false);}
        }
        catch{}

        resetEvent.Invoke();
        currentIndex = 0;    
        finished = false;

    }

    public void OnFinish()
    {
        finishEvent.Invoke();
        finished = true;
        if(animator != null){animator.SetTrigger(flipbookList[currentIndex].animTrigger);}
    }

}
