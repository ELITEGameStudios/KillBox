using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecroSpecial : MonoBehaviour, SpecialUpgrade
{

    public void OnInit()
    {
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.NECRO;
    }

    public void OnDeactivate()
    {
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.NONE;
    }

}
