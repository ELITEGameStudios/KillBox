using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineWave : MonoBehaviour
{
    [SerializeField]
    private float timeElasped, period;
    [SerializeField]
    private Transform mainTf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeElasped += Time.deltaTime;
        transform.position = new Vector2(mainTf.position.x, mainTf.position.y + Mathf.Sin(timeElasped * period));
    }
}
