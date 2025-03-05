using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DetectIfWeaponOwned : MonoBehaviour
{
    public ShopScript shop;
    public int GunIndex;

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < shop.OwnedGuns.Length; i++)
        {
            if (shop.OwnedGuns[i] == GunIndex)
            {
                gameObject.GetComponent<Button>().interactable = false;
                break;
            }
        }
    }
}
