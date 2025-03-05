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
        if (collision.gameObject.CompareTag("DroppedToken"))
        {
            manager.OnPickupToken(1);
            collision.gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("DroppedTutorialToken"))
        {
            TutorialManager.tutorialManager.tokenProgress++;
            manager.OnPickupToken(1);

            InventoryUIManager.Instance.UpdateUI();
            UpgradesManager.Instance.ChooseUpgrade();

            Destroy(collision.gameObject);
        }
    }
}