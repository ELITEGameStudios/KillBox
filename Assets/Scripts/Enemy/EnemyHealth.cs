using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth, CurrentHealth, a;
    public GameManager manager;
    public GameObject guaranteedDrop;
    public Color explosionColor;
    public AudioSource audio;
    public AudioClip death, hit;
    public bool destructive_immune, no_drops, triggerDeathEvent;
    public bool guaranteedDropCondition => guaranteedDrop != null;
    public bool in_fortress {get; private set;}
    [SerializeField] private bool preventDefaultDeath, immortal, ignoresDamage;
    [SerializeField] private EnemyProfile profile; 
    public EnemyProfile hostProfile => profile; 
    [SerializeField] private UnityEvent onTakeDamage, onDie; 
    [SerializeField] private IDeathHandler deathHandler;
    public void SetDeathHandler(IDeathHandler deathHandler) {
        this.deathHandler = deathHandler;
    } 
    public void SetImmortal(bool immortal)
    {
        this.immortal = immortal;
    }


    void Awake(){
        if (profile != null) {maxHealth = profile.MaxHealth;}
    }

    
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = maxHealth;
        audio = gameObject.GetComponent<AudioSource>();
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
        if(CurrentHealth <= 0 && !immortal)
        {
            Die();
        }
    }

    public void TakeDmg(int Dmg)
    {
        if (ignoresDamage) return;
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
        if (profile != null)
        {
            if (profile.hasDrop || (KillBox.currentGame.round > 45 && a == 1))
            {
                if (!BossRoundManager.main.isBossRound && to_player)
                {
                    // Instantiates a token drop
                    
                    GameObject token = ObjectPoolManager.GetObjectFromPool("Token");
                    // token.transform.SetParent(null);
                    token.GetComponent<sine_movement>().ROOT = transform.position;
                    token.transform.position = transform.position;
                    // token.gameObject.SetActive(true);


                    Transform grid = GameObject.Find("Grid").transform;

                    for (int i = 0; i < grid.childCount; i++)
                    {
                        if (grid.GetChild(i).gameObject.activeInHierarchy)
                        {
                            token.transform.SetParent(grid.GetChild(i));
                            token.transform.localEulerAngles = new Vector3(0, 0, 0);
                            token.transform.position = transform.position;
                            token.transform.rotation = transform.rotation;
                            break;
                        }
                    }
                }
            }
        }

        try
        {
            GameObject explosionEffect = ObjectPoolManager.instance.InstantiateFromPool("Explosion", transform.position, transform.rotation);
            explosionEffect.GetComponent<ParticleSystem>().startColor = explosionColor;
            explosionEffect.GetComponent<ParticleSystem>().Play();
            explosionEffect.GetComponent<BulletDestroy>().RestartTimer();
            //InsObject = Instantiate(ExplosionOnDeath, transform);
            explosionEffect.transform.SetParent(null);
            explosionEffect.transform.localEulerAngles = new Vector3(0, 0, 0);

            if (guaranteedDropCondition)
            {
                GameObject item = Instantiate(guaranteedDrop, transform);
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
                Player.main.health.CurrentHealth += (int)UpgradesList.lifesteal.values[0][UpgradesManager.Instance.current_levels[3] - 1];
                GameplayUI.instance.GetHealthAnimator().Play("lifestealTick");
            }
            if (manager == null)
            {
                manager = GameObject.Find("Manager").GetComponent<GameManager>();
            }
            if (to_player)
            {
                manager.player_kills++;
                ChallengeFields.UpdateKills(this);
                manager.ultra_kills++;
             
                Player.main.AddKill();
            }
            if (in_fortress)
            {
                GameObject.FindGameObjectWithTag("fortress_rune").GetComponent<RuneFortressClass>().AddKill(this);
            }
        }
        catch{
            Debug.LogAssertion("Error within enemy death sequence... (Comment out try-catch with this comment in EnemyHealth script to find error)");
        }


        if(triggerDeathEvent){onDie.Invoke();}
        
        if (deathHandler != null)
        {
            deathHandler.OnDeath(to_player);
        }
        if(preventDefaultDeath) { return; }

        if(profile != null) profile.Retire();
        Destroy(gameObject);
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.tag == "rune_fortress_field"){
            in_fortress = false;
        }
    }
}
