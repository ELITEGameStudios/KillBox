using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSystemManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] level_parent_transforms;

    
    public GameObject[] chests;

    void Awake()
    {
        for(int i = 0; i < level_parent_transforms.Length; i++)
        {
            level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().active = new bool[level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().points.Length];
            level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().active_2= new bool[level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().lv2_points.Length];
            level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().active_3 = new bool[level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().lv3_points.Length];
            level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().active_4 = new bool[level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>().lv4_points.Length];
        }
    }

    void Update()
    {

    }

    public void RefreshChests()
    {
        for(int i = 0; i < level_parent_transforms.Length; i++)
        {
            int iterations = Random.Range(3, 8);
            ChestSystemSubmanager submanager = level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>();
            for (int ii = 0; ii < iterations; ii++)
            {
                submanager.PlaceChest(Random.Range(0, submanager.points.Length));
            }

            iterations = Random.Range(1, 4);
            for (int ii = 0; ii < iterations; ii++)
            {
                submanager.PlaceChest2(Random.Range(0, submanager.lv2_points.Length));
            }

            iterations = Random.Range(1, 3);
            for (int ii = 0; ii < iterations; ii++)
            {
                submanager.PlaceChest3(Random.Range(0, submanager.lv3_points.Length));
            }
        }
    }

    public void RefreshChestsSingle(ChestSystemSubmanager submanager)
    {
        int iterations = Random.Range(3, 8);
        for (int ii = 0; ii < iterations; ii++)
        {
            submanager.PlaceChest(Random.Range(0, submanager.points.Length));
        }

        iterations = Random.Range(1, 4);
        for (int ii = 0; ii < iterations; ii++)
        {
            submanager.PlaceChest2(Random.Range(0, submanager.lv2_points.Length));
        }

        iterations = Random.Range(1, 3);
        for (int ii = 0; ii < iterations; ii++)
        {
            submanager.PlaceChest3(Random.Range(0, submanager.lv3_points.Length));
        }
    }

    public void RefreshCheck()
    {
        //for (int i = 0; i < level_parent_transforms.Length; i++)
        //{
        //    bool valid = true;
        //    ChestSystemSubmanager submanager = level_parent_transforms[i].gameObject.GetComponent<ChestSystemSubmanager>();
        //
        //    for (int ii = 0; ii < submanager.active.Length; ii++)
        //    {
        //        if (!submanager.active[ii])
        //        {
        //            continue;
        //        }
        //        else
        //        {
        //            valid = false;
        //            break;
        //        }
        //    }
        //
        //    if (valid)
        //    {
        //        for (int ii = 0; ii < submanager.active_2.Length; ii++)
        //        {
        //            if (!submanager.active_2[ii])
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                valid = false;
        //                break;
        //            }
        //        }
        //
        //        if (valid)
        //        {
        //            for (int ii = 0; ii < submanager.active_3.Length; ii++)
        //            {
        //                if (!submanager.active_3[ii])
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    valid = false;
        //                    break;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            continue;
        //        }
        //
        //        if (valid)
        //        {
        //            for (int ii = 0; ii < submanager.active_4.Length; ii++)
        //            {
        //                if (!submanager.active_4[ii])
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    valid = false;
        //                    break;
        //                }
        //            }
        //            if (valid)
        //            {
        //                RefreshChestsSingle(submanager);
        //            }
        //            else
        //            {
        //                continue;
        //            }
        //        }
        //    }
        //    else
        //    {
        //        continue;
        //    }
        //}
    }
}
