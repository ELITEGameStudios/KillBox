using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerLock : MonoBehaviour
{
    public Transform CamTf, PlayerTf;
    public float CamZ;
    Vector3 vec;

    // Update is called once per frame
    void Update()
    {
        vec = CamTf.position;
        CamTf.position = new Vector3(vec.x, vec.y, CamZ);
    }
}
