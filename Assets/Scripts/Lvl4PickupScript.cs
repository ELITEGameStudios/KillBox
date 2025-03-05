
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lvl4PickupScript : MonoBehaviour
{
    public GameObject manager;
    public int tier;

    private bool picked_up;

    public WeaponItem target_item;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(GunHandler.Instance.owned_weapons.Count);

        manager = GameObject.Find("Manager");

        List<WeaponItem> list = WeaponItemList.Instance.GetItemsOfTier(tier);

        List<WeaponItem> new_list = new List<WeaponItem>();


        for(int i = 0; i < list.Count; i++){

            for(int j = 0; j < GunHandler.Instance.owned_weapons.Count; j++){

                if(list[i].weapon != GunHandler.Instance.owned_weapons[j].weapon){

                    if(j + 1 == GunHandler.Instance.owned_weapons.Count){
                        new_list.Add(list[i]);
                        Debug.Log("dertidddd");
                    }
                }
                else{
                    Debug.Log("Denied");
                    break;
                }
                

                Debug.Log("iterd");
            }
        }

        if(new_list.Count > 0){
            int choice = Random.Range(0, new_list.Count);
            target_item = new_list[choice];

            SpriteRenderer renderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            renderer.color = InventoryUIManager.Instance.tier_colors[tier];
            renderer.sprite = target_item.graphic;
        }
        else if (list.Count > 0){
            int choice = Random.Range(0, list.Count);
            target_item = list[choice];

            SpriteRenderer renderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
            renderer.color = InventoryUIManager.Instance.tier_colors[tier];
            renderer.sprite = target_item.graphic;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(picked_up){
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            //manager.GetComponent<ShopScript>().FreeGun(this);
            picked_up = true;
            target_item.FreeTransaction(this);
            GunHandler.Instance.NewItem(target_item, true);
            Destroy(gameObject);
        }
    }
}
