
using System.Collections.Generic;
using UnityEngine;

public class EnemyList : MonoBehaviour
{
    [SerializeField]
    private GameManager manager; 

    [SerializeField]
    private Spawn spawn; 

    [SerializeField]
    private float[] debut_dif, retire_dif, boss_dif, fire_boss_dif;
    [SerializeField]
    private GameObject[] enemy_types, mini_boss_types, fire_enemy_types, fire_mini_bosses;

    private bool[] has_appeared_prior, has_appeared_this_round, map_compatible;
    private bool[][] map_compatibility_tables;

    [SerializeField]
    private int[] mini_boss_allowed, boss_spawn_cooldown, boss_cooldown;
    public List<int> bossRounds;// {get; private set;}
    
    [SerializeField]
    private List<int> bossMaps;
    public List<int> BossMaps { get => bossMaps; private set => bossMaps = value; }

    [SerializeField]
    private int boss_round_counter = 11;
    public static EnemyList instance { get; private set; }

    void Awake(){

        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
        
        map_compatible = new bool[5]{
            false,
            false,
            false,
            false,
            false
        };


        has_appeared_this_round = new bool[5]{
            false,
            false,
            false,
            false,
            false
        };
        map_compatibility_tables = new bool[][]{

            new bool[] {false, false, false},
            new bool[] {false, false, false},
            new bool[] {false, false, false},
            new bool[] {true, true, true},

            new bool[] {true, true, true},
            new bool[] {true, true, true},
            new bool[] {true, true, true},
            new bool[] {true, true, true},

            new bool[] {true, true, true},
            new bool[] {true, true, true},
            new bool[] {true, false, false},
            new bool[] {true, true, true},

            new bool[] {true, true, true},
            new bool[] {true, false, true},
            new bool[] {true, true, true},
            new bool[] {true, false, false},
            
            new bool[] {true, true, true}, // the hub...
            new bool[] {true, true, true}, // runic
            new bool[] {true, true, true} // runic
        };

        bossRounds = new List<int>();
    }

    public void Restart(){
        bossRounds.Clear();
        OnStart();
    }

    public void OnStart(){
        boss_round_counter = 11;
        int starting_boss_round = 12;
        
        if(KillBox.currentGame.difficultyIndex == 0) {
            starting_boss_round = 14;
            bossRounds.Add(starting_boss_round);
            bossRounds.Add(21);
            bossRounds.Add(27);
            bossRounds.Add(35);
            for (int i = 42; i < 254; i += Random.Range(5, 8)){
                bossRounds.Add(i);
            }    
        }

        else if(KillBox.currentGame.difficultyIndex == 1) {
            starting_boss_round = 12;
            bossRounds.Add(starting_boss_round);
            bossRounds.Add(19);
            bossRounds.Add(24);
            bossRounds.Add(30);
            for (int i = 40; i < 150; i += Random.Range(5, 8)){
                bossRounds.Add(i);
            }    
        }

        else {starting_boss_round = 7;
            bossRounds.Add(starting_boss_round);
            bossRounds.Add(12);
            bossRounds.Add(16);
            bossRounds.Add(20);
            for (int i = 25; i < 150; i += Random.Range(5, 8)){
                bossRounds.Add(i);
            }    
        }

        
    }

    public void BossAppearanceCheck(int next_map){
        // boss_round_counter--;
        if (bossRounds.Contains(GameManager.main.LvlCount)){
            spawn.InitBossRound(true);
        }
        else{
            spawn.InitBossRound(false);
        }

        // map_compatible =  map_compatibility_tables[next_map];
    }

    public bool HasBoss(int round){
        return bossRounds.Contains(round);
    }


    public List<GameObject> UpdatedList
    {
        get
        {
            float difficulty = manager.Difficulty;
            List<GameObject> result = new List<GameObject>();

            if(GameManager.main.LvlCount == 1){
                result.Add(enemy_types[0]);
                return result; 
            }

            for(int i = 0; i < enemy_types.Length; i++)
            {
                if(debut_dif[i] <= difficulty && (retire_dif[i] >= difficulty || retire_dif[i] == -1))
                {
                    result.Add(enemy_types[i]);
                }
            }
            return result;
        }
    }

    [SerializeField]
    public List<GameObject> FireList
    {
        get
        {
            //float difficulty = manager.Difficulty;
            List<GameObject> result = new List<GameObject>();

            for(int i = 0; i < fire_enemy_types.Length; i++)
            {
                result.Add(fire_enemy_types[i]);
            }

            return result;
        }
    }

    public GameObject MiniFireBoss{
        get
        {
            Debug.Log("OH NO A BOSS AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");

            List<GameObject> valid_boses = new List<GameObject>();

            for(int i = 0; i < fire_mini_bosses.Length; i++){
                // if(manager.CurrentBossDif - fire_boss_dif[i] >= 0)
                //     valid_boses.Add(fire_mini_bosses[i]);
            }

            if(valid_boses.Count > 0) {

                int choice = Random.Range(0, valid_boses.Count);
                return valid_boses[choice];
            }

            else {return null;}
        }
    }
}
