using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugAntiRotator : MonoBehaviour
{
    [SerializeField]
    private Transform mainBody;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    this.transform.localEulerAngles = new Vector3(0, 0, mainBody.localEulerAngles.z * -1);
    }
}
