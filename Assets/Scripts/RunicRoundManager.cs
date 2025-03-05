using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunicRoundManager : MonoBehaviour
{
    public static RunicRoundManager main {get; private set;}

    [SerializeField]
    private GameObject wind_object;

    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BlowWind(bool blow){
        wind_object.SetActive(blow);
    }


}
