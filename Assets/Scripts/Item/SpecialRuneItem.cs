using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UpgradesList;

public class SpecialRuneItem : MonoBehaviour, ItemPickup
{
    [SerializeField] Animator animator;
    [SerializeField] SpecialUpgradeEnum upgradeType;
    
    public void OnPickup()
    {
        Invoke(nameof(TriggerPickupRuneEvent), 1);
        Player.main.AddSpecialUpgrade(upgradeType);
        animator.SetTrigger("Pickup");
        
        // GameManager.main.OnPickupToken(1);
    }

    public void TriggerPickupRuneEvent()
    {
        // BlessingDisplayManager.instance.BeginBlessingSequence(BlessingDisplayManager.BlessingType.SPECIAL);
        
        // Temporary I guess?
        gameObject.SetActive(false);
    }
}
