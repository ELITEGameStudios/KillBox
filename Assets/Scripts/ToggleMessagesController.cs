using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMessagesController : MonoBehaviour
{
    private bool active = true;
    [SerializeField]
    private GameObject[] objects; 

    public void Switch()
    {
        if (active)
        {
            active = false;
        }
        else
        {
            active = true;
        }

        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(active);
        }
    }
}
