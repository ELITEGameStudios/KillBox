using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth, CurrentHealth, a;
    public GameManager manager;
    public GameObject player, DroppedItem, ExplosionOnDeath, color_easter_egg_portal, InsObject, DpdItemClone, main_player, key_item, guaranteed_drop_item;
    public GameObject[] lvl_4_prefabs;
    public ObjectPool[] objectPool;
    public Color explosionColor;
    public AudioSource audio;
    public AudioClip death, hit;
    public bool can_drop_lvl_4, destructive_immune, no_drops, key_drops, guaranteed_drop_bool, triggerDeathEvent;
    public bool in_fortress {get; private set;}
    [SerializeField] private EnemyProfile profile; 
    [SerializeField] private UnityEvent onTakeDamage, onDie; 

    void Awake(){
        if (profile != null) {maxHealth = profile.MaxHealth;}
    }
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
        objectPool[0] = GameObject.Find("BulletPool5").GetComponent<ObjectPool>();
        objectPool[1] = GameObject.Find("BulletPool6").GetComponent<ObjectPool>();
        audio = gameObject.GetComponent<AudioSource>();

        main_player = GameObject.FindWithTag("Player");
    }

    void OnTriggerEnter2D(Collider2D trigger){
        if(trigger.gameObject.tag == ("Destructive") && !destructive_immune){
            Destroy(gameObject);
        }
        if(trigger.tag == "rune_fortress_field"){
            in_fortress = true;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(CurrentHealth == 0)
        {
            Die();
        }

        else
        {
            if(CurrentHealth < 0)
            {
                Die();
            }
        }

    }
    public void TakeDmg(int Dmg)
    {
        CurrentHealth -= Dmg;
        onTakeDamage.Invoke();

        audio.clip = hit;
        audio.pitch = Random.Range(0.8f, 1.2f);
        audio.volume = 0.26f;
        audio.Play();
    }
    public void Die(bool to_player = true)
    {
        a = Random.Range(1, 6);
        if (profile.hasDrop || (KillBox.currentGame.round > 45 && a == 1))
        {
            if (!BossRoundManager.main.isBossRound && to_player)
            {
                // Instantiates a token drop
                if (objectPool[0].GetPooledObject() != null)
                {
                    DpdItemClone = objectPool[0].GetPooledObject();

                    DpdItemClone.transform.SetParent(null);
                    DpdItemClone.GetComponent<sine_movement>().ROOT = transform.position;
                    DpdItemClone.transform.position = transform.position;

                    DpdItemClone.gameObject.SetActive(true);


                    Transform grid = GameObject.Find("Grid").transform;

                    for (int i = 0; i < grid.childCount; i++)
                    {
                        if (grid.GetChild(i).gameObject.activeInHierarchy)
                        {
                            DpdItemClone.transform.SetParent(grid.GetChild(i));
                            DpdItemClone.transform.localEulerAngles = new Vector3(0, 0, 0);
                            DpdItemClone.transform.position = transform.position;
                            DpdItemClone.transform.rotation = transform.rotation;
                            break;
                        }
                    }
                }
            }
        }
        profile.Retire();
        if(triggerDeathEvent){onDie.Invoke();}


        //int b = Random.Range(1, 800);
        //if(b == 1 && !no_drops){
        //    GameObject color_ee_instance = Instantiate(color_easter_egg_portal, transform);
        //    color_ee_instance.transform.SetParent(null);
        //}

        // int c = Random.Range(1, 1500);
        // if(c == 1 & can_drop_lvl_4 && !no_drops){
        //     GameObject gun_drop = Instantiate(lvl_4_prefabs[Random.Range(0, 4)], transform);
        //     gun_drop.transform.SetParent(null);
        //     gun_drop.transform.localEulerAngles =new Vector3(0, 0, 0);
        // }
        try
        {


            if (objectPool[1].GetPooledObject() != null)
            {
                InsObject = objectPool[1].GetPooledObject();
                InsObject.transform.position = gameObject.transform.position;
                InsObject.transform.rotation = gameObject.transform.rotation;
                InsObject.gameObject.SetActive(true);
                InsObject.GetComponent<ParticleSystem>().startColor = explosionColor;
                InsObject.GetComponent<ParticleSystem>().Play();
                InsObject.GetComponent<BulletDestroy>().RestartTimer();
                //InsObject = Instantiate(ExplosionOnDeath, transform);
                InsObject.transform.SetParent(null);
                InsObject.transform.localEulerAngles = new Vector3(0, 0, 0);
            }

            if (key_drops)
            {
                GameObject item = Instantiate(key_item, transform);
                Transform grid = GameObject.Find("Grid").transform;

                for (int i = 0; i < grid.childCount; i++)
                {
                    if (grid.GetChild(i).gameObject.activeInHierarchy)
                    {
                        item.transform.SetParent(grid.GetChild(i));
                        item.transform.localEulerAngles = new Vector3(0, 0, 0);
                        item.transform.localScale = new Vector3(0.15f, 0.15f, 0.15f);
                        break;
                    }
                }
            }

            if (guaranteed_drop_bool)
            {
                GameObject item = Instantiate(guaranteed_drop_item, transform);
                Transform grid = GameObject.Find("Grid").transform;

                for (int i = 0; i < grid.childCount; i++)
                {
                    if (grid.GetChild(i).gameObject.activeInHierarchy)
                    {
                        item.transform.SetParent(grid.GetChild(i));
                        item.transform.localEulerAngles = new Vector3(0, 0, 0);
                        item.transform.localScale = new Vector3(1, 1, 1);
                        break;
                    }
                }
            }


            if (UpgradesManager.Instance.current_levels[3] != 0 && to_player && !Player.main.health.isMaxHealth)
            {
                Player.main.health.CurrentHealth += (int)UpgradesList.lifesteal.values[UpgradesManager.Instance.current_levels[3] - 1];
                GameplayUI.instance.GetHealthAnimator().Play("lifestealTick");
            }
            if (manager == null)
            {
                manager = GameObject.Find("Manager").GetComponent<GameManager>();
            }
            manager.player_kills++;
            ChallengeFields.UpdateKills(this);
            manager.ultra_kills++;

            if (in_fortress)
            {
                GameObject.FindGameObjectWithTag("fortress_rune").GetComponent<RuneFortressClass>().AddKill(this);
            }

            if (to_player)
            {
                Player.main.AddKill();
            }
        }
        catch{
            Debug.LogAssertion("Error within enemy death sequence... (Comment out try-catch with this comment in EnemyHealth script to find error)");
        }
        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "rune_fortress_field"){
            in_fortress = false;
        }
    }
}
