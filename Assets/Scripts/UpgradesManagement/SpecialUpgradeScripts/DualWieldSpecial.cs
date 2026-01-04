using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DualWieldSpecial : MonoBehaviour, SpecialUpgrade
{

    public void OnInit()
    {
        GunHandler.Instance.SetDual(true);
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.DUAL_WIELD;
        InventoryUIManager.Instance.dual_element.gameObject.SetActive(true);

        GameplayUI.instance.GetDualAnimator().gameObject.SetActive(true);
        GameplayUI.instance.GetDualAnimator().SetBool("Active", true);

        GunHandler.Instance.UIRefresh();
    }

    public void OnDeactivate()
    {
        GunHandler.Instance.SetDual(false);
        Player.main.specialUpgradeEnum = UpgradesList.SpecialUpgradeEnum.NONE;
        InventoryUIManager.Instance.dual_element.gameObject.SetActive(false);

        GameplayUI.instance.GetDualAnimator().gameObject.SetActive(false);
        GameplayUI.instance.GetDualAnimator().SetBool("Active", false);
        
        GunHandler.Instance.UIRefresh();
    }

}
