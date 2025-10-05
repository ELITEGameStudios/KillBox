using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCamera : MonoBehaviour
{
    [SerializeField] Camera referenceCamera, thisCamera;

    // Update is called once per frame
    void Update()
    {
        thisCamera.orthographicSize = referenceCamera.orthographicSize;
        
    }
}
