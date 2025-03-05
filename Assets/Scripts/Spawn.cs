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

    [SerializeField] private EnemyCounter enemyCounter;
    [SerializeField] public List<GameObject> Entries {get; private set;}
    [SerializeField] private EnemyList enemyList;


    void Awake(){
        spawns = new List<Spawn2>();
        // init();
    }

    void Start(){
        GenerateNewEnemyPool();
        ResetVars();
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
        ResetVars();
        // SetVars = true;
        // StartCoroutine(SVarTimer());
    }

    void Update()
    {
        // if(SetVars)
        // {

        //     // OLD spawnTime = 1f + ((-gameManager.Difficulty / (float)Mathf.Pow(80, 2)) * (gameManager.LvlCount));
        //     spawnTime = (float) (1/gameManager.difficulty_coefficient)*Mathf.Pow(1.3f, (-gameManager.LvlCount / 1.5f) + 4) + spawnTimeConstant;

        //     //if(spawnTime < 0.3f)
        //     //{
        //     //    spawnTime = 0.3f;
        //     //}
        // }
    }

    public void ResetVars()
    {
        
        
        if( gameManager.LvlCount != 1){ instances = (int)((2.5 * Mathf.Sqrt(gameManager.Difficulty)) - 16f); }
        spawnTime = (float) (1/gameManager.difficulty_coefficient)*Mathf.Pow(1.3f, (-gameManager.LvlCount / 1.5f) + 4) + spawnTimeConstant;
        ended = false;

        active_spawns.Clear();

        for (int i = 0; i < spawns.Count; i++) {
            // if (spawns[i].ActiveSpawn)
            if (spawns[i].ActiveSpawn && spawns[i].gameObject.activeInHierarchy)
            { active_spawns.Add(spawns[i]); }
        }
    }

    public void GenerateNewEnemyPool()
    {

        // if(!GameManager.main.isFireRound){ Entries = enemyList.UpdatedList; }
        // else{ Entries = enemyList.FireList; }
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

        foreach (Spawn2 spawn in spawns){
            spawn.AutomationManager();
        }
    }

    public void InitBossRound(bool is_round){
        boss_round = is_round;
    }

    public void StartSpawnSequence()
    {
        // active_spawns = new List<Spawn2>();

        ResetVars();
        allow = true;
        
        // if(boss_round){
        //     guaranteed_boss_instance = Random.Range(4, instances);    
        // }

        StartCoroutine(spawning());
        if (instant)
        {
            for (int i = 0; i < active_spawns.Count; i++){
                if(active_spawns[i].proximitySpawning != true){
                    active_spawns[i].InstantSpawn();
                    instances--;
                }
            }
            
        }
    }

    public void AddSpawn(Spawn2 _object){
        spawns.Add(_object);
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
        // if(gameManager.LvlCount >= 26){
        //     boss_chance = 50;
        // }
        // else if(gameManager.LvlCount >= 45){
        //     boss_chance = 25;
        // }
        // else{
        //     boss_chance = 75;
        // }

        if(instances == 0){
            instances = 1;
        }
        while(instances != 0)
        {
            if(enemyCounter.enemiesInScene < 35 && allow)
            {

                for (int ii = 0; ii < active_spawns.Count; ii++)
                {
                    yield return new WaitForSeconds(spawnTime);

                    if (instances <= 0 || ended)
                    {
                        break;
                    }

                    // if(guaranteed_boss_instance == instances && boss_round){
                    //     active_spawns[ii].MiniBossSpawn();
                    //     instances--;
                    //     continue;
                    // }

                    if (active_spawns[ii].proximitySpawning != true)
                    {
                        // int chance = Random.Range(0, boss_chance);

                        // if(chance == 1 && boss_round){//> gameManager.BossRoundStart){

                        //     active_spawns[ii].MiniBossSpawn();
                        // }
                        // else{
                        // }
                        active_spawns[ii].InstantSpawn();

                        instances--;
                    }

                    continue; //ReIterate();
                }

                if(ended){
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