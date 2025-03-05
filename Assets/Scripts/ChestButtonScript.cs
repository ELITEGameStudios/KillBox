using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestButtonScript : MonoBehaviour
{
    public ChestScript chest;
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
        if(manager.ScoreCount >= chest.cost)
        {
            manager.ScoreCount -= chest.cost;
            chest.GrantChest();
        }
        // else
        // {
        //     Debug.Log("no");
        // }
    }
}
