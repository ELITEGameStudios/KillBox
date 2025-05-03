using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using KillboxWeaponClasses;
using UnityEngine;

public class KillboxEventSystem : MonoBehaviour
{

    // Weapon Event Triggers
    public static void TriggerSwitchWeaponEvent(WeaponEventData data){
        foreach (IWeaponEventListenter listener in FindObjectsOfType<MonoBehaviour>().OfType<IWeaponEventListenter>())
        { listener.OnSwitchWeapon(data); }
    } 
    
    public static void TriggerEquipWeaponEvent(WeaponEventData data){
        foreach (IWeaponEventListenter listener in FindObjectsOfType<MonoBehaviour>().OfType<IWeaponEventListenter>())
        { listener.OnEquipWeapon(data); }
    } 

    public static void TriggerFireWeaponEvent(WeaponEventData data){
        foreach (IWeaponEventListenter listener in FindObjectsOfType<MonoBehaviour>().OfType<IWeaponEventListenter>())
        { listener.OnFireWeapon(data); }
    } 

    public static void TriggerCooldownBegin(WeaponEventData data){
        foreach (ICooldownEventListenter listener in FindObjectsOfType<MonoBehaviour>().OfType<ICooldownEventListenter>())
        { listener.OnCooldownBegin(data); }
    } 

    public static void TriggerCooldownEnd(WeaponEventData data){
        foreach (ICooldownEventListenter listener in FindObjectsOfType<MonoBehaviour>().OfType<ICooldownEventListenter>())
        { listener.OnCooldownEnd(data); }
    } 

    
    // Round Event Triggers
    public static void TriggerRoundChangeEvent(){
        foreach (IMainRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IMainRoundEventListener>())
        { listener.OnRoundChange(); }
    } 
    public static void TriggerRoundStartEvent(){
        foreach (IMainRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IMainRoundEventListener>())
        { listener.OnRoundStart(); }
    }
    public static void TriggerRoundEndEvent(){
        foreach (IMainRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IMainRoundEventListener>())
        { listener.OnRoundEnd(); }
    }
    public static void TriggerBossRoundChangeEvent(){
        foreach (IBossRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IBossRoundEventListener>())
        { listener.OnBossRoundChange(); }
    }
    public static void TriggerBossRoundEndEvent(){
        foreach (IBossRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IBossRoundEventListener>())
        { listener.OnBossRoundEnd(); }
    }
    public static void TriggerBossRoundStartEvent(){
        foreach (IBossRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IBossRoundEventListener>())
        { listener.OnBossRoundStart(); }
    }
    public static void TriggerBossSpawnEvent(){
        foreach (IBossRoundEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IBossRoundEventListener>())
        { listener.OnBossSpawn(); }
    }

    // Shop Triggers
    public static void TriggerPurchaseWeaponEvent(WeaponItem purchasedWeapon, int weaponPrice){
        foreach (IPurchaseEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPurchaseEventListener>())
        { listener.OnPurchaseWeapon(purchasedWeapon, weaponPrice); }
    } 

    public static void TriggerPurchaseUpgradeEvent(Upgrade upgrade, int cost, int level){
        foreach (IPurchaseEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPurchaseEventListener>())
        { listener.OnPurchaseUpgrade(upgrade, cost, level); }
    } 
    public static void TriggerSetNewWeaponEvent(WeaponItem item, int id){
        foreach (IShopUIEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IShopUIEventListener>())
        { listener.OnSetNewWeapon(item, id); }
    } 
    public static void TriggerOpenShopEvent(int id){
        foreach (IShopUIEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IShopUIEventListener>())
        { listener.OnOpenShop(id); }
    }
    public static void TriggeCloseShopEvent(){
        foreach (IShopUIEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IShopUIEventListener>())
        { listener.OnCloseShop(); }
    }


    // ProgressionTriggers

    public static void TriggerLevelUpEvent(){
        foreach (IProgressionListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IProgressionListener>())
        { listener.OnLevelUp(); }
    } 
    public static void TriggerGainXpEvent(int xp){
        foreach (IProgressionListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IProgressionListener>())
        { listener.OnGainXp(xp); }
    } 

    // Self res listeners
    public static void TriggerSelfResPromptEvent(){
        foreach (ISelfResListener listener in FindObjectsOfType<MonoBehaviour>().OfType<ISelfResListener>())
        { listener.OnSelfResPrompt(); }
    } 
    public static void TriggerSelfResEvent(){
        foreach (ISelfResListener listener in FindObjectsOfType<MonoBehaviour>().OfType<ISelfResListener>())
        { listener.OnSelfRes(); }
    } 

    public static void TriggerDeathEvent(){
        foreach (IPlayerEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPlayerEventListener>())
        { listener.OnDie(); }
    } 

    // Back button listener
    public static void TriggerBackButtonEvent(bool pressedThisFrame){
        foreach (IBackButtonListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IBackButtonListener>())
        { listener.OnBackButton(pressedThisFrame); }
    } 

    public static void TriggerShopButtonAnimationEndEvent(){
        foreach (IShopButtonStateListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IShopButtonStateListener>())
        { listener.OnAnimationFinished(); }
    }


    public static void TriggerWeaponButtonSelectEvent(WeaponItem weapon){
        foreach (IUIButtonSelectListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IUIButtonSelectListener>())
        { listener.OnWeaponButtonSelect(weapon); }
    } 
    public static void TriggerUpgradeButtonSelectEvent(Upgrade upgrade){
        foreach (IUIButtonSelectListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IUIButtonSelectListener>())
        { listener.OnUpgradeButtonSelect(upgrade); }
    } 

    public static void TriggerGameRestartEvent(){
        foreach (IRestartListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IRestartListener>())
        { listener.OnRestartGame(); }
    } 

    public static void TriggerReturnToMenuEvent(){
        foreach (IRestartListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IRestartListener>())
        { listener.OnReturnToMenu(); }
    } 

    public static void TriggerDashEvent(){
        foreach (IPlayerEventListener listener in FindObjectsOfType<MonoBehaviour>().OfType<IPlayerEventListener>())
        { listener.OnDash(); }
    } 
}

public interface IKillboxEventListener : IWeaponEventListenter, IUniversalRoundEventListener
{}


// Round Based Events
public interface IBossRoundEventListener{
    public void OnBossRoundChange();
    public void OnBossRoundStart();
    public void OnBossRoundEnd();
    public void OnBossSpawn();
}

public interface IMainRoundEventListener{
    public void OnRoundChange();
    public void OnRoundStart(); 
    public void OnRoundEnd(); 
    public void OnPortalInteract(); 
}

public interface IUniversalRoundEventListener : IMainRoundEventListener, IBossRoundEventListener, IPlayerEventListener, IPurchaseEventListener, IRestartListener{}

// Weapon Based Events
public struct WeaponEventData
{
    public WeaponItem weapon {get; private set;}    
    public bool primary {get; private set;}    
    public bool dual {get; private set;}

    public WeaponEventData(WeaponItem _weapon, bool _primary, bool _dual = false){
        weapon = _weapon;
        primary = _primary;
        dual = _dual;
    }

    public int rarity 
    {
        get {return weapon.tier;} 
    }
    
}

public interface ICooldownEventListenter {
    public void OnCooldownBegin(WeaponEventData data); 
    public void OnCooldownEnd(WeaponEventData data); 
}

public interface IWeaponEventListenter  : ICooldownEventListenter{
    public void OnSwitchWeapon(WeaponEventData data); 
    public void OnFireWeapon(WeaponEventData data); 
    public void OnEquipWeapon(WeaponEventData data); 
}

// Player based events
public struct DamageEventData
{
    public int damage {get; private set;}    
    public string description {get; private set;}    
    public GameObject damagerObject {get; private set;}    

    public DamageEventData(int _damage, GameObject _damagerObject, string _description = ""){
        damage = _damage;
        description = _description;
        damagerObject = _damagerObject;
    }
    
}

public interface IEquipmentEventListener{
    public void OnEquipmentCharged();  
    public void OnTriggerEquipment();  
}

public interface IPlayerEventListener{
    
    public void OnTakeDamage(DamageEventData data); 
    public void OnDie(); 
    public void OnDash();  
    public void OnDashRecharged();  
    public void OnInteract();  
}

public interface IPurchaseEventListener{
    public void OnPurchaseWeapon(WeaponItem weapon, int cost);
    public void OnPurchaseUpgrade(Upgrade upgrade, int cost, int level);

}

public interface IShopUIEventListener{
    public void OnOpenShop(int shopId);
    public void OnCloseShop();
    public void OnSetNewWeapon(WeaponItem weaponItem, int slot);

}

public interface IProgressionListener{
    public void OnLevelUp();
    public void OnGainXp(int xp);

}

public interface ISelfResListener{
    public void OnSelfResPrompt();
    public void OnSelfRes();

}

public interface IRestartListener{
    public void OnRestartGame();
    public virtual void OnReturnToMenu(){}
}

public interface IBackButtonListener{
    public void OnBackButton(bool pressedThisFrame);
}

public interface IShopButtonStateListener{
    public void OnAnimationFinished();
}

public interface IUIButtonSelectListener{
    public void OnWeaponButtonSelect(WeaponItem weapon);
    public void OnUpgradeButtonSelect(Upgrade upgrade);
}