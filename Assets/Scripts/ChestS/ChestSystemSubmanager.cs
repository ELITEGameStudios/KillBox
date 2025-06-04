using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSystemSubmanager : MonoBehaviour
{
    public Transform[] points, lv2_points, lv3_points, lv4_points;
    public bool[] active, active_2, active_3, active_4;

    [SerializeField]
    private ChestSystemManager manager;
    [SerializeField]
    private GameManager gameManager;


    private Transform parent;

    // Start is called before the first frame update
    void Awake()
    {

        manager = GameObject.Find("Manager").GetComponent<ChestSystemManager>();
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();

        parent = transform.parent;
    }

    public void PlaceChest(int point) {
        if (point < active.Length)
        {
            if (!active[point])
            {

                GameObject new_chest = Instantiate(manager.chests[0], points[point].transform);
                //new_chest.transform.SetParent(parent);

                active[point] = true;
            }
        }
    }

    public void PlaceChest2(int point)
    {
        if (point < active_2.Length)
        {
            if (!active_2[point])
            {
                GameObject new_chest = Instantiate(manager.chests[1], lv2_points[point].transform);
                //new_chest.transform.SetParent(parent);

                active_2[point] = true;
            }
        }
    }

    public void PlaceChest3(int point)
    {
        if (point < active_3.Length)
        {
            if (!active_3[point])
            {
                GameObject new_chest = Instantiate(manager.chests[2], lv3_points[point].transform);
                //new_chest.transform.SetParent(parent);

                active_3[point] = true;
            }
        }
    }

    public void PlaceChest4(int point)
    {
        if (point < active_4.Length)
        {
            if (!active_4[point])
            {
                GameObject new_chest = Instantiate(manager.chests[3], lv4_points[point].transform);
                //new_chest.transform.SetParent(parent);

                active_4[point] = true;
            }
        }
    }
}
