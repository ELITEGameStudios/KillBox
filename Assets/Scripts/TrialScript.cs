using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TrialScript : MonoBehaviour
{
    [SerializeField]
    private GameObject child_button_obj, reward_obj;
    private Button child_button;

    [SerializeField]
    private GameObject player, button_prefab;
    [SerializeField]
    private UnityEvent reward_function;

    [SerializeField]
    private List<GameObject> summoned = new List<GameObject>();
    
    [SerializeField]
    private GameObject[] enemies;
    [SerializeField]
    private Transform[] spawns;

    private bool 
        player_is_nearby = false, 
        _is_enabled = false, 
        _is_finished_spawning = false,
        _has_claimed_reward = false;
    
    [SerializeField]
    private float _target_delay;
    private float dist, _delay, _current_delay_timer;

    private GameManager manager;

    [SerializeField]
    private int target_instances;
    private int instances, _target_spawn;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
        manager = GameObject.Find("Manager").GetComponent<GameManager>();

        child_button_obj = Instantiate(button_prefab, transform);



        child_button_obj.SetActive(false);
        child_button_obj.transform.SetParent(GameObject.Find("WorldUiCanvas").transform);
        child_button_obj.transform.Translate(0, 1, 0);
        child_button_obj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        child_button = child_button_obj.GetComponent<Button>();
        child_button.interactable = false;
        child_button_obj.GetComponent<TrialButtonScript>().summoner = this;
        child_button_obj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Prove Your Worth";

        _delay = _target_delay/Time.fixedDeltaTime;
    }

    void Update()
    {
        dist = Vector3.Distance(player.transform.position, gameObject.transform.position);
        if (dist < 2 && !player_is_nearby)
        {
            player_is_nearby = true;
            OnPlayerApproach();
        }

        else if (dist > 2 && player_is_nearby)
        {
            player_is_nearby = false;
            OnPlayerLeave();
        }

        foreach (GameObject enemy in summoned)
        {
            if(enemy == null){
                summoned.Remove(enemy);
            }
        }

        if(_is_finished_spawning && summoned.Count == 0 && !_has_claimed_reward){
            reward_function.Invoke();
            _has_claimed_reward = true;
            gameObject.GetComponent<Animator>().Play("trial_end");
        }


    }
    void FixedUpdate(){
        if(_current_delay_timer <= 0 && instances > 0){
            SpawnEntity(_target_spawn);

            _target_spawn++;
            if(_target_spawn == spawns.Length){
                _target_spawn = 0;
            }

            _current_delay_timer = _delay;


            if(instances == 0){
                _is_finished_spawning = true;
            }
        }

        _current_delay_timer -= 1;

    }

    void OnPlayerApproach()
    {
        if(!_is_enabled){
            child_button.interactable = true;
            child_button_obj.SetActive(true);
        }
    }

    void OnPlayerLeave()
    {
        child_button.interactable = false;
        child_button_obj.SetActive(false);
    }

    public void EndSequence(){
        reward_obj = Instantiate(reward_obj, transform.position, Quaternion.Euler(0, 0, 0));
        reward_obj.transform.SetParent(null);

        Destroy(gameObject);
    }

    public void StartChallenge()
    {
        instances = target_instances;
        _current_delay_timer = 0;
        _target_spawn = 0;

        _is_enabled = true;

        child_button.interactable = false;
        child_button_obj.SetActive(false);
        //StartCoroutine(BossSummon());


    }

    void SpawnEntity(int spawn_index)
    {
        GameObject new_enemy = Instantiate(enemies[Random.Range(0, enemies.Length)], spawns[spawn_index].position, transform.rotation); 
        new_enemy.transform.SetParent(null);

        summoned.Add(new_enemy);
        instances--;
    }

}
