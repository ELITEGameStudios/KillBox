using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneFortressClass : MonoBehaviour
{

    private StateMachine states;
    private State _idle, _follow, _appear, _activate;

    private Transform player;

    private float _distance;
    private bool appeared = false;
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
    private int mode;

    public int active_progress {get; private set;}
    public int target_progress {get; private set;} = 5;

    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        FindBar();

        states = new StateMachine(gameObject);

        _idle = new RuneFortressIdle(gameObject, states);
        _follow = new RuneFollow(gameObject, states);
        _activate= new RuneFortressActive(gameObject, states);
        _appear = new RuneFortressAppear(gameObject, states, animator);

        states.SetFirstState(_idle);

        //_portal = GameObject.FindGameObjectWithTag("Portal").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        states.Update();
        _distance = Vector3.Distance(transform.position, player.position);

        if(_distance <= 5 && !appeared){
            states.SwitchState(_appear);
            appeared = true; }

        if(_distance <= 1 && !activated && !finished){
            Activate(); } 

        if(activated){
            ui_counter_slider.value = active_progress;
            ui_counter_text.text = active_progress.ToString() + " | " + target_progress.ToString();
            
            if(active_progress >= target_progress){
                Finish();
            }
        }
        if(!found_bar){
            FindBar();
        }

    }
    void FindBar(){

        if(GameObject.FindGameObjectWithTag("fortress_rune_ui") == null){
            return;
        }

        ui_element_root = GameObject.FindGameObjectWithTag("fortress_rune_ui").transform.GetChild(0).gameObject;
        ui_element_root.SetActive(false);

        ui_counter_text = ui_element_root.transform.GetChild(2).gameObject.GetComponent<Text>();
        ui_counter_slider = ui_element_root.transform.GetChild(0).gameObject.GetComponent<Slider>();
        found_bar = true;
    }

    void Activate(){
        activated = true;
        area = Instantiate(area_prefab, transform.position, transform.rotation);

        area.transform.SetParent(null);
        area.transform.localScale = new Vector3(1, 1, 1);

        ui_element_root.SetActive(true);
        ui_counter_slider.maxValue = target_progress;
    }

    void Finish(){
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
        states.SwitchState(_follow);

        RunicRoundManager.main.BlowWind(true);
    }

    public void AddKill(EnemyHealth caller){
        if(caller.CurrentHealth <= 0){
            active_progress++;
        }
    }


    //public void OnPortalInteraction(){
        
        //_portal.gameObject.GetComponent<PortalScript>().SetMode(mode);
        //Destroy(gameObject);
    //}

}
