using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public GameManager manager;
    public Image image;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (manager.HasKey)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.black;
        }
    }
}
