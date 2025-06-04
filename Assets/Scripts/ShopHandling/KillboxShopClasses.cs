using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;

namespace KillboxShopClasses
{
    class Market
    {
        public readonly int health_cost = 2;
        public readonly int speed_cost = 3;
    }

    public class PurchaseRequest
    {
        public readonly string key;
        public readonly int money;
        public readonly bool purchased;

        PurchaseRequest(string key_input, int money_input, bool purchased_status = false)
        {
            key = key_input;
            money = money_input;
            purchased = purchased_status;
        }

        public static PurchaseRequest BuyWeapon(string key_input, int payment)
        {

            bool validation = false;

            WeaponItem item = WeaponItemList.Instance.GetItem(key_input);

            if(item == null)
            {
                return new PurchaseRequest(key_input, payment, validation);
            }
            
            validation = item.Compare(payment);

            if (validation)
            {
                payment = item.Transaction(payment);
            }

            PurchaseRequest result = new PurchaseRequest(key_input, payment, validation);

            

            return result;
        }
    }

    //class WeaponCheckout
    //{
    //    protected bool status;
    //    protected Weapon weapon;
    //
    //    public WeaponCheckout(bool status_input, Weapon weapon_input)
    //    {
    //        status = status_input;
    //        weapon = weapon_input;
    //    }
    //
    //}
}
