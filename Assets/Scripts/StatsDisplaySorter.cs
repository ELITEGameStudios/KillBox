using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatsDisplaySorter : MonoBehaviour
{
    private Slider[] child_sliders, sort_helper;
    private float[] normalized_values;
    private ShopScript shop;

    void Awake()
    {
        child_sliders = new Slider[transform.childCount];
        normalized_values = new float[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            child_sliders[i] = transform.GetChild(i).gameObject.GetComponent<Slider>();
        }
        shop = GameObject.Find("Manager").GetComponent<ShopScript>();
        //Sort();

    }

    // Update is called once per frame
    public void Update()
    {
        //if (shop.weapon_class != self_class)
        //{
        for (int i = 0; i < child_sliders.Length; i++)
        {
            normalized_values[i] = (child_sliders[i].value - child_sliders[i].minValue) / (child_sliders[i].maxValue - child_sliders[i].minValue);
        }

        Array.Sort(normalized_values);

        sort_helper = new Slider[child_sliders.Length];

        for (int i = 0; i < child_sliders.Length; i++)
        {
            for (int n = 0; n < normalized_values.Length; n++)
            {
                if (NormalizeFloat(child_sliders[i].value, child_sliders[i].maxValue, child_sliders[i].minValue) == normalized_values[n])
                {
                    sort_helper[n] = child_sliders[i];
                    break;
                }
            }
        }

        for (int i = 0; i < sort_helper.Length; i++)
        {
            if(sort_helper[i] != null)
                sort_helper[i].transform.SetSiblingIndex(transform.childCount - 1 - i);
        }
        //}

        //else{
        //    for (int i = 0; i < transform.childCount; i++)
        //    {
        //        child_sliders[i].transform.SetSiblingIndex(transform.childCount - 1 - i);
        //    }
        //}
    }

    float NormalizeFloat(float value, float high_value, float low_value)
    {
        float return_value = (value - low_value) / (high_value - low_value);

        return return_value;
    }
}
