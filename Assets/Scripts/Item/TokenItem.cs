using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenItem : MonoBehaviour, ItemPickup
{
    public void OnPickup()
    {
        GameManager.main.OnPickupToken(1);
        gameObject.SetActive(false);
    }
}
