using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveTimer : MonoBehaviour
{
    // [SerializeField] private List<TimeData> values;
    public static ActiveTimer instance { get; private set; }

    void Awake()
    {
        if(instance == null){ instance = this; }
        if(instance != this){ Destroy(this); }
    }

    struct TimeData
    {
        public float value;
        public float coefficient;
        public float endState;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void Slide(ref float value, float slideTo)
    {
        
    }

    public static void AddTimer(ref float value, float coefficient)
    {
        TimeData data;
        data.value = value;
        data.coefficient = coefficient;
        data.endState = data.coefficient >= 0 ? Mathf.Infinity : -Mathf.Infinity; 
    }
}