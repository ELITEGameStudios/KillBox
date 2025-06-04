using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradesButtonAssigner : MonoBehaviour
{
    [SerializeField] private Text[] lvlDisplays, costDisplays;
    
    // Start is called before the first frame update
    void Awake()
    {
        UpgradesManager.Instance.SetSecondaryDisplays(lvlDisplays, costDisplays);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
