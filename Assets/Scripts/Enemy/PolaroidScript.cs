using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using System.IO;

public class PolaroidScript : MonoBehaviour
{
    [SerializeField]
    private int spin_range_x, spin_range_y;
    
    [SerializeField]
    private List<GameObject> beam_objects;
    
    [SerializeField]
    private GameObject beam_prefab;

    [SerializeField]
    private int beams_count;

    private ObjectPool pool;

    [SerializeField]
    private AIPath path;

    [SerializeField]
    private float start_spin_intensity, spin_time, beam_width, current_spin_time, current_spin_intensity, normalized_time, follow_time;




    void Start(){
        start_spin_intensity = Random.Range(spin_range_x, spin_range_y);
        current_spin_time = 0;
        current_spin_intensity = start_spin_intensity;
        normalized_time = 0;
        follow_time = Random.Range(25, 60);
        follow_time /= 100;

        transform.localEulerAngles = new Vector3(0, 0, 0);
        
        pool = GameObject.Find("polaroid_beam_pool").GetComponent<ObjectPool>();


        beam_objects = new List<GameObject>();

        //List<GameObject> objects_to_add = pool.GetPooledObjects(4);

        for (int i = 0; i < beams_count; i++)
        {
            beam_objects.Add(Instantiate(beam_prefab, transform));
        }

        foreach (GameObject beam in beam_objects)
        {
            beam.SetActive(true);
            beam.transform.SetParent(transform);
            beam.transform.localPosition = Vector3.zero;
            beam.GetComponent<PolaroidBeamObject>().BeginSequence(beam_width, spin_time);
        }

        float angle_dif = 360/beam_objects.Count;
        float current_angle = 0;

        foreach (GameObject item in beam_objects)
        {
            item.transform.localEulerAngles = new Vector3(0, 0, current_angle);
            current_angle += angle_dif;
        }
//
        //beam_objects[0].transform.localEulerAngles = new Vector3(0, 0, 0);
        //beam_objects[1].transform.localEulerAngles = new Vector3(0, 0, 90);
        //beam_objects[2].transform.localEulerAngles = new Vector3(0, 0, 180);
        //beam_objects[3].transform.localEulerAngles = new Vector3(0, 0, 270);

        
        path.enabled = true;
    }

    void Update(){

        current_spin_time += Time.deltaTime;
        
        normalized_time = current_spin_time / spin_time;
        current_spin_intensity = start_spin_intensity * (1 - normalized_time);
        
        if(normalized_time >= follow_time && path.enabled){
            path.enabled = false;
        }

        if(normalized_time >= 1){
            foreach (GameObject beam in beam_objects)
            {
                beam.transform.SetParent(null);
                beam.transform.localScale = new Vector3(1, 1, 1);
            }

            gameObject.GetComponent<EnemyHealth>().Die(to_player: false);
            gameObject.GetComponent<EnemyHealth>().TakeDmg(gameObject.GetComponent<EnemyHealth>().CurrentHealth);
        }

        transform.localEulerAngles += new Vector3(0, 0, current_spin_intensity * Time.deltaTime);
        
    }


}
