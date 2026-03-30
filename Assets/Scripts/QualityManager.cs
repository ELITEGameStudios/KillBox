using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityManager : MonoBehaviour
{
    public GameObject[] HQGameObjects;
    public GameObject gridEffectObject;
    // Update is called once per frame
    void Update()
    {
        QualityControl qualityControl = QualityControl.main;
        if(qualityControl.MainPostProcessing)
        {
            for (int i = 0; i < HQGameObjects.Length; i++)
                HQGameObjects[i].SetActive(true);
        }
        else
        {
            for (int i = 0; i < HQGameObjects.Length; i++)
                HQGameObjects[i].SetActive(false);
        }

        if(qualityControl.GridEffectShader != gridEffectObject.activeInHierarchy)
        {
            gridEffectObject.SetActive(qualityControl.GridEffectShader);
        }

    }
}
