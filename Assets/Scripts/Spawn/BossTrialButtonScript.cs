using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTrialButtonScript : MonoBehaviour
{
    public BossTrialScript summoner;
    private GameManager manager;

    void Awake()
    {
        manager = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Update()
    {
        //if(manager.ScoreCount < chest.cost)
        //{
        //    gameObject.GetComponent<Button>().interactable = false;
        //}
    }

    public void ClickEvent()
    {
        summoner.SummonBoss();
    }
}
