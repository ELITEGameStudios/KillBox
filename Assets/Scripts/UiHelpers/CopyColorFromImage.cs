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
        copyied_image.color = target_image.color;
    }
}
