using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScytheAiScript : MonoBehaviour
{
    [SerializeField] private float timer, target_time;
    [SerializeField] private bool thrown_scythe;
    [SerializeField] private BossScythe scythe;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private SpriteRenderer graphic, scythe_graphic;
    [SerializeField] private Collider2D collider, scythe_collider;
    [SerializeField] private FixedRotator rotator;
    [SerializeField] private IEnumerator thrower;
    [SerializeField] private IEnumerator teleporter;
    // Start is called before the first frame update
    void Start()
    {
        BossBarManager.Instance.AddToQueue(gameObject, "SCYTHE");
        
        thrower = ThrowAttackNumerator();
        StartCoroutine(thrower);
        teleporter = TpAttackPhase(2, 3, 2);
    }

    // Update is called once per frame
    void Update()
    {
    
        // if (timer <= 0){
        //     thrown_scythe = !thrown_scythe;
        //     timer = target_time;
        //     if(thrown_scythe){
        //         scythe.IdleState();
        //     }
        //     else{
        //         scythe.Throw();
        //     }
        // 
        // if(Application.isFocused) {timer -= Time.deltaTime;}
    
    }

    void Dissapear(){
        rb.simulated = false;
        graphic.enabled = false;
        scythe_graphic.enabled = false;
        collider.enabled = false;
        scythe_collider.enabled = false;
    }

    void InitTp(){
        teleporter = TpAttackPhase(2, 3, 2);
        StartCoroutine(teleporter);
    }

    void InitThrow(){
        StopCoroutine(thrower);
        thrower = ThrowAttackNumerator();
        StartCoroutine(thrower);
    }

    void Appear(){
        rb.simulated = true;
        graphic.enabled = true;
        scythe_graphic.enabled = true;
        collider.enabled = true;
        scythe_collider.enabled = true;
    }


    IEnumerator TpAttackPhase(float rate, int times, float max_distance){

        //float tp_timer = rate;
        for (int i = 0; i < times; i++)
        {   

            yield return new WaitForSeconds(rate);
            transform.position = new Vector3(
                Player.main.tf.position.x + Random.Range(-max_distance, max_distance),
                Player.main.tf.position.y + Random.Range(-max_distance, max_distance),
                Player.main.tf.position.z + Random.Range(-max_distance, max_distance)
            );
            Appear();

            transform.LookAt(Player.main.tf);
            transform.Rotate(0, 0, -135);
            transform.localEulerAngles = new Vector3(0, 0, transform.rotation.z);
            rotator.enabled = true;

            yield return new WaitForSeconds(1);
            rotator.enabled = false;
            Dissapear();
        }

        Appear();
        InitThrow();
        StopCoroutine(teleporter);
    }

    IEnumerator ThrowAttackNumerator(){
        Appear();
        scythe.Throw();
        yield return new WaitForSeconds(8);
        scythe.IdleState();
        Dissapear();

        InitTp();

    }
}
