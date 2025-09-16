using UnityEngine;

public class OverdriveManager : EquipmentBase
{
    public GameObject CamEffectObj;

    void Start()
    {
        CamEffectObj.SetActive(false);
    }

    public override void ActiveUpdate(){ GunHandler.Instance.cooldown.ResetCooldown(); }

    public override void GamemodeStart() {}
}
