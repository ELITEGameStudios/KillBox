using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrialButtonScript : MonoBehaviour
{
    public TrialScript summoner;
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
        summoner.StartChallenge();
    }
}
