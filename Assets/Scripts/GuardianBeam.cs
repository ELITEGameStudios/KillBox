using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardianBeam : MonoBehaviour
{
    private bool phase_one = true;
    public SpriteRenderer renderer;
    [SerializeField]private Collider2D mainCollider;
    //public ParticleSystem particles;

    [SerializeField]
    private int state, warningFlashes;

    [SerializeField]
    private float width, time, color_rate, target_col_y, timer;

    [SerializeField]
    private Transform beam_obj;
    private GameObject player;

    [SerializeField]
    private Vector3 end_pos = new Vector3(0, -100, 0);

    private EnemyDamage enemyDamage;
    // private Collider2D collider;
    [SerializeField] private Color warningColor;
    [SerializeField] private bool active;

    // Start is called before the first frame update
    void Awake()
    {
        enemyDamage = transform.GetChild(0).gameObject.GetComponent<EnemyDamage>();
    }

    void OnEnable(){
        beam_obj.transform.localScale = new Vector3(0, 200, 1);
        //collider.enabled = false;
        enemyDamage.enabled = false;
        renderer.color = new Color(255f, 255f, 255f, 0f);

        player = Player.main.obj;
    }

    public void BeginSequence(float _width, float _time){
        state = 0;

        width = _width;
        time = _time;
        timer = time;

        renderer.color += new Color(5f, 5f, 5f, 0);
        active = true;

    }

    // Update is called once per frame
    void Update()
    {
        //float collider_pos = collider.offset.y - 0.5f * 200; 

        Vector3 base_position = beam_obj.transform.localPosition;
        // Debug.Log(" BASE POSITION: " + base_position);
        base_position.y -= 100;
        // Debug.Log(" BASE POSITION 222: " + base_position);
        base_position = transform.TransformPoint(base_position);
        // Debug.Log(" BASE POSITION 333: " + base_position);

        //Debug.Log(Vector2.Distance(base_position, player.transform.position));

        //float target_col_y = ((Vector2.Distance(base_position, player.transform.position)+end_pos.y) / 200);
        target_col_y = (Vector2.Distance(base_position, player.transform.position) / 200f) + (-100f / 200f);
        mainCollider.offset = new Vector2(0, target_col_y);

        if(!active) {return;}
        
        switch(state){
            case 0:
                if (timer > 0)
                {
                    // renderer.color += new Color(color_rate * Time.deltaTime / time, color_rate * Time.deltaTime / time, color_rate * Time.deltaTime / time, (1 * Time.deltaTime) / time);
                    renderer.color = 
                        Color.Lerp(Color.clear, warningColor, 0.5f * Mathf.Sin(2*Mathf.PI * (warningFlashes * timer - 0.25f)) + 0.5f);
                    timer -= Time.deltaTime;
                    beam_obj.transform.localScale += new Vector3(width * Time.deltaTime * 0.5f, 0, 0);
                    mainCollider.enabled = false;
                     
                }

                else{ 
                    renderer.color = Color.white;
                    beam_obj.transform.localScale = new Vector3(width, 200, 1);
                    //collider.enabled = true;
                    enemyDamage.enabled = true;
                    mainCollider.enabled = true;
                    //particles.Play();

                    state++; 
                }

                break;

            case 1:
                if(timer < time)
                {
                    beam_obj.transform.localScale -= new Vector3(((Time.deltaTime) / time) * width , 0, 0);
                    renderer.color -= new Color(0f, 0f, 0f, (Time.deltaTime) / time);
                    timer += Time.deltaTime;
                }
                else{
                    state++;
                    mainCollider.enabled = false;
                    enemyDamage.enabled = false;
                    active = false;
                    gameObject.SetActive(false);
                    // Destroy(gameObject);
                    //gameObject.SetActive(false);
                }

                break;

        }      
    }
}
