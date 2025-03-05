using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;


public class WeaponItem
{
    public readonly string name;
    public readonly Weapon weapon;
    public readonly int price, tier;

    public readonly string non_purchase_desc, special_key;
    public Sprite graphic { get; private set; }
    // public bool owned { get; private set; }
    public bool owned;

    public WeaponItem(string name_input, Weapon weapon_input, Sprite texture = null, int price_input = -1, bool owned_input = false, int tier_input = 0, string attain_desc = "Unpurchasable", string _special_key = "")
    {
        name = name_input;
        weapon = weapon_input;
        price = price_input;
        owned = owned_input;
        tier = tier_input;
        non_purchase_desc = attain_desc;

        if (texture != null)
        {
            graphic = texture;
        }
        else
        {
            graphic = null;
        }

        special_key = _special_key;
    }

    public void SetGraphic(Sprite texture)
    {
        graphic = texture;
    }

    public int Transaction(int request)
    {
        if (request >= price)
        {
            request -= price;
            owned = true;

            owned = true;

            GunHandler.Instance.NewItem(this);

            return request;
        }
        else
        {
            return -1;
        }
    }

    public void Reset(){
        owned = false;
    }

    public void FreeTransaction(Lvl4PickupScript caller)
    {

        owned = true;
        GunHandler.Instance.NewItem(this);

    }

    public bool Compare(int request)
    {
        if (request >= price && price != -1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CostDifference(int request)
    {
        return price - request;
    }
}