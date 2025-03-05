using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopModelToggleOnClass : MonoBehaviour
{
    public GameObject[] weapon_graphics;
    [SerializeField]
    private ShopScript shop;

    // Start is called before the first frame update
    void Awake()
    {
        shop = GameObject.Find("Manager").GetComponent<ShopScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
