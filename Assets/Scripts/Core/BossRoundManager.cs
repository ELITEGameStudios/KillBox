using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoundManager : MonoBehaviour, IRestartListener
{



    public static BossRoundManager main {get; private set;}

    public BossRoomSpawnSystem spawnSystem {get; private set;}
    public bool isBossRound {get; private set;}
    public bool finishedBossRoundMainPhase {get; private set;}
    public int bossRoundTier {get; private set;}
    public int timeUntilNextBoss{get; private set;}
    public int timeSinceLastBoss{get; private set;}

    [SerializeField] private EnemyList enemyList;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }
    }

    void Start(){
        spawnSystem = BossRoomSpawnSystem.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Count of bosses: " + EnemyCounter.main.bossProfiles.Count);
        // Debug.Log("Bosses to spawn: " + spawnSystem.CurrentBossTable.Count);
        // if(spawnSystem.isSpawning){ Debug.Log("the spawn system is in fact spawning.");}
        // if(LvlStarter.main.HasStarted){ Debug.Log("the level has in fact started.");}
        // if(finishedBossRoundMainPhase){ Debug.Log("the main phase has in fact finished.");}


        if(
            EnemyCounter.main.bossProfiles.Count == 0 &&
            LvlStarter.main.HasStarted &&
            spawnSystem.currentBossIndex ==  spawnSystem.CurrentBossTable.Count &&
            spawnSystem.isSpawning &&
            spawnSystem.spawnsAfterBoss >= 10)

        { spawnSystem.StopSpawning(); finishedBossRoundMainPhase = true; }
    }
    public int GetTierOfRound(int round){
        return enemyList.bossRounds.FindIndex(match => match == round);
    }

    public void SetBossRound(bool hasBoss){
        isBossRound = hasBoss;

        if(isBossRound){ 
            finishedBossRoundMainPhase = false;

            bossRoundTier = enemyList.bossRounds.FindIndex(match => match == GameManager.main.LvlCount);
            spawnSystem.SetBossSpawnList(bossRoundTier % 5);
            KillboxEventSystem.TriggerBossRoundChangeEvent();
        }
        else{ finishedBossRoundMainPhase = true; }
    }

    public void UpdateCounters(){
        int round = GameManager.main.LvlCount;
        int targetRound = GameManager.main.LvlCount;

        while(!enemyList.HasBoss(targetRound)){
            targetRound++;
            if(targetRound > 150){
                targetRound = -1;
                break;
            }
        }

        // for (int i = 0; i < enemyList.bossRounds.Count && (i > 0 ? enemyList.bossRounds[i - 1] : 0) < GameManager.main.LvlCount; i++){
        //     if(enemyList.bossRounds[i] >= GameManager.main.LvlCount){
        //         targetRound = enemyList.bossRounds[i];
        //         Debug.Log(targetRound + "target");
        //         Debug.Log(enemyList.bossRounds[i] + "current round");
        //         break;
        //     }
        // }

        timeUntilNextBoss = targetRound - GameManager.main.LvlCount;
        Debug.Log(timeUntilNextBoss + "boss time");

        targetRound = GameManager.main.LvlCount;
        
        while(!enemyList.HasBoss(targetRound)){
            targetRound--;
            if(targetRound < 0){
                targetRound = -1;
                break;
            }
        }
        
        // foreach(int rounds in reversedRounds){
        //     if(rounds <= GameManager.main.LvlCount){
        //         targetRound = rounds;
        //         break;
        //     }
        // }

        timeSinceLastBoss = GameManager.main.LvlCount - targetRound;

        if(timeSinceLastBoss > 0){
            BossRoundCounterUI.main.UpdateDisplay(false);
        }
        else{
            BossRoundCounterUI.main.UpdateDisplay(true);
        }
    }

    public void OnRestartGame()
    {
        if(isBossRound){
            isBossRound = false;
            spawnSystem.ResetSpawning(); 
            finishedBossRoundMainPhase = true;
        }
        
        MainAudioSystem.main.PlayMainLoop();
        VolumeControl.main.SetSilentSnapshot(false, 2);
    }
}
