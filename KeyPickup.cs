using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    [SerializeField]
    private GameManager manager;

    [SerializeField]
    private GameObject pickup_effects;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            manager.HasKey = true;
            pickup_effects.SetActive(true);
            pickup_effects.transform.SetParent(null);
            pickup_effects.transform.localScale = new Vector3(1, 1, 1);

            Destroy(gameObject);
        }
    }
}
