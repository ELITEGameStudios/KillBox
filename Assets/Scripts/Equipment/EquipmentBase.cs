using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public abstract class EquipmentBase : MonoBehaviour
{
    public bool startEffect, endEffect;
    [SerializeField] private float UltraTime, start_effect_clock, end_effect_clock;
    [SerializeField] private Text buttonText;
    public EquipmentManager.EquipmentType equipmentType;
    public Volume volume;

    public abstract void GamemodeStart();
    public virtual void GamemodeEnd(){}
}
