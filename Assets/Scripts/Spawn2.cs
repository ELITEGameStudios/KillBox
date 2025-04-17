using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn2 : MonoBehaviour
{

    //public GameObject[] Entries, lvl5, lvl10, lvl15, lvl20, lvl25, lvl30, lvl35, lvl40, lvl45, lvl50, lvl55, lvl60, lvl65, lvl70, lvl75, lvl80, Enemies, Players;
    public GameObject[] Players;
    [SerializeField]
    private List<GameObject> Entries;
    private Spawn2 self;
    public GameObject Player, boss_entry;
    public int[] EntryManager;
    public int EnemyIndex, Round, Offset, Iterations, TargetLvl, unlock_on_level;
    public float DistanceFromPlayer;
    public GameManager gameManager;

    [SerializeField]
    private EnemyList enemyList;

    [SerializeField]
    private Spawn spawn_master;

    public bool ActiveSpawn, proximitySpawning, CanProxySpawn;

    void Awake()
    {
        enemyList = GameObject.Find("Manager").GetComponent<EnemyList>();
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
        
        try{
            spawn_master = GameManager.main.GetSpawn;
        }
        catch{
            Debug.Log("Caught");
        } 
        
        // if(spawn_master != null){
        //     spawn_master.AddSpawn(this);
        //     AutomationManager();
        //     Debug.Log("A");
        // }
    }

    void Start()
    {
        if(spawn_master == null){
            spawn_master = GameManager.main.GetSpawn;
            if(spawn_master != null){
                spawn_master.AddSpawn(this);
                Debug.Log("B");
            }
        }

        EntryManager[0] = 0;
        AutomationManager();
    }
    void Update()
    {
        Round = gameManager.LvlCount;
        Players = GameObject.FindGameObjectsWithTag("Player");

        //For Proximity Spawning only.
        if(proximitySpawning){
            DistanceFromPlayer = Vector3.Distance(Player.transform.position, transform.position);
            while(DistanceFromPlayer <= 10 && CanProxySpawn){
                CanProxySpawn = false;
                InstantSpawn();
                StartCoroutine(ProximitySpawn());
            }
        }

        if(Entries == null){
            Entries = GameManager.main.GetSpawn.Entries;
        }
    }
//
    public void AutomationManager()
    {
        Entries = GameManager.main.GetSpawn.Entries;
        // if(!gameManager.isFireRound){
        //     Entries = enemyList.UpdatedList;
        // }
        // else{
        //     Entries = enemyList.FireList;
        // }
    }

    public void InstantSpawn()
    {
        EnemyIndex = Random.Range(0, Entries.Count);

        if(ActiveSpawn)
        {
            bool spawned = false;
            int loopPrevention = 0;
            while (loopPrevention < Entries.Count){

                // Checks if there is or is not a limit to this enemies' spawning
                // Gets the profile of the enemy to spawn, if there is one
                EnemyProfile candaditeProfile = Entries[EnemyIndex].GetComponent<EnemyProfile>();
                
                if(candaditeProfile != null){
                    // Runs this block if there is a limit to the spawning of this enemy
                    if(candaditeProfile.Limit > 0){
                        // Changes the index if the amount of this enemy type has reached its limit 
                        int enemiesCount = EnemyCounter.main.GetEnemyCountOfName(candaditeProfile.EnemyName);
                        if(enemiesCount >= candaditeProfile.Limit){ 
                        
                            EnemyIndex++; 
                            if(EnemyIndex >= Entries.Count){EnemyIndex = 0;}
                            loopPrevention++;
                            continue;
                        }
                    }
                }

                Instantiate(Entries[EnemyIndex], transform.position, transform.rotation); 
                spawned = true;
                break;
            }
            if(!spawned){
                EnemyProfile candaditeProfile = Entries[EnemyIndex].GetComponent<EnemyProfile>();
                Instantiate(Entries[EnemyIndex], transform.position, transform.rotation); 
                spawned = true;
            }
        }
        
        EnemyIndex = 0;
    }

    public void MiniBossSpawn()
    {

        // GameObject boss = enemyList.RandomBoss;

        // if(gameManager.isFireRound){
        //     boss = enemyList.MiniFireBoss;
        // }

        // if(ActiveSpawn)
        // {
        //     if(boss != null){
        //         Instantiate(boss, transform.position, transform.rotation);
        //     }
        //     else{InstantSpawn();}
        // }
    }

    public void BossSpawn()
    {
        if(ActiveSpawn)
        {
            Instantiate(boss_entry, transform.position, transform.rotation);
        }
    }

    IEnumerator ProximitySpawn(){
        yield return new WaitForSeconds(9);
        CanProxySpawn = true;
    }
}