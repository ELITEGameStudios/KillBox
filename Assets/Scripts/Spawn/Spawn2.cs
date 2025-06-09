using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn2 : MonoBehaviour
{

    //public GameObject[] Entries, lvl5, lvl10, lvl15, lvl20, lvl25, lvl30, lvl35, lvl40, lvl45, lvl50, lvl55, lvl60, lvl65, lvl70, lvl75, lvl80, Enemies, Players;
    public GameObject[] players;
    [SerializeField]
    private List<GameObject> Entries;
    private Spawn2 self;
    public GameObject Player, boss_entry;
    public int[] EntryManager;
    public int enemyIndex, unlock_on_level;
    public float DistanceFromPlayer;
    public GameManager gameManager;


    public bool proximitySpawning, CanProxySpawn;

    void Awake()
    {
        gameManager = GameObject.Find("Manager").GetComponent<GameManager>();
    }

    void Start()
    {
        EntryManager[0] = 0;
        RefreshEntries();
    }
    
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

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

    public void RefreshEntries() { Entries = GameManager.main.GetSpawn.Entries; }

    public void InstantSpawn(bool withDrop = false)
    {
        enemyIndex = Random.Range(0, Entries.Count);
        bool spawned = false;
        int loopPrevention = 0;
        while (loopPrevention < Entries.Count){

            // Checks if there is or is not a limit to this enemies' spawning
            // Gets the profile of the enemy to spawn, if there is one
            EnemyProfile candaditeProfile = Entries[enemyIndex].GetComponent<EnemyProfile>();
            
            if(candaditeProfile != null){
                // Runs this block if there is a limit to the spawning of this enemy
                if(candaditeProfile.Limit > 0){
                    // Changes the index if the amount of this enemy type has reached its limit 
                    int enemiesCount = EnemyCounter.main.GetEnemyCountOfName(candaditeProfile.EnemyName);
                    if(enemiesCount >= candaditeProfile.Limit){ 
                    
                        enemyIndex++; 
                        if(enemyIndex >= Entries.Count){enemyIndex = 0;}
                        loopPrevention++;
                        continue;
                    }
                }
            }
            FinalizeSpawnEntry(withDrop);

            spawned = true;
            break;
        }
        if(!spawned){
            EnemyProfile candaditeProfile = Entries[enemyIndex].GetComponent<EnemyProfile>();
            FinalizeSpawnEntry(withDrop);

            spawned = true;
        }
        
        enemyIndex = 0;
    }

    void FinalizeSpawnEntry(bool hasDrop = false)
    {
        GameObject newEnemy = Instantiate(Entries[enemyIndex], transform.position, transform.rotation);
        if (hasDrop)
        {
            EnemyProfile profile = newEnemy.GetComponent<EnemyProfile>();
            if (profile != null){
                profile.AddDrop();
            }
        }
        
    }
    public void BossSpawn(){
            Instantiate(boss_entry, transform.position, transform.rotation);
    }

    IEnumerator ProximitySpawn(){
        yield return new WaitForSeconds(9);
        CanProxySpawn = true;
    }
}