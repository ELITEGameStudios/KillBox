using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ProgressionSystem : MonoBehaviour, IMainRoundEventListener, IPurchaseEventListener, IBossRoundEventListener
{
    public static int playerXp {get; private set;}
    public static int nextLevelXp {get; private set;}
    public static int currentLevelXp {get; private set;}
    public static bool initialized {get; private set;}
    public static int playerLevel{get; private set;}
    private static int[] levelOffsets = new int[5]{1000, 2500, 5000, 10000, 20000};
    private static int[] coreLevels = new int[6]{5, 10, 15, 20, 25, 30};
    
    [SerializeField] private GameObject[] unlockPrompts;
    [SerializeField] private GameObject[] unlockButtons;
    [SerializeField] private UnityEvent[] unlockEvent;
    [SerializeField] private ProgressionDisplay[] displays;
    [SerializeField] private ProgressionDisplay inGameDisplay;
    public static ProgressionSystem main {get; private set;}


    // Update is called once per frame
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        if(!initialized){
            if(SaveSystem.LoadPlayer() != null){
                PlayerData data = SaveSystem.LoadPlayer();
                playerXp = data.playerXP;

            }
            else{
                playerXp = 0;
            }

            initialized = true;
            CalculateLevel();
            SetAllDisplays();
            SyncRewards();
            Debug.Log("The player's XP has been initialized. XP: " + playerXp + ", LEVEL: " + playerLevel);
        }
        else{

            CalculateLevel();
            SetAllDisplays();
            SyncRewards();
            
            Debug.Log("The player's XP session has been restored. XP: " + playerXp + ", LEVEL: " + playerLevel);

        }
    }

    void Update(){
        // Debug.Log("Player's current xp = " + playerXp);
    }

    public void InitializeXP(int xp, GameManager manager){
        if(manager == GameManager.main){
            if(!initialized){
                playerXp = xp;
                initialized = true;
                CalculateLevel();
                SetAllDisplays();
                SyncRewards();
                
                Debug.Log("The player's XP has been initialized. XP: " + playerXp + ", LEVEL: " + playerLevel);
            }
            else{

                CalculateLevel();
                SetAllDisplays();
                SyncRewards();
                
                Debug.Log("The player's XP session has been restored. XP: " + playerXp + ", LEVEL: " + playerLevel);
            }

        }
    }

    void AddXp(int xp){
        if(!GameManager.main.freeplay){
            playerXp += xp;
            KillboxEventSystem.TriggerGainXpEvent(xp);
            if(playerXp >= nextLevelXp){
                CalculateLevel();
                KillboxEventSystem.TriggerLevelUpEvent();
            }

            // if(inGameDisplay != null){
            //     inGameDisplay.SetDisplays();
            // }

            SetAllDisplays();
            Debug.Log("Added "+ xp + " to the Player's current xp... " + playerXp);
            Debug.Log("Next XP level is " + nextLevelXp + " And the player is currently level " + playerLevel);
        }
    }

    void CalculateLevel(){
        if(playerXp < 5000){

            playerLevel = playerXp / levelOffsets[0]; 
            currentLevelXp = levelOffsets[0] * playerLevel; 
            nextLevelXp = currentLevelXp + levelOffsets[0]; 
        
        }
        else if(playerXp < 17500){

            playerLevel = (playerXp - 5000) / levelOffsets[1] + 5; 
            currentLevelXp = levelOffsets[1] * (playerLevel - 5) + 5000; 
            nextLevelXp = currentLevelXp + levelOffsets[1]; 

        }
        else if(playerXp < 42500 ){

            playerLevel = (playerXp - 17500) / levelOffsets[2] + 10; 
            currentLevelXp = levelOffsets[2] * (playerLevel - 10) + 17500; 
            nextLevelXp = currentLevelXp + levelOffsets[2]; 

        }
        else if(playerXp < 72500){

            playerLevel = (playerXp - 42500) / levelOffsets[3] + 15; 
            currentLevelXp = levelOffsets[3] * (playerLevel - 15) + 42500; 
            nextLevelXp = currentLevelXp + levelOffsets[3]; 

        }
        else{

            playerLevel = (playerXp - 72500) / levelOffsets[4] + 20; 
            currentLevelXp = levelOffsets[4] * (playerLevel - 20) + 72500; 
            nextLevelXp = currentLevelXp + levelOffsets[4]; 
        }
    }

    void SetAllDisplays(){
        foreach (ProgressionDisplay display in displays)
        { display.SetDisplays(); }
    }

    void SyncRewards(){
        int value = playerLevel / 5;
        // if(value > 0){
        for (int i = 0; i < unlockPrompts.Length; i++)
        {
            // unlockButtons[i].SetActive(true);
            // if(i < value){
            //     unlockPrompts[i].SetActive(false);
            //     if(unlockButtons[i] != null){
            //     }
            //     // unlockEvent[i].Invoke();
            //     continue;
            // }
            // else{
            // unlockPrompts[i].SetActive(false);
            // }
            // Debug.Log("nope");
        // }
        } 
    }


    // Interface Implementations
    
    public void OnRoundChange()
    {
        if(GameManager.main.LvlCount < 10){
            AddXp(  (int)(100 * GameManager.main.difficulty_coefficient) );
        }
        else if(GameManager.main.LvlCount < 20){
            AddXp(  (int)(200 * GameManager.main.difficulty_coefficient) );
        }
        else if(GameManager.main.LvlCount < 30){
            AddXp(  (int)(300 * GameManager.main.difficulty_coefficient) );
        }
        else if(GameManager.main.LvlCount < 40){
            AddXp(  (int)(400 * GameManager.main.difficulty_coefficient) );
        }
        else{
            AddXp(  (int)(500 * GameManager.main.difficulty_coefficient) );
        }
    }

    public void OnRoundStart() {}
    public void OnRoundEnd() {}
    public void OnPortalInteract() {}

    public void OnPurchaseWeapon(WeaponItem weapon, int cost)
    {
        AddXp(  (int)( 150 * (weapon.tier+1) * GameManager.main.difficulty_coefficient ) );
    }

    public void OnPurchaseUpgrade(Upgrade upgrade, int cost, int level)
    {
        AddXp((int)( 150 * (1 + cost / 10)*GameManager.main.difficulty_coefficient));
    }

    public void OnBossRoundChange() {}
    
    public void OnBossRoundStart() {
        AddXp(150);
    }
    public void OnBossRoundEnd() {
        AddXp((int)( 500 * (BossRoundManager.main.bossRoundTier+1)*GameManager.main.difficulty_coefficient ));
    }

    public void OnBossSpawn() {}
}
