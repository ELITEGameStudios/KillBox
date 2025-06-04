using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopAudioHandler : MonoBehaviour, IPurchaseEventListener, IShopUIEventListener, IUIButtonSelectListener, IMainRoundEventListener ,IBossRoundEventListener
{
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip purchaseClip, selectClip, roundClip;


    public void OnCloseShop()
    {
        source.Stop();
        source.time = 0;
        source.clip = selectClip;

        source.pitch = 0.7f;
        source.Play();
    }

    public void OnOpenShop(int shopId)
    {
        source.Stop();
        source.time = 0;
        source.clip = selectClip;

        source.pitch = 0.7f;
        source.Play();
    }

    public void OnPortalInteract()
    {
        throw new System.NotImplementedException();
    }

    public void OnPurchaseUpgrade(Upgrade upgrade, int cost, int level)
    {
        source.Stop();
        source.time = 0;
        source.clip = purchaseClip;

        source.pitch = 1 + ((float)level / 30);
        source.Play();
    }

    public void OnPurchaseWeapon(WeaponItem weapon, int cost)
    {
        source.Stop();
        source.time = 0;
        source.clip = purchaseClip;

        source.pitch = Random.Range(0.9f, 1.1f) - ((float)weapon.tier / 10) ;
        source.Play();
    }


    public void OnSetNewWeapon(WeaponItem weaponItem, int slot)
    {
        // source.Stop();
        // source.time = 0;
        // source.clip = selectClip;

        // source.pitch = Random.Range(0.9f, 1.2f) + ((float)weaponItem.tier / 20) ;
        // source.Play();
    }

    public void OnUpgradeButtonSelect(Upgrade upgrade)
    {
        if(GameManager.main == null){return;}
        if(GameManager.main.started_game){

            source.Stop();
            source.time = 0;
            source.clip = selectClip;

            source.pitch = Random.Range(1f, 1.1f);
            source.Play();
        }
    }

    public void OnWeaponButtonSelect(WeaponItem weapon)
    {
        if(GameManager.main.started_game){

            source.Stop();
            source.time = 0;
            source.clip = selectClip;

            source.pitch = Random.Range(1f, 1.1f) + ((float)weapon.tier / 10) ;
            source.Play();
        }
    }
    public void OnRoundChange()
    {}

    public void OnRoundEnd()
    {}

    public void OnRoundStart()
    {}
    public void OnBossRoundChange()
    {}

    public void OnBossRoundEnd()
    {}

    public void OnBossRoundStart()
    {}

    public void OnBossSpawn()
    {}
}
