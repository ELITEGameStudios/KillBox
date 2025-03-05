using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorEasterEggPortalPrefabClass : MonoBehaviour
{
    

    // Start is called before the first frame update
    void Start()
    {
        TpScript tp_script = gameObject.GetComponent<TpScript>();
        tp_script.Portals[0] = GameObject.Find("color_ee_tp_destination");
        tp_script.portalScript = GameObject.Find("Portal").GetComponent<PortalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
