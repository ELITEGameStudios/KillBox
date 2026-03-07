using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemHandler : MonoBehaviour
{
    public Transform PlayerTransform;
    public GameManager manager;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        ItemPickup item = collision.GetComponent<ItemPickup>();

        if(item != null){
            item.OnPickup();
        }

        // if (collision.gameObject.CompareTag("DroppedTutorialToken"))
        // {
        //     TutorialManager.tutorialManager.tokenProgress++;
        //     manager.OnPickupToken(1);

        //     InventoryUIManager.Instance.UpdateUI();
        //     UpgradesManager.Instance.ChooseUpgrade();

        //     Destroy(collision.gameObject);
        // }
    }
}