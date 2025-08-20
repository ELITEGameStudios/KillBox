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
    public BossType bossType;

    [SerializeField] private EnemyList enemyList;


    
    public enum BossType 
    {
        SHARD,
        CUTTER,
        ALPHATRIAD,
        MIDAS,
        TWINS,
        PROLOGUE,
        GUARDIANS,
        BOSSRUSH,
        LOOPY
    }

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

        // Detects if all bosses have died
        if (
            EnemyCounter.main.bossProfiles.Count == 0 &&
            LvlStarter.main.HasStarted &&
            spawnSystem.currentBossIndex == spawnSystem.CurrentBossTable.Count &&
            spawnSystem.isSpawning &&
            spawnSystem.spawnsAfterBoss >= 10)

        {
            spawnSystem.StopSpawning();
            finishedBossRoundMainPhase = true;

            if (GameManager.main.GetType() == typeof(BossChallengeGameManager))
            {
                BossChallengeGameManager manager = GameManager.main as BossChallengeGameManager;
                manager.UpdateCurrentBoss();
            }

            // Giving bonus
            int bonus = EconomyManager.instance.GetBossBonus();
            GameManager.main.OnPickupToken(bonus, false);
            BonusesUIManager.instance.ActivateBonus(bossType.ToString().ToLower(), bonus, 10);
        }
    }
    // public int GetTierOfRound(int round){
    //     return KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE
    //     ? GameManager.main.LvlCount
    //     : enemyList.bossRounds.FindIndex(match => match == GameManager.main.LvlCount);
    // }

    public void SetBossRound(bool hasBoss, BossType? _bossType = null){
        isBossRound = hasBoss;
        if (KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE)
        {
            finishedBossRoundMainPhase = false;

            bossRoundTier = (int)(GameManager.main as BossChallengeGameManager).currentBoss;
            bossType = (GameManager.main as BossChallengeGameManager).currentBoss; // Must fix implementation with old boss implementation. Bug exists because of the spawn rule below
            spawnSystem.SetBossSpawnList(bossType);
            
            KillboxEventSystem.TriggerBossRoundChangeEvent();

            return;

        }
        if(isBossRound){ 
            finishedBossRoundMainPhase = false;
            if (_bossType == null)
            {
                int forcedBossRoundIndex = enemyList.bossRounds.FindIndex(match => match == GameManager.main.LvlCount) % 4;
                switch (forcedBossRoundIndex)
                {
                    case 2: bossType = BossType.GUARDIANS; break;
                    case 3: bossType = BossType.LOOPY; break;
                    default:
                        bossType = (BossType)(forcedBossRoundIndex);
                        break; 
                }
                bossType = (BossType)bossRoundTier;
            }
            else
            {
                bossRoundTier = (int)_bossType;
                bossType = (BossType)_bossType; // Must fix implementation with old boss implementation. Bug exists because of the spawn rule below
            }


            spawnSystem.SetBossSpawnList(bossType);
            KillboxEventSystem.TriggerBossRoundChangeEvent();
        }
        else{ finishedBossRoundMainPhase = true; }
    }

    public void UpdateCounters(){

        if (KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE)
        {
            timeUntilNextBoss = 0;
            timeSinceLastBoss = 0;
            // finishedBossRoundMainPhase = false; ;
            return;
        }

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
