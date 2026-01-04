using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesSpecial : MonoBehaviour, SpecialUpgrade
{

    public void OnInit()
    {
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.MASTERY;
        Player.main.health.ResetHealth();
    }

    public void OnDeactivate()
    {
        GunHandler.Instance.SetDual(false);
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.NONE;
    }

}
