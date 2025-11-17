using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cymkBulletRandomizer : MonoBehaviour
{
    [SerializeField]
    private Color color;
    private int integer;
    [SerializeField] private SpriteRenderer sprite;

    void OnEnable(){
        integer = Random.Range(1, 5);
        switch (integer){
            case 1:
            color = Color.cyan;
            break;
            case 2:
            color = Color.yellow;
            break;
            case 3:
            color = Color.magenta;
            break;
            case 4:
            color = Color.black;
            break;
        }
        sprite.color = color;
    }
}