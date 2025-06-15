using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public abstract class EquipmentBase : MonoBehaviour
{
    public bool startEffect, endEffect;
    public float time;
    public EquipmentManager.EquipmentType equipmentType;
    // [SerializeField] private float time, start_effect_clock, end_effect_clock;
    // [SerializeField] private string buttonText;
    [SerializeField] private Color color;
    public Volume volume;

    public abstract void GamemodeStart();
    public virtual void GamemodeEnd(){}
}
