using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class TutorialPortal : MonoBehaviour
{
    public float distance;
    public GameObject Player, self;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindWithTag("Player");
        distance = Vector3.Distance(Player.transform.position, self.transform.position);
        if(distance < 0.5)
        {
            SceneManager.LoadScene("Menus");
        }
    }
}
