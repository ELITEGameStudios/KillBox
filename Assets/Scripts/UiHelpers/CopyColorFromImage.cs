using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyColorFromImage : MonoBehaviour
{
    [SerializeField]
    private Image target_image, copyied_image;
    [SerializeField] bool withAlpha = false;
    [SerializeField] private float savedAlpha;

    void Awake() {
        savedAlpha = copyied_image.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (copyied_image != null && target_image != null)
        {
            if (!withAlpha)
            {
                copyied_image.color = target_image.color;
            }
            else{
                copyied_image.color = Color.Lerp(Color.clear, target_image.color, savedAlpha);
            }
        }
    }
}
