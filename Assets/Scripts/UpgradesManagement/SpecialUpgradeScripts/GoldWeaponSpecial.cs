using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldWeaponSpecial : MonoBehaviour, SpecialUpgrade
{

    public void OnInit()
    {
        // GunHandler.Instance.SetDual(true);
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.GOLDEN;
    }

    public void OnDeactivate()
    {
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.NONE;
        // GunHandler.Instance.SetDual(false);
        
    }

}
