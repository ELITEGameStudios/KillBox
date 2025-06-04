using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyUI : MonoBehaviour
{
    public Image image;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(GameManager.main == null) return;
        
        if (GameManager.main.HasKey)
        {
            image.color = Color.white;
        }
        else
        {
            image.color = Color.black;
        }
    }
}
