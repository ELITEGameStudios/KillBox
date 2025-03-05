using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvolutionManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] lvl_1_Barriers;
    [SerializeField]
    private GameObject[] lvl_2_Barriers;
    [SerializeField]
    private int[] set_round, lvl_1_map, lvl_2_map;
    [SerializeField]
    private PortalScript portal;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnRoundChange(int round)
    {
        
    }
}
