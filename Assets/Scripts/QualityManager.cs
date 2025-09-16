using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public GameObject[] HQGameObjects;
    // Update is called once per frame
    void Update()
    {
        QualityControl qualityControl = QualityControl.main;
        if(qualityControl.hqVolumeIndex == 1)
        {
            for (int i = 0; i < HQGameObjects.Length; i++)
                HQGameObjects[i].SetActive(true);
        }
        else
        {
            for (int i = 0; i < HQGameObjects.Length; i++)
                HQGameObjects[i].SetActive(false);
        }

    }
}
