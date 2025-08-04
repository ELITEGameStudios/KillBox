using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyColorFromImage : MonoBehaviour
{
    [SerializeField]
    private Image target_image, copyied_image;

    // Update is called once per frame
    void Update()
    {
        if(copyied_image != null && target_image != null)
        copyied_image.color = target_image.color;
    }
}
