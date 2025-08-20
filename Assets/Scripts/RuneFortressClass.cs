using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static BossRoundManager;

public class RuneFortressClass : MonoBehaviour
{

    public BossType bossType;
    // private StateMachine states;
    private State _idle, _follow, _appear, _activate;

    private Transform player;
    private float _distance;
    public float inRangeDistance = 5;
    private bool appeared = false;
    private bool playerInRange { get { return _distance < inRangeDistance; } }
    private bool activated = false;
    private bool finished = false;
    private bool found_bar = false;

    [SerializeField]
    private GameObject area_prefab, area, ui_element_root;

    private Slider ui_counter_slider;
    private Text ui_counter_text;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private int mode, requiredBulletHits, currentBulletHits;

    public int active_progress { get; private set; }
    public int target_progress { get; private set; } = 5;

    [SerializeField] private SimpleContinuousSpawner linkedSpawner;


    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        FindBar();

        // states = new StateMachine(gameObject);

        // _idle = new RuneFortressIdle(gameObject, states);
        // _follow = new RuneFollow(gameObject, states);
        // _activate= new RuneFortressActive(gameObject, states);
        // _appear = new RuneFortressAppear(gameObject, states, animator);

        // states.SetFirstState(_idle);

        //_portal = GameObject.FindGameObjectWithTag("Portal").transform;
    }

    // Update is called once per frame
    void Update()
    {
        // states.Update();
        _distance = Vector3.Distance(transform.position, player.position);
        animator.SetFloat("Blend", _distance);

        if(!playerInRange){ currentBulletHits = 0; }


        if (activated)
        {
            ui_counter_slider.value = active_progress;
            ui_counter_text.text = active_progress.ToString() + " | " + target_progress.ToString();

            if (active_progress >= target_progress)
            {
                Finish();
            }
        }
        if (!found_bar)
        {
            FindBar();
        }

    }
    void FindBar()
    {

        if (GameplayUI.instance.GetProgressBarObject() == null)
        {
            return;
        }

        ui_element_root = GameplayUI.instance.GetProgressBarObject();
        ui_element_root.SetActive(false);

        ui_counter_text = ui_element_root.transform.GetChild(2).gameObject.GetComponent<Text>();
        ui_counter_slider = ui_element_root.transform.GetChild(0).gameObject.GetComponent<Slider>();
        found_bar = true;
    }

    void Activate()
    {
        activated = true;
        linkedSpawner.enabled = true;
        animator.Play("Sleep");
        GetComponent<Collider2D>().enabled = false;

        area = Instantiate(area_prefab, transform.position, transform.rotation);

        area.transform.SetParent(null);
        area.transform.localScale = new Vector3(1, 1, 1);

        ui_element_root.SetActive(true);
        ui_counter_slider.maxValue = target_progress;

        PortalScript.main.gameObject.SetActive(false);

    }

    void Finish()
    {
        linkedSpawner.enabled = false;

        ParticleSystem[] children = new ParticleSystem[] {
            area.transform.GetChild(0).gameObject.GetComponent<ParticleSystem>(),
            area.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>(),
            area.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>()
        };

        foreach (ParticleSystem item in children)
        { item.loop = false; }

        Destroy(area.transform.GetChild(3).gameObject);

        activated = false;
        finished = true;

        ui_element_root.SetActive(false);

        EnemyCounter.main.DestroyAllEnemies();

        PortalScript.main.gameObject.SetActive(true);
        PortalScript.main.SetMode(1, bossType: bossType);
        PortalScript.main.transform.position = transform.position;

        // states.SwitchState(_follow);

        // RunicRoundManager.main.BlowWind(true);
    }

    public void AddKill(EnemyHealth caller)
    {
        if (caller.CurrentHealth <= 0)
        {
            active_progress++;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.GetComponent<BulletClass>() != null)
        {
            if (playerInRange && !activated)
            {
                currentBulletHits++;
                col.gameObject.GetComponent<BulletDestroy>().ResetRangeCallWhenHit();
                animator.SetTrigger("Hit");
                if (requiredBulletHits <= currentBulletHits) { Activate(); }
            }
        }
    }


    //public void OnPortalInteraction(){

    //_portal.gameObject.GetComponent<PortalScript>().SetMode(mode);
    //Destroy(gameObject);
    //}

}
