using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;

public abstract class EquipmentBase : MonoBehaviour
{
    public float time;
    [SerializeField] private Color color;
    public abstract void GamemodeStart();
    public virtual void GamemodeEnd(){}
}
