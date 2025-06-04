using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffPickupScript : MonoBehaviour
{

    public BuffsManager manager;

    [SerializeField]
    private int target_index;

    public Color[] buff_colors;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GameObject.FindWithTag("Player").GetComponent<BuffsManager>();
        target_index = Random.Range(0, 5);

        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = buff_colors[target_index];
    }



    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject.tag == "Player"){
            manager.RecieveBuff(target_index);
            Destroy(gameObject);
        }
    }
}
