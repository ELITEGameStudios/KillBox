using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class EAI_equipment_manager : EquipmentBase
{
    public bool has_ally, startEffect, endEffect;

    [SerializeField]
    private float UltraTime, start_effect_clock, end_effect_clock;

    public GameObject Ally, activate_button; 
    public GameObject[] clone;
    
    // Start is called before the first frame update
    void Start()
    {
        has_ally = false;
    }

    public override void GamemodeStart()
    {
        clone = new GameObject[5];
        StartCoroutine(Lifespan());
    }

    IEnumerator Lifespan()
    {
        float interval = 0.5f;
        for (int i = 0; i < clone.Length; i++)
        {
            interval = 0.5f;
            while (interval > 0)
            {
                interval -= Time.deltaTime;
                yield return null;
            }

            clone[i] = Instantiate(Ally, transform);
            clone[i].transform.SetParent(null);
        }
    }

    public override void GamemodeEnd()
    {
        for (int i = 0; i < clone.Length; i++)
        {
            if(clone[i] != null)
            {
                Destroy(clone[i]);
            }
        }
    }
}
