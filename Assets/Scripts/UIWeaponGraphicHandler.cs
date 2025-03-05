using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWeaponGraphicHandler : MonoBehaviour
{
    public int WeaponID1, WeaponID2, WeaponID3, WeaponID4;
    public int[] WeaponID;
    public bool Dual, Animated;

    public bool[] weapon_class;

    public Color[] colors;

    [SerializeField]
    private ShopScript shop;

    public bool PlayAnim = false, launcher_ui;
    public Image image;
    public Animator animator;

    void Awake(){

        WeaponID = new int[] {WeaponID1, WeaponID2, WeaponID3, WeaponID4};
        shop = GameObject.Find("Manager").GetComponent<ShopScript>();
        if (image == null)
            image = gameObject.GetComponent<Image>();
        if (Animated && animator == null)
        {
            animator = gameObject.GetComponent<Animator>();
        }
    }
    void Update(){
        if(image == null)
            image = gameObject.GetComponent<Image>();
        if(Animated && animator == null){
            animator = gameObject.GetComponent<Animator>();
        }
    }

    public void Dissapear(){
        image.color = new Color(1f, 1f, 1f, 0f);
    }
    public void CheckFunction(){

        if (shop.weapon_class[0] == weapon_class[0] && weapon_class[0] && !Dual)
        {
            for (int i = 0; i < 4; i++) {
                if (shop.CurrentGunIndex == shop.ar_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                    break;
                }
            }
        }

        else if (shop.weapon_class[1] == weapon_class[1] && weapon_class[1] && !Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.CurrentGunIndex == shop.smg_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                    break;
                }
            }
        }

        else if (shop.weapon_class[2] == weapon_class[2] && weapon_class[2] && !Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.CurrentGunIndex == shop.shotgun_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                }
            }
        }

        else if (shop.weapon_class[3] == weapon_class[3] && weapon_class[3] && !Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.CurrentGunIndex == shop.marksman_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                }
            }
        }

        else if (shop.weapon_class[4] == weapon_class[4] && weapon_class[4] && !Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.CurrentGunIndex == shop.launcher_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                }
            }
        }

        else if(!Dual)
        {
            image.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void CheckDualFunction(){
        if (shop.weapon_class2[0] == weapon_class[0] && weapon_class[0] && Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.second_gun_index == shop.ar_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                    break;
                }
            }
        }

        else if (shop.weapon_class2[1] == weapon_class[1] && weapon_class[1] && Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.second_gun_index == shop.smg_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                    break;
                }
            }
        }

        else if (shop.weapon_class2[2] == weapon_class[2] && weapon_class[2] && Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.second_gun_index == shop.shotgun_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                }
            }
        }

        else if (shop.weapon_class2[3] == weapon_class[3] && weapon_class[3] && Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.second_gun_index == shop.marksman_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                }
            }
        }

        else if (shop.weapon_class2[4] == weapon_class[4] && weapon_class[4] && Dual)
        {
            for (int i = 0; i < 4; i++)
            {
                if (shop.second_gun_index == shop.launcher_id[i])
                {
                    image.color = colors[i];
                    PlayAnim = true;
                }
            }
        }

        else if (Dual)
        {
            image.color = new Color(1f, 1f, 1f, 0f);
        }
    }


//        if(Dual){
//            if(ID == WeaponID1){
//                image.color = new Color(1f, 1f, 0f, 1f);
//            }
//            else
//            {
//                if(ID == WeaponID2){
//                    image.color = new Color(0f, 1f, 1f, 1f);
//                }
//                else
//                {
//                    if(ID == WeaponID3){
//                        image.color = new Color(1f, 0f, 1f, 1f);
//                    }
//                    else{
//                        image.color = new Color(1f, 1f, 1f, 0f);
//                    }
//                }
//            }
//        }
//        else{
//            switch (ID)
//            {
//                case WeaponID1const:
//                    image.color = new Color(1f, 1f, 0f, 1f);
//                    PlayAnim = true;
//                    break;
//
//                case WeaponID2const:
//                    image.color = new Color(0f, 1f, 1f, 1f);
//                    PlayAnim = true;
//                    break;
//
//                case WeaponID3const:
//                    image.color = new Color(1f, 0f, 1f, 1f);
//                    PlayAnim = true;
//                    break;
//
//                default:
//                    image.color = new Color(1f, 1f, 1f, 1f);
//                    break;
//                
//            }
//        }
}
