using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rgbBulletRandomizer : MonoBehaviour
{
    [SerializeField]
    private Color color;
    private int integer;
    [SerializeField] private SpriteRenderer sprite;

    void OnEnable(){
        integer = Random.Range(1, 4);
        switch (integer){
            case 1:
            color = Color.red;
            break;
            case 2:
            color = Color.green;
            break;
            case 3:
            color = Color.blue;
            break;
        }
        sprite.color = color;
    }
}