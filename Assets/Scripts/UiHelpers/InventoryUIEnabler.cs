using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIEnabler : MonoBehaviour
{
    void OnEnable()
    {
         StartCoroutine(LateEnable());
    }
 
    IEnumerator LateEnable()
    {
        yield return null;

        if(GameManager.main.EscapeRoom()){
            InventoryUIManager.Instance.OnSetTargetKey("Revolver"); 
            InventoryUIManager.Instance.PurchaseCall(); 
            Debug.Log("this should be purchased" + InventoryUIManager.Instance.target_key);
            InventoryUIManager.Instance.Backup();
        } 
        else{
            InventoryUIManager.Instance.OnSetTargetKey("Pistol");
        }

    }
}
