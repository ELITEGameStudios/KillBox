using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    Spawn spawn;
    GameManager manager;
    challengeClass challenge_class;

    [SerializeField]
    private PlayerHealth player_hp;

    public int constant {get; private set;}
    public int defaultHealth {get; private set;}

    public ChestSystemManager chest_system;

    public static DifficultyManager main;

    public int index {get; private set;}
    void Start(){

        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        spawn = gameObject.GetComponent<Spawn>();
        manager = gameObject.GetComponent<GameManager>();
        challenge_class = gameObject.GetComponent<challengeClass>();
        index = 1;
    }

    //0 = easy, 1 = medium, 2 = hard

    public void SetDifficulty(int input){
        index = input;

        if(index == 0){
            Easy();
        }

        if(index == 1){
            Standard();
        }

        if(index == 2){
            Hard();
        }
    }

    void Easy(){
        spawn.Step[1] = 0.2f;
        spawn.boss_allowed = false;
        spawn.SpawnTimeStart[0] = 4;
        spawn.SpawnTimeStart[1] = 2;
        spawn.InstancesCap = 13;
        // manager.difficulty_coefficient = 0.65f;
        manager.BossRoundStart = 35;
        manager.constant = 51;

        defaultHealth = 250;

        player_hp.SetMaxHealth(250, "JWBVIHEWBCV*&T^&237236fg3gv38fvr3v3v6)*&", true);
        spawn.instances = 1;
        spawn.spawnTimeConstant = 0.55f;

        
        chest_system.RefreshChests();
        //spawn.init();
    }

    void Standard(){
        spawn.Step[1] = 0.2f;
        spawn.boss_allowed = false;
        spawn.SpawnTimeStart[0] = 4;
        spawn.SpawnTimeStart[1] = 1.5f;
        spawn.InstancesCap = 13;
        // manager.difficulty_coefficient = 1f;
        manager.constant = 51;

        manager.BossRoundStart = 2;

        defaultHealth = 250;
        player_hp.SetMaxHealth(250, "JWBVIHEWBCV*&T^&237236fg3gv38fvr3v3v6)*&", true);
        spawn.spawnTimeConstant = 0.1f;
        spawn.instances = 2;

        chest_system.RefreshChests();
        //spawn.init();
    }

    void Hard(){
        spawn.Step[1] = 0.2f;
        spawn.boss_allowed = false;
        spawn.SpawnTimeStart[0] = 4;
        spawn.SpawnTimeStart[1] = 2;
        spawn.InstancesCap = 13;
        // manager.difficulty_coefficient = 2f;
        manager.constant = 150;

        manager.BossRoundStart = 12;

        defaultHealth = 150;
        spawn.spawnTimeConstant = 0.05f;
        player_hp.SetMaxHealth(150, "JWBVIHEWBCV*&T^&237236fg3gv38fvr3v3v6)*&", true);
        spawn.instances = 2;
        
        chest_system.RefreshChests();
        //spawn.init();
    }
}
