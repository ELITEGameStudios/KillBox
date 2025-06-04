using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrialSystemManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawn_transforms;
    public int[] boss_index;
    
    public GameObject trial_button;

    [SerializeField]
    private GameObject trial_obj;

    void Awake()
    {
        SpawnTrialToggle();

        //for(int i = 0; i < spawn_transforms.Length; i++)
        //{
        //    spawn_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().active = new bool[spawn_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().points.Length]; ;
        //}
    }

    public void SpawnTrialToggle()
    {
        int placement_index = Random.Range(0, spawn_transforms.Length);

        trial_obj = Instantiate(trial_button, spawn_transforms[placement_index]);
        trial_obj.GetComponent<BossTrialScript>().boss_index = boss_index[placement_index];

        //for (int i = 0; i < spawn_transforms.Length; i++)
        //{
        //    int iterations = Random.Range(3, 8);
        //    for (int ii = 0; ii < iterations; ii++)
        //    {
        //        submanager.PlaceChest(Random.Range(0, submanager.points.Length));
        //    }
        //}
    }
}
