using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sineWave : MonoBehaviour
{
    [SerializeField]
    private float timeElasped, period, amplitude, initPos, velocity;
    [SerializeField]
    private Transform mainTf;
    // Start is called before the first frame update
    void Awake()
    {
        timeElasped = 0f;
        initPos = mainTf.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        timeElasped += Time.deltaTime;
        transform.position = new Vector2(initPos + amplitude * Mathf.Sin(timeElasped * period), mainTf.position.y + velocity*Time.deltaTime);
    }
}
