using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitchScript : MonoBehaviour
{

    public Color output_color, output_particle_color;

    [SerializeField]
    private GameObject player, player_sprite;

    [SerializeField]
    private GameObject[] guns;
    
    public PortalScript portal_script;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        player_sprite = player.transform.GetChild(1).gameObject;

        guns = new GameObject[player.transform.GetChild(2).transform.childCount];

        for (int i = 0; i < player.transform.GetChild(2).transform.childCount; i++)
        {
            guns[i] = player.transform.GetChild(2).transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col){
        if(col.gameObject == player){
            player_sprite.GetComponent<SpriteRenderer>().color = output_color;

            for(int i = 0; i < guns.Length; i++){
                guns[i].transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().color = output_color;
                guns[i].GetComponent<shooterScript2D>().bullet_color = output_color;
                guns[i].GetComponent<shooterScript2D>().particle_color = output_particle_color;
            }

            player.transform.position = GameManager.main.GetMapByID(portal_script.currentMapIndex).Player.position;
        }
    }
}
