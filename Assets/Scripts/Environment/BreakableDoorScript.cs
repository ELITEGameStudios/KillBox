using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BreakableDoorScript : MonoBehaviour
{
    [SerializeField] private int default_hp, current_hp;
    [SerializeField] private bool broken, resets_on_next_round;
    [SerializeField] private Door door;
    [SerializeField] private Text UI;

    // Start is called before the first frame update
    void Break()
    {
        broken = true;
        UI.text = "";
        door.Open();
    }
    
    public void Reset()
    {
        broken = false;
        current_hp = default_hp;
        UI.text = current_hp.ToString();
    }

    void Update(){

    }

    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletClass bulletScript = collision.gameObject.GetComponent<BulletClass>();
            current_hp -= bulletScript.damage;
            collision.gameObject.GetComponent<BulletDestroy>().ResetRangeCallWhenHit();

            if(current_hp <= 0 && !broken){
                Break();
            }
            else if(!broken){
                UI.text = current_hp.ToString();
                door.Interact();
            }
        }
    }
}
