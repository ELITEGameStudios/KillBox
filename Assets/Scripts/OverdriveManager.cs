using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class OverdriveManager : EquipmentBase
{
    public GameObject CamEffectObj;

    void Start()
    {
        CamEffectObj.SetActive(false);
    }

    void Update(){ GunHandler.Instance.cooldown.ResetCooldown(); }

    public override void GamemodeStart() {}
}
