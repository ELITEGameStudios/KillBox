using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UprightRotation : MonoBehaviour
{
    [SerializeField] private float customAngle;

    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, customAngle);
    }
}
