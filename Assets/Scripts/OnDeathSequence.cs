using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathSequence : MonoBehaviour
{
    public GameObject player, DeathMenu;
    public bool HasDied;

    // Start is called before the first frame update
    void Start()
    {
        HasDied = false;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");

        if(player == null)
        {
            //DeathMenu.SetActive(true);
            HasDied = true;
        }

    }
}
