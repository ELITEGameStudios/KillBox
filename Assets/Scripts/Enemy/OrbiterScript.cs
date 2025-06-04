using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbiterScript : MonoBehaviour
{

    [SerializeField]
    private int orbiters;

    [SerializeField]
    private float distance, spin_rate;

    [SerializeField]
    private GameObject[] prefabs, objects;

    private float gap, main_angle;

    // Start is called before the first frame update
    void Awake(){}

    void Start(){
        gap = 360 / orbiters;

        objects = new GameObject[orbiters];
        main_angle = 0;
        float angle = 0;

        for (int i = 0; i < orbiters; i++){
            Debug.Log(i + "AAAAAAAAA" + angle);
            objects[i] = Instantiate(prefabs[i], new Vector2(Mathf.Cos( Mathf.Deg2Rad * angle ), Mathf.Sin( Mathf.Deg2Rad * angle )) * distance, transform.rotation);
            objects[i].transform.SetParent(null);
            objects[i].transform.localScale = new Vector3(1, 1, 1);
            angle += gap;
        }
    }

    // Update is called once per frame
    void Update()
    {
        main_angle += spin_rate * Time.deltaTime;
        float angle = main_angle;
        
        for (int i = 0; i < orbiters; i++){
            angle += gap;
            //angle -= 45;
            if(objects[i] == null){ continue; }
            objects[i].transform.position = transform.position + new Vector3(Mathf.Cos( Mathf.Deg2Rad * angle ), Mathf.Sin( Mathf.Deg2Rad * angle ), 1) * distance;
        }
    }
}
