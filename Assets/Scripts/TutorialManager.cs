using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager tutorialManager;

    public GameObject[] prompts;
    public UnityEvent[] events;
    public int currentSequence, sequences, tokenProgress;
    private bool tokenEvent = true, shopEvent = true, weaponEvent = true, weaponBuyEvent = true;

    // Start is called before the first frame update
    void Awake()
    {
        tutorialManager = this;
        sequences = events.Length;
    }

    void Update(){
        if (tokenEvent && tokenProgress == 4)
        {
            AdvanceSequence();
            tokenEvent = false;
        }
    }

    public void OnShopOpen(){
        if(shopEvent){
            AdvanceSequence();
            shopEvent = false;
        }
    }

    public void OnWeaponOpen(){
        if(weaponEvent){
            AdvanceSequence();
            weaponEvent = false;
        }
    }

    public void OnWeaponBuy(){
        if(weaponBuyEvent){
            AdvanceSequence();
            weaponBuyEvent = false;
        }
    }

    public void AdvanceSequence(){
        currentSequence++;
        events[currentSequence].Invoke();
        for (int i = 0; i < prompts.Length; i++)
        {
            prompts[i].SetActive(false);
        }
        prompts[currentSequence].SetActive(true);
    }
}
