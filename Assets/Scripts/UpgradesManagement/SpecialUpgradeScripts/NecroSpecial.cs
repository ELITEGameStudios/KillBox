using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroSpecial : MonoBehaviour, SpecialUpgrade
{

    public void OnInit()
    {
        GunHandler.Instance.SetDual(true);
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.DUAL_WIELD;
    }

    public void OnDeactivate()
    {
        GunHandler.Instance.SetDual(false);
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.NONE;
    }

}
