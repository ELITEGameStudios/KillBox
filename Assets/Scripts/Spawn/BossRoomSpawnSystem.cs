using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is responsible for
// Spawning enemies periodically into the scene
// Adjusting spawnrates and which enemies spawn given a difficulty level for the game
// Limits the enemies which can spawn into the scene at once based on the difficulty 

public class BossRoomSpawnSystem : MonoBehaviour
{
    // A list of positions of the spawns in the map
    [SerializeField] private List<Transform> active_spawns, shardMapSpawns, cutterMapSpawns, aTriadMapSpawns, guardianMapSpawns, bossRushMapSpawns;
    
    [SerializeField]
    private Transform mainBossSpawner;
    
    // The current list of enemy prefabs the spawner will spawn in the game
    [SerializeField]
    private List<GameObject> currentEnemyTable, currentBossTable;
   
    // The lists of enemy prefabs the spawner should spawn at set difficulties 
    [SerializeField]
    private List<GameObject> shardEnemyTable, cutterEnemyTable, alphaTriadEnemyTable, guardianEnemyTable, bossRushEnemyTable;
    
    [SerializeField]
    private List<GameObject> shardBossTable, cutterBossTable, alphaTriadBossTable, guardianBossTable, bossRushBossTable;
    
    // The time offset between periodic spawning and the max enemies allowed on the map
    [SerializeField]
    private float spawnRate;
    private int enemyCap, bossSpawnOffset, bossSpawnStep;
    public int spawnsAfterBoss {get; private set;}
    public int currentBossIndex {get; private set;}

    // The script for determining how many enemies are in the scene
    [SerializeField]
    private EnemyCounter enemyCounter;
    
    // The index of the current enemy list the spawner will pull from next
    [SerializeField]
    private int enemyIndex;
    public static BossRoomSpawnSystem main {get; private set;}
    public List<GameObject> CurrentBossTable { get => currentBossTable; }

    public bool isSpawning {get; private set;}

    // Called before the first frame of the game
    void Awake(){

        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        // Sets the initial settings for the spawner
        SetBossSpawnList(0);
        enemyIndex = 0;
    }




    // Adjusts the settings of the spawner given the difficulty of the game
    public void SetBossSpawnList(int difficulty){
        switch (difficulty){
            // Each case sets the corresponding enemy table depending on the difficulty
            // To the current enemy table
            // Also sets the unique spawnrate and enemy cap for each difficulty
            
            case 0:
                // Shard boss
                currentEnemyTable = shardEnemyTable;
                spawnRate = (BossRoundManager.main.bossRoundTier > 4 ? 0.5f : 1.25f) / GameManager.main.difficulty_coefficient;
                enemyCap = 9;
                bossSpawnOffset = BossRoundManager.main.bossRoundTier > 4 ? 2 : Random.Range(10, 20);
                bossSpawnStep = BossRoundManager.main.bossRoundTier > 4 ? 1 : 0;
                currentBossTable = shardBossTable;
                active_spawns = shardMapSpawns;
                
                break;
            
            case 1:
                // Cutter Boss
                currentEnemyTable = cutterEnemyTable;
                spawnRate = 0.85f;
                enemyCap = 25;
                bossSpawnOffset = BossRoundManager.main.bossRoundTier > 4 ? 3 : Random.Range(10, 21);
                bossSpawnStep = BossRoundManager.main.bossRoundTier > 4 ? 4 : 0;
                currentBossTable = cutterBossTable;
                active_spawns = cutterMapSpawns;
                break;
            
            case 2:
                // Alpha Triad Boss
                currentEnemyTable = alphaTriadEnemyTable;
                spawnRate = 0.7f;
                enemyCap = 30;
                bossSpawnOffset = BossRoundManager.main.bossRoundTier > 4 ? 5 : Random.Range(7, 16);
                bossSpawnStep = BossRoundManager.main.bossRoundTier > 4 ? 7 : 0;
                bossSpawnStep = Random.Range(10, 24);
                currentBossTable = alphaTriadBossTable;
                active_spawns = aTriadMapSpawns;
                break;
            
            case 3:
                // Guardian Boss
                currentEnemyTable = guardianEnemyTable;
                spawnRate = 0.85f;
                enemyCap = 4000;
                bossSpawnOffset = BossRoundManager.main.bossRoundTier > 4 ? 8 : Random.Range(12, 23);
                bossSpawnStep = BossRoundManager.main.bossRoundTier > 4 ? 9 : Random.Range(20, 40);
                currentBossTable = guardianBossTable;
                active_spawns = guardianMapSpawns;
                break;
            
            case 4:
                // Boss Rush
                spawnRate = 1f;
                currentEnemyTable = bossRushEnemyTable;
                enemyCap = 3000;
                bossSpawnOffset = BossRoundManager.main.bossRoundTier > 4 ? 1 : Random.Range(0, 5);
                bossSpawnStep = BossRoundManager.main.bossRoundTier > 4 ? 3 : Random.Range(5, 10);
                currentBossTable = bossRushBossTable;
                active_spawns = bossRushMapSpawns;
                break;
                
        }

        if(BossRoundManager.main.bossRoundTier > 4){
            List<GameObject> prestigeBossTable = new List<GameObject>();
            
            for(int i = 0; i < 8 * (int)(BossRoundManager.main.bossRoundTier / 5); i++ ){
                foreach (GameObject boss in currentBossTable){
                    prestigeBossTable.Add(boss);
                }
            }
            currentBossTable = prestigeBossTable;
            // bossSpawnOffset = (int)(BossRoundManager.main.bossRoundTier / 5) + 1;
            // bossSpawnStep = (int)(BossRoundManager.main.bossRoundTier / 5) + 1;
        }

        if(active_spawns.Count == 0){active_spawns = shardMapSpawns;}
    }

    // Starts the main spawning loop
    // Called by the game manager of the main game when the play button is pressed
    public void StartSpawnSequence()
    {
        // if(active_spawns.Count > 0) { active_spawns.Clear(); }
        // List<Spawn2> spawns = GameManager.main.GetSpawn.active_spawns;
        // foreach (Spawn2 spawn in spawns)
        // { active_spawns.Add(spawn.transform); }

        isSpawning = true;
        spawnsAfterBoss = 0;
        currentBossIndex = 0;
        StartCoroutine(spawning());
    }
    public void StopSpawning(){
        // StopCoroutine(spawning());
        enemyIndex = 0;
        isSpawning = false;
    }

    public void ResetSpawning(){
        // StopCoroutine(spawning());
        enemyIndex = 0;
        isSpawning = false;
        spawnsAfterBoss = 0;
        SetBossSpawnList(0);
        currentBossTable = shardBossTable;
    }

    // The main spawning coroutine the script operates on
    // Using this as a coroutine allows accurate time offsets and skips frames when not needed
    // The only place using a while loop is viable in unity, since otherwise the usage  
    // Of while loops would freeze the game! 
    IEnumerator spawning()
    {
        int currentStep = bossSpawnStep;
        int currentOffset = bossSpawnOffset;
        currentBossIndex = 0;
        bool spawnedOnce = false;

        while(true) {

            // Iterates through each active spawnpoint
            for (int i = 0; i < active_spawns.Count; i++)
            {

                if(!isSpawning){break;}
                
                if(enemyCounter.enemiesInScene < enemyCap)
                {

                    /* Normal Enemy Spawning */
                    // Waits for the given spawnrate time and spawns an enemy
                    // at the position and rotation of the spawn used 
                    yield return new WaitForSeconds(spawnRate);
                    Instantiate(currentEnemyTable[enemyIndex], active_spawns[i].position, active_spawns[i].rotation);
                    // Updates the index of the list to use, or sets it to  
                    // Zero if the index goes beyond the variable table length 
                    enemyIndex++;
                    if(enemyIndex == currentEnemyTable.Count){enemyIndex = 0;}


                    /* Boss Spawning */
                    // For the first boss spawn
                    if(!spawnedOnce){
                        if(currentOffset == 0){
                            Instantiate(currentBossTable[currentBossIndex], mainBossSpawner.position, mainBossSpawner.rotation);
                            spawnedOnce = true;
                            spawnsAfterBoss = 0;
                            currentBossIndex++;
                            KillboxEventSystem.TriggerBossSpawnEvent();
                        }
                        else{ currentOffset--; }
                    }

                    else if(currentBossIndex < currentBossTable.Count){
                        if(currentStep == 0){
                            Instantiate(currentBossTable[currentBossIndex], mainBossSpawner.position, mainBossSpawner.rotation);
                            currentStep = bossSpawnOffset;
                            spawnsAfterBoss = 0;
                            currentBossIndex++;
                            KillboxEventSystem.TriggerBossSpawnEvent();
                        }
                        else{currentStep--;}
                    }
                    else{
                        spawnsAfterBoss ++;
                    }

                }

                // Pauses execution until the next runtime frame  
                // If the enemies are capped 
                yield return null;
            }
            
            if(!isSpawning){break;}
        }
    }
}