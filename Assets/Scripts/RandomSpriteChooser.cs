using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomSpriteChooser : MonoBehaviour
{
    [SerializeField]
    private bool instant, colorize, anchor_images, sprite_change;

    [SerializeField]
    private Sprite[] sprites;

    [SerializeField]
    private Color[] color;

    [SerializeField]
    private Vector2[] anchors;

    [SerializeField]
    private RectTransform tf;

    [SerializeField]
    private Image target_image;

    // Start is called before the first frame update
    void Awake()
    {
        if(instant){
            ChooseSprite();
        }
    }

    // Update is called once per frame
    void ChooseSprite()
    {
        int target = Random.Range(0, sprites.Length);

        if(sprite_change){
            target_image.sprite = sprites[target];
        }
        

        tf = target_image.gameObject.GetComponent<RectTransform>();

        if(colorize){
            if(color[target] != Color.clear){
                target_image.color = color[target];
            }
        }

        if(anchor_images){
            tf.pivot = anchors[target];
        }
    }
}
