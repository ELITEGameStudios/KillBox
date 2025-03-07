using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using KillboxShopClasses;
using KillboxWeaponClasses;

public class ShopScript : MonoBehaviour
{
    public GameManager manager;
    public PlayerHealth playerHealth;
    public GameObject Ar2, DWButton, ErrorMessage;
    public GameObject[] Guns, ToggleOnBuy, ToggleOffBuy, EquipButtons, WeaponGraphicSprites, weapon_buttons;
    public GameObject[] SecondGun;

    [SerializeField]
    private StatsDisplaySorter[] display_sorters;

    public int[] Costs, OwnedGuns;
    public int CurrentGunIndex, second_gun_index;
    public TwoDPlayerController PlayerController;
    public UIWeaponGraphicHandler[] graphicsListUI;
    public bool DualEnabled = false;
    public UnityEvent[] ToggleEvents, on_equip;

    public UnityEvent OnHpMax, OnSpeedMax;

    public Color lvl1Color, lvl2Color, lvl3Color, lvl4Color, NAColor;

    public bool[] weapon_class, weapon_class2;

    public int[] ar_id, smg_id, shotgun_id, marksman_id, launcher_id;

    [SerializeField]
    private Button[] toggle_on_buttons, toggle_off_buttons;


    void Awake(){
        OwnedGuns = new int[Guns.Length];
    }
    void Update()
    {
        //if(!DualEnabled)
        //{
        //    for (int i = 0; i < SecondGun.Length; i++)
        //    {
        //        SecondGun[i].SetActive(false);
        //    }
        //}
    }

    public void PurchaseGun(string key)
    {

        PurchaseRequest result = PurchaseRequest.BuyWeapon(key, manager.ScoreCount);

        if (result != null && result.purchased)
        {
            manager.SetLowerScore(result.money);

            GunHandler.Instance.NewItem(WeaponItemList.Instance.GetItem(key));
            KillboxEventSystem.TriggerPurchaseWeaponEvent( WeaponItemList.Instance.GetItem(key), WeaponItemList.Instance.GetItem(key).price);
        }
    }
    public int PurchaseUpgrade(Upgrade upgrade, int level)
    {

        int result = upgrade.Transaction(manager.ScoreCount, level, this);

        if (result != -1)
        {
            manager.ScoreCount = result;
            ChallengeFields.UpdateUpgrades(this, upgrade, level);
            KillboxEventSystem.TriggerPurchaseUpgradeEvent( upgrade, upgrade.costs[level], level);
            return 1;
        }
        else{
            return -1;
        }
    }

    public void ToggleAfterBuy(int ToggleIndex)
    {
        for (int i = 0; i < weapon_class.Length; i++)
        {
            weapon_class[i] = false;
        }

        for (int i = 0; i < ar_id.Length; i++)
        {
            if (ToggleIndex == ar_id[i])
            {
                weapon_class[0] = true;
            }
        }

        for (int i = 0; i < smg_id.Length; i++)
        {
            if (ToggleIndex == smg_id[i])
            {
                weapon_class[1] = true;
            }
        }

        for (int i = 0; i < shotgun_id.Length; i++)
        {
            if (ToggleIndex == shotgun_id[i])
            {
                weapon_class[2] = true;
            }
        }

        for (int i = 0; i < marksman_id.Length; i++)
        {
            if (ToggleIndex == marksman_id[i])
            {
                weapon_class[3] = true;
            }
        }

        for (int i = 0; i < launcher_id.Length; i++)
        {
            if (ToggleIndex == launcher_id[i])
            {
                weapon_class[4] = true;
            }
        }

        ToggleEvents[ToggleIndex].Invoke();

        for (int i = 0; i < WeaponGraphicSprites.Length; i++)
        {
            UIWeaponGraphicHandler graphics = WeaponGraphicSprites[i].GetComponent<UIWeaponGraphicHandler>();
            graphics.CheckFunction();
        }
    }

    public void PurchaseDualWield()
    {
        if(manager.ScoreCount >= 20){
            manager.SetLowerScore(manager.ScoreCount - 20);
            GunHandler.Instance.PurchaseDual();
        }
    }
    
    //    if (manager.ScoreCount >= Costs[0])
    //    {
    //        manager.ScoreCount -= Costs[0];
    //        DualEnabled = true;
//
    //        manager.tokens_used += Costs[0];
//
    //        for (int i = 0; i < SecondGun.Length; i++)
    //        {
    //            SecondGun[i].SetActive(false);
    //        }
//
    //        SecondGun[CurrentGunIndex].SetActive(true);
//
    //        DWButton.SetActive(false);
//
    //        if(Guns[0].activeInHierarchy != true){
    //            SecondGun[0].SetActive(false);
    //        }
//
    //        //classifying class
    //        for (int i = 0; i < weapon_class2.Length; i++)
    //        {
    //            weapon_class2[i] = false;
    //        }
//
    //        for (int i = 0; i < ar_id.Length; i++)
    //        {
    //            if (CurrentGunIndex == ar_id[i])
    //            {
    //                weapon_class2[0] = true;
    //            }
    //        }
//
    //        for (int i = 0; i < smg_id.Length; i++)
    //        {
    //            if (CurrentGunIndex == smg_id[i])
    //            {
    //                weapon_class2[1] = true;
    //            }
    //        }
//
    //        for (int i = 0; i < shotgun_id.Length; i++)
    //        {
    //            if (CurrentGunIndex == shotgun_id[i])
    //            {
    //                weapon_class2[2] = true;
    //            }
    //        }
//
    //        for (int i = 0; i < marksman_id.Length; i++)
    //        {
    //            if (CurrentGunIndex == marksman_id[i])
    //            {
    //                weapon_class2[3] = true;
    //            }
    //        }
//
    //        for (int i = 0; i < launcher_id.Length; i++)
    //        {
    //            if (CurrentGunIndex == launcher_id[i])
    //            {
    //                weapon_class2[4] = true;
    //            }
    //        }
//
    //        second_gun_index = CurrentGunIndex;
//
    //        for (int i = 0; i < WeaponGraphicSprites.Length; i++)
    //        {
    //            UIWeaponGraphicHandler graphics = WeaponGraphicSprites[i].GetComponent<UIWeaponGraphicHandler>();
    //            graphics.CheckDualFunction();
    //        }

            //for (int i = 0; i < WeaponGraphicSprites.Length; i++){
            //    UIWeaponGraphicHandler graphics = WeaponGraphicSprites[i].GetComponent<UIWeaponGraphicHandler>();
            //    if(graphics.Dual){
            //        if(graphics.WeaponID1 == CurrentGunIndex){
            //            WeaponGraphicSprites[i].GetComponent<Image>().color = lvl1Color;
            //        }
            //        else{
            //            if (graphics.WeaponID2 == CurrentGunIndex)
            //                WeaponGraphicSprites[i].GetComponent<Image>().color = lvl2Color;
            //            else{
            //                if (graphics.WeaponID3 == CurrentGunIndex)
            //                    WeaponGraphicSprites[i].GetComponent<Image>().color = lvl3Color;
            //                else
            //                {
            //                    if (graphics.WeaponID4 == CurrentGunIndex)
            //                        WeaponGraphicSprites[i].GetComponent<Image>().color = lvl4Color;
            //                    else
            //                    {
            //                        WeaponGraphicSprites[i].GetComponent<Image>().color = Color.clear;
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        //}
        //else
        //{
        //    StartCoroutine(Error());
        //}
    //}

    public void EquipGun(int GunIndex)
    {
        for(int i = 0; i < Guns.Length; i++)
        {
            Guns[i].SetActive(false);
        }
        Guns[GunIndex].SetActive(true);
        CurrentGunIndex = GunIndex;
        DWButton.SetActive(true);
        // Change Texture Code Goes Here
        Player.main.PrimaryGunGraphic.sprite = WeaponItemList.Instance.inGameWeaponTextures[GunIndex];

        for (int i = 0; i < weapon_class.Length; i++)
        {
            weapon_class[i] = false;
        }

        for (int i = 0; i < ar_id.Length; i++)
        {
            if (GunIndex == ar_id[i])
            {
                weapon_class[0] = true;
            }
        }

        for (int i = 0; i < smg_id.Length; i++)
        {
            if (GunIndex == smg_id[i])
            {
                weapon_class[1] = true;
            }
        }

        for (int i = 0; i < shotgun_id.Length; i++)
        {
            if (GunIndex == shotgun_id[i])
            {
                weapon_class[2] = true;
            }
        }

        for (int i = 0; i < marksman_id.Length; i++)
        {
            if (GunIndex == marksman_id[i])
            {
                weapon_class[3] = true;
            }
        }

        for (int i = 0; i < launcher_id.Length; i++)
        {
            if (GunIndex == launcher_id[i])
            {
                weapon_class[4] = true;
            }
        }


        for (int ii = 0; ii < OwnedGuns.Length; ii++)
        {
            if (OwnedGuns[ii] != 0 && EquipButtons[OwnedGuns[ii]] != EquipButtons[GunIndex] && EquipButtons[GunIndex] != null)
            {
                if (EquipButtons[OwnedGuns[ii]] != null)
                {

                    EquipButtons[OwnedGuns[ii]].SetActive(true);
                    weapon_buttons[OwnedGuns[ii]].SetActive(false);
                }
            }
            else
            {
                if (ii > 0)
                {
                    if (EquipButtons[OwnedGuns[ii]] != null)
                    {
                        weapon_buttons[OwnedGuns[ii]].SetActive(true);
                        EquipButtons[OwnedGuns[ii]].SetActive(false);
                    }
                }
            }
        }


        for (int i = 0; i < WeaponGraphicSprites.Length; i++)
        {
            UIWeaponGraphicHandler graphics = WeaponGraphicSprites[i].GetComponent<UIWeaponGraphicHandler>();
            graphics.CheckFunction();
        }

        //for (int i = 0; i < WeaponGraphicSprites.Length; i++){
        //    UIWeaponGraphicHandler graphics = WeaponGraphicSprites[i].GetComponent<UIWeaponGraphicHandler>();
        //    if(!graphics.Dual){
        //        if(graphics.WeaponID1 == GunIndex){
        //            WeaponGraphicSprites[i].GetComponent<Image>().color = lvl1Color;
        //        }
        //        else{
        //            if (graphics.WeaponID2 == GunIndex)
        //                WeaponGraphicSprites[i].GetComponent<Image>().color = lvl2Color;
        //            else{
        //                if (graphics.WeaponID3 == GunIndex)
        //                    WeaponGraphicSprites[i].GetComponent<Image>().color = lvl3Color;
        //                else
        //                {
        //                    WeaponGraphicSprites[i].GetComponent<Image>().color = Color.white;
        //                }
        //            }
        //        }
        //    }
        //}


        DWButton.SetActive(true);

        for (int i = 0; i < WeaponGraphicSprites.Length; i++)
        {
            UIWeaponGraphicHandler graphics = WeaponGraphicSprites[i].GetComponent<UIWeaponGraphicHandler>();
            graphics.CheckFunction();
        }

        //for (int i = 0; i < graphicsListUI.Length; i++){
        //    if(graphicsListUI[i].Dual == false){
        //        graphicsListUI[i].Dissapear();
        //        graphicsListUI[i].CheckFunction();
        //    }
        //}
    }

    //public void FreeGun(Lvl4PickupScript caller){
    //    GunHandler.Instance.NewItem(caller.target_item);
    //}
    //    for(int i = 0; i < Guns.Length; i++)
    //    {
    //        Guns[i].SetActive(false);
    //    }
//
    //    Guns[GunIndex].SetActive(true);
    //    CurrentGunIndex = GunIndex;
    //    ToggleAfterBuy(GunIndex);
    //    DWButton.SetActive(true);
//
//
    //    for (int i = 0; i < OwnedGuns.Length; i++)
    //    {
    //        if(OwnedGuns[i] == 0){
    //            OwnedGuns[i] = GunIndex;
    //            break;
    //        }
    //    }
//
    //    for (int ii = 0; ii < OwnedGuns.Length; ii++)
    //    {
    //        if (OwnedGuns[ii] != 0 && EquipButtons[OwnedGuns[ii]] != EquipButtons[GunIndex] && EquipButtons[GunIndex] != null)
    //        {
    //            if (EquipButtons[OwnedGuns[ii]] != null)
    //            {
//
    //                EquipButtons[OwnedGuns[ii]].SetActive(true);
    //                weapon_buttons[OwnedGuns[ii]].SetActive(false);
    //            }
    //        }
    //        else
    //        {
    //            if (ii > 0)
    //            {
    //                if (EquipButtons[OwnedGuns[ii]] != null)
    //                {
    //                    weapon_buttons[OwnedGuns[ii]].SetActive(true);
    //                    EquipButtons[OwnedGuns[ii]].SetActive(false);
    //                }
    //            }
    //        }
    //    }
    //}


    IEnumerator Error()
    {
        ErrorMessage.SetActive(true);
        yield return new WaitForSeconds(2);
        ErrorMessage.SetActive(false);
    }
}
