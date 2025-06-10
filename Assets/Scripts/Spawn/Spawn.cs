using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public bool allow, instant, boss_round, boss_allowed, enemiesReachedCap, ended;
    public GameManager gameManager;
    public List <Spawn2> spawns {get; private set;}
    public List<Spawn2> active_spawns;
    public float[] SpawnTimeStart, Step, StartStep;
    public float InstancesFloat, stepRate, SpawnRound, InstancesRound, SRTS, IRTS, EndRate, SVTime, spawnTime, spawnTimeConstant;
    public int instances, InstancesCap, boss_chance, guaranteed_boss_instance;

    public int[] enemyDropIndexes; // list of instances with drops
    [SerializeField] private EnemyCounter enemyCounter;
    [SerializeField] public List<GameObject> Entries {get; private set;}
    [SerializeField] private EnemyList enemyList;


    void Awake(){
        spawns = new List<Spawn2>();
        // init();
    }

    void Start(){
        GenerateNewEnemyPool();
        // UpdateRoundBasedVars();
    }

    //int GetFireInstances(){
    //    return (int)();
    //}

    public void OnFireRound(){
        instances = (int) Mathf.Pow(1.1f, gameManager.LvlCount);
    }


    public void init()
    {
        GenerateNewEnemyPool();
        // UpdateRoundBasedVars();
        // SetVars = true;
        // StartCoroutine(SVarTimer());
    }

    public void GenerateDrops()
    {
        
        int dropsThisRound = EconomyManager.instance.tokensThisRound;
        if (dropsThisRound == -1) return;

        // list of instances with drops
        enemyDropIndexes = new int[dropsThisRound];
        
        // list from 1 to (instances)
        List<int> indexDraws = new();
        for (int i = 1; i < instances; i++) { indexDraws.Add(i); } 

        for (int i = 0; i < enemyDropIndexes.Length && i < instances-1; i++)
        {
            // this minisystem is intended to add random draws for which instances of enemies drop tokens, without adding duplicate draws

            int newIndex = Random.Range(0, indexDraws.Count); // retrieves random index from indexdraws
            enemyDropIndexes[i] = indexDraws[newIndex]; // adds the number indexDraws has at that random index
            indexDraws.RemoveAt(newIndex); // removes that indexDraws number from the indexDraws pool
        }
    }

    public void Refresh()
    {
        ended = false;
    }

    public void UpdateRoundBasedVars()
    {

        if (gameManager.LvlCount != 1) { instances = (int)((2.5 * Mathf.Sqrt(GameManager.main.Difficulty)) - 16f); }
        spawnTime = (float)(1 / KillBox.currentGame.difficultyCoefficient) * Mathf.Pow(1.3f, (-KillBox.currentGame.round / 1.5f) + 4) + spawnTimeConstant;
        ended = false;

        GenerateDrops();

        if (BossRoundManager.main.isBossRound) { RoundCompositionManager.main.AvoidComposition(); }
        else { RoundCompositionManager.main.ChangeComposition(); }
    }

    public void GenerateNewEnemyPool()
    {
        Entries = enemyList.UpdatedList;
        
        if(RoundCompositionManager.main.hasComposition){
            List<GameObject> filteredEntries = new List<GameObject>();

            for(int i = 0; i < Entries.Count; i++){
                EnemyProfile profile = Entries[i].GetComponent<EnemyProfile>();
                if(profile != null){
                    if(RoundCompositionManager.main.current.tags.Contains<string>(profile.EnemyName)){ filteredEntries.Add(Entries[i]); }
                }
            }
            Entries = filteredEntries;
        }


        foreach (Spawn2 spawn in spawns)
        {
            spawn.RefreshEntries();
        }
    }

    public void InitBossRound(bool is_round){
        boss_round = is_round;
    }

    public void StartSpawnSequence()
    {
        UpdateRoundBasedVars();
        allow = true;

        StartCoroutine(spawning());
        if (instant)
        {
            for (int i = 0; i < spawns.Count; i++){
                if(spawns[i].proximitySpawning != true){
                    spawns[i].InstantSpawn();
                    instances--;
                }
            }
            
        }
    }

    public void AddSpawn(Spawn2 _object){
        spawns.Add(_object);
    }

    public void SetSpawns(Spawn2[] spawns){
        this.spawns = spawns.ToList();
        foreach (Spawn2 spawner in this.spawns){
            spawner.RefreshEntries();
        }
    }

    public void CancelAllSpawns(){
        instances = 0;
        ended = true;
        allow = false;
    }

    public void SuspendSpawns(){
        instances = 0;
        ended = true;
        allow = false;
    }

    public void ResumeSpawns(){
        instances = 0;
        ended = true;
        allow = false;
    }

    void ReIterate()
    {
        if(allow)
        {
            StopCoroutine(spawning());
            StartCoroutine(spawning());
        }
    }

    IEnumerator spawning()
    {

        if(instances == 0){
            instances = 1;
        }
        
        // Spawns enemies until there are no more instances
        while (instances != 0)
        {
            // Checks if enemies are able to spawn (crowd control)
            if (enemyCounter.enemiesInScene < 35 && allow)
            {

                // iterates between each spawner
                for (int ii = 0; ii < spawns.Count; ii++)
                {
                    // Waits the spawn time
                    yield return new WaitForSeconds(spawnTime);

                    if (instances <= 0 || ended){
                        // Detects if there are no more instances
                        break;
                    }

                    // Spawns the next enemy if this is not a normal spawner
                    if (spawns[ii].proximitySpawning != true)
                    {
                        bool hasDrop = false;
                        foreach (int item in enemyDropIndexes)
                        {
                            if (item == instances) { hasDrop = true; break; }
                        }    
                        spawns[ii].InstantSpawn(hasDrop);

                        instances--;
                    }

                    continue; //ReIterate();
                }

                if (ended)
                {
                    break;
                }

                yield return null;
                Debug.Log("Caught ya!");
            }
            else
            {
                yield return null;
                continue; //ReIterate();
            }
        }

        ended = true;
        allow = false;
        StopCoroutine(spawning());
    
    }

}