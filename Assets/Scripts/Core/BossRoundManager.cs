using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Game;

public class BossRoundManager : MonoBehaviour, IRestartListener
{



    public static BossRoundManager main {get; private set;}

    public BossRoomSpawnSystem spawnSystem {get; private set;}
    public bool isBossRound {get; private set;}
    public bool finishedBossRoundMainPhase {get; private set;}
    public int timeUntilNextBoss{get; private set;}
    public int roundsBeyondLastBoss => 
        GetPreviousBossRoundEntry() == null ? 
        999 : 
        GameManager.main.LvlCount - GameManager.main.GetLastRoundInPhase(((BossRoundEntry)GetPreviousBossRoundEntry()).phase);

    public BossType bossType;
    [SerializeField] private EnemyList enemyList;

    public List<BossRoundEntry> activeBossRoundEntries;

    public List<BossRoundEntry> easyBossRoundEntries;
    public List<BossRoundEntry> standardBossRoundEntries;
    public List<BossRoundEntry> extremeBossRoundEntries;


    
    public enum BossType 
    {
        SHARD,
        CUTTER,
        ALPHATRIAD,
        MIDAS,
        DUSK,
        DAWN,
        PROLOGUE,
        GUARDIANS,
        BOSSRUSH,
        LOOPY
    }

    [System.Serializable]
    public struct BossRoundEntry
    {
        public BossType bossType;
        public int phase;
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

    public void InitializeEntries()
    {
        switch (KillBox.currentGame.difficulty)
        {
            case Difficulty.EASY:
                activeBossRoundEntries = easyBossRoundEntries;
                break;
            
            case Difficulty.STANDARD:
                activeBossRoundEntries = standardBossRoundEntries;
                break;

            case Difficulty.EXTREME:
                activeBossRoundEntries = extremeBossRoundEntries;
                break;
        }
    }

    void Start(){
        spawnSystem = BossRoomSpawnSystem.main;
    }

    public void EndBossRound()
    {
        isBossRound = false;
        MainAudioSystem.main.PlayMainLoop();
        UpdateCounters();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log("Count of bosses: " + EnemyCounter.main.bossProfiles.Count);
        // Debug.Log("Bosses to spawn: " + spawnSystem.CurrentBossTable.Count);
        // if(spawnSystem.isSpawning){ Debug.Log("the spawn system is in fact spawning.");}
        // if(LvlStarter.main.HasStarted){ Debug.Log("the level has in fact started.");}
        // if(finishedBossRoundMainPhase){ Debug.Log("the main phase has in fact finished.");}
        if (isBossRound)
        {
            // Detects if all bosses have died
            if (
                EnemyCounter.main.bossProfiles.Count == 0 &&
                LvlStarter.main.HasStarted &&
                spawnSystem.currentBossIndex == spawnSystem.CurrentBossTable.Count 
                && spawnSystem.bossHasSpawned
                && (spawnSystem.stopAfterSpawnBosses ? true : spawnSystem.spawnsAfterBoss >= 10)
            )
            {
                spawnSystem.StopSpawning();
                finishedBossRoundMainPhase = true;
                
                if (GameManager.main.GetType() == typeof(BossChallengeGameManager))
                {
                    BossChallengeGameManager manager = GameManager.main as BossChallengeGameManager;
                    manager.UpdateCurrentBoss();
                }

                // Giving bonus
                // int bonus = EconomyManager.instance.GetBossBonus();
                // GameManager.main.OnPickupToken(bonus, false);
                // BonusesUIManager.instance.ActivateBonus(bossType.ToString().ToLower(), bonus, 10);

            }

            // if(finishedBossRoundMainPhase)
            // {

            // }
        }
    }
    // public int GetTierOfRound(int round){
    //     return KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE
    //     ? GameManager.main.LvlCount
    //     : enemyList.bossRounds.FindIndex(match => match == GameManager.main.LvlCount);
    // }

    public void SetupBossRound(BossType _bossType){
        
        isBossRound = true;
        finishedBossRoundMainPhase = false;

        // bossRoundTier = (int)_bossType;
        bossType = _bossType; // Must fix implementation with old boss implementation. Bug exists because of the spawn rule below
        spawnSystem.ResetSpawning(false);
        spawnSystem.SetBossSpawnList(bossType);
        KillboxEventSystem.TriggerBossRoundChangeEvent();

        // if (KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE)
        // {
        //     finishedBossRoundMainPhase = false;
        //     bossRoundTier = (int)(GameManager.main as BossChallengeGameManager).currentBoss;
        //     bossType = (GameManager.main as BossChallengeGameManager).currentBoss; // Must fix implementation with old boss implementation. Bug exists because of the spawn rule below
        //     spawnSystem.SetBossSpawnList(bossType);
        //     KillboxEventSystem.TriggerBossRoundChangeEvent();         
    }

    public BossRoundEntry? GetNextBossRoundEntry()
    {
        for (int i = 0; i < activeBossRoundEntries.Count; i++)
        {
            if(activeBossRoundEntries[i].phase < GameManager.main.GetPhase()){ continue; }
            return activeBossRoundEntries[i];
        }

        return null;
    }

    public BossRoundEntry? GetPreviousBossRoundEntry()
    {
        for (int i = activeBossRoundEntries.Count-1 ; i >= 0; i--)
        {
            if(activeBossRoundEntries[i].phase >= GameManager.main.GetPhase()){ continue; }
            return activeBossRoundEntries[i];
        }

        return null;
    }

    public void UpdateCounters(){

        if (KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE)
        {
            timeUntilNextBoss = 0;
            return;
        }

        // Updating when the next boss round will happen
        int round = GameManager.main.LvlCount;
        int targetRound;
        
        if(GetNextBossRoundEntry() == null){ timeUntilNextBoss = 999; } // Must be finished later, will only occur when there are no set bosses left
        else{
            targetRound = GameManager.main.GetLastRoundInPhase( ((BossRoundEntry)GetNextBossRoundEntry()).phase );
            timeUntilNextBoss = targetRound - GameManager.main.LvlCount;            
        }

        BossRoundCounterUI.main.UpdateDisplay(roundsBeyondLastBoss <= 0);
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
