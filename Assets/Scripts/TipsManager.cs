using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipsManager : MonoBehaviour, IWeaponEventListenter, IEquipmentEventListener, IMainRoundEventListener, IPurchaseEventListener, IShopUIEventListener, IPlayerEventListener
{
    // public bool tooltipsEnabled {get; private set;} 
    [SerializeField] private bool tooltipsEnabled;
    [SerializeField] private GameObject[] tip; 
    [SerializeField] private bool[] tipInitiated;


    public void OnCooldownBegin(WeaponEventData data)
    {
        if(GunHandler.Instance.owned_weapons.Count > 1){
            ToggleTip(0, true);
        }
        ToggleTip(3, true);
    }

    public void OnCooldownEnd(WeaponEventData data)
    {}

    public void OnEquipmentCharged()
    {
        
        ToggleTip(4, true);
    }

    public void OnEquipWeapon(WeaponEventData data)
    {}

    public void OnFireWeapon(WeaponEventData data)
    {}


    public void OnPortalInteract()
    {
    }

    public void OnPurchaseUpgrade(Upgrade upgrade, int cost, int level)
    {}

    public void OnPurchaseWeapon(WeaponItem weapon, int cost)
    {
        ToggleTip(5, true);
    }

    public void OnRoundChange()
    {
        if(GameManager.main.LvlCount == 2){
            ToggleTip(1, true);
        }
        else if(GameManager.main.LvlCount == 3){
            ToggleTip(2, true);
        }
        else if(GameManager.main.LvlCount == 4){
            ToggleTip(6, true);
        }
        ToggleTip(4, false);
        ToggleTip(3, false);
    }

    public void OnRoundEnd()
    {
        
    }

    public void OnRoundStart()
    {
        ToggleTip(1, false);
        ToggleTip(2, false);
        ToggleTip(5, false);
    }


    public void OnSwitchWeapon(WeaponEventData data)
    {
        ToggleTip(0, false);
    }

    public void OnTriggerEquipment()
    {

        ToggleTip(4, false);
    }

    public void OnCloseShop()
    {
        
    }
    public void OnOpenShop(int shopId)
    {
        ToggleTip(1, false);
    }

    public void OnSetNewWeapon(WeaponItem weaponItem, int slot)
    {
        ToggleTip(5, false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void ToggleTip(int index, bool state){
        if(state){
            if(!tip[index].activeInHierarchy && !tipInitiated[index] && tooltipsEnabled){
                tip[index].SetActive(true);
                tipInitiated[index] = true;
            }
        }
        else{
            tip[index].SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTakeDamage(DamageEventData data)
    {}

    public void OnDie()
    {
        ToggleTip(0, false);
        ToggleTip(1, false);
        ToggleTip(2, false);
        ToggleTip(3, false);
        ToggleTip(4, false);
        ToggleTip(5, false);
        ToggleTip(6, false);
    }

    public void OnDash()
    {
        ToggleTip(6, false);
    }

    public void OnDashRecharged()
    {}

    public void OnInteract()
    {}
}
