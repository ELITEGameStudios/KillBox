using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCounter : MonoBehaviour
{
    public bool noEnemiesOrSpawns, noLongerSpawning, AnimAccept;
    public bool end_of_main_round {get; private set;}
    public int enemiesInScene;
    public Spawn GetSpawn;
    public GameObject Portal;
    public string tag;
    public PortalScript portalScript;

    [SerializeField]
    private LvlStarter starter;

    [SerializeField]
    public GameObject[] enemies {get; private set;}
    public List<EnemyProfile> enemyProfiles {get; private set;}
    public List<EnemyProfile> bossProfiles {get; private set;}



    public static EnemyCounter main {get; private set;}

    // Start is called before the first frame update

    void Awake(){
        if(main == null){ main = this; }
        else if (main != this) { Destroy(this); }

        enemyProfiles = new List<EnemyProfile>();
        bossProfiles = new List<EnemyProfile>();
    }

    void Start()
    {
        noEnemiesOrSpawns = false;
        noLongerSpawning = false;
        Portal.SetActive(false);
        AnimAccept = true;
    }

    public void Reset()
    {
        noEnemiesOrSpawns = false;
        noLongerSpawning = false;
        Portal.SetActive(false);
        AnimAccept = true;
        end_of_main_round = false;

    }

    public void DestroyAllEnemies(bool bosses = true){
        for(int i = 0; i < enemyProfiles.Count; i++){
            if(enemyProfiles[i].Boss){continue;}
            else{
                Destroy(enemyProfiles[i].gameObject);
            }
            
        }
        enemyProfiles.Clear();

        if(bosses){
            for(int i = 0; i < bossProfiles.Count; i++){
                Destroy(bossProfiles[i].gameObject);
            }
            bossProfiles.Clear();
        }
        else{
            enemyProfiles.AddRange(bossProfiles);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Constantly cleans the list of enemies in the scene
        enemyProfiles.RemoveAll(item => item == null);
        bossProfiles.RemoveAll(item => item == null);
        
        // Gets current enemy count
        enemies = GameObject.FindGameObjectsWithTag(tag);
        enemiesInScene = enemyProfiles.Count;
        
        // Main condition for ending the round
        if(enemies.Length == 0 && noLongerSpawning){
            noEnemiesOrSpawns = true;
            //AudioSystemMaster.main.PlayPreRound();
        }

        // Detects if there are any spawns still to be executed
        if (GetSpawn.instances == 0 && GetSpawn.ended && starter.HasStarted ||
            BossRoundManager.main.isBossRound && BossRoundManager.main.finishedBossRoundMainPhase && starter.HasStarted)
            {noLongerSpawning = true;}

        // End round event
        if (noEnemiesOrSpawns && noLongerSpawning && !GameManager.main.EscapeRoom() && !Portal.activeInHierarchy)
        {
            Portal.SetActive(true);
            GameManager.main.CheckDamageless();

            if (BossRoundManager.main.isBossRound) { GameManager.main.OnPickupToken(7, false); }
            else
            {
                GameManager.main.OnPickupToken(1, false);
                BonusesUIManager.instance.ActivateBonus("finished_round", 1);

                if (KillBox.currentGame.round % 5 == 0)
                {
                    GameManager.main.OnPickupToken(4, false);
                    BonusesUIManager.instance.ActivateBonus("5round", 4);
                }
            }

            CameraBgManager.instance.SetBackground(Color.black, 2);

            LvlStarter.main.InitiatePostRound(PortalScript.main.currentMapIndex);
            GridAnimationManager.instance.DoEndRoundAnimation();
            KillboxEventSystem.TriggerRoundEndEvent();
            
            // Audio
            if(BossRoundManager.main.isBossRound && !end_of_main_round){
                // MainAudioSystem.main.PlayMainLoop();
                VolumeControl.main.SetSilentSnapshot(true, 2);
                KillboxEventSystem.TriggerBossRoundEndEvent();
            }
            else if(BossRoundManager.main.GetTierOfRound(GameManager.main.LvlCount+1) != -1 && !end_of_main_round){
                MainAudioSystem.main.PlayBossAmbience(
                    BossRoundManager.main.GetTierOfRound(GameManager.main.LvlCount+1), true);
                MainAudioSystem.main.Ambience();
            }

            // Portal animation
            // if (AnimAccept)
            // {
            //     portalScript.portalAnimator.Play("PortalAnim");
            //     AnimAccept = false;
            // }

            // Door behaviour
            if(!end_of_main_round){
                // foreach (Door door in Door.doors){ 
                //     door.RoundEnd();
                // }    
            
            }

            end_of_main_round = true;
        }

        if (enemies.Length > 0 && !GameManager.main.EscapeRoom())
        {
            noEnemiesOrSpawns = false;
            Portal.SetActive(false);

            if(!noLongerSpawning){ return; }
            
            float counter = 0;
            foreach(GameObject enemy in enemies) {
                if(enemy.GetComponent<RingScript>() != null){
                    counter++; 
                }
            }


            if(counter == enemiesInScene){
                Debug.Log("Time to die");
                foreach(GameObject enemy in enemies) {
                    enemy.GetComponent<EnemyHealth>().Die(false);
                }
            }
            
        }

    }

    public void AddEnemy(EnemyProfile profile){
        enemyProfiles.Add(profile);
    }

    public void AddBoss(EnemyProfile profile){
        bossProfiles.Add(profile);
    }

    public void RemoveEnemy(EnemyProfile profile){
        enemyProfiles.Remove(profile);
        if(profile.Boss){ bossProfiles.Remove(profile); }
    }

    public int GetEnemyCountOfName(string name){
        int result = 0;
        foreach (EnemyProfile profile in enemyProfiles){
            if(profile.name == name){ result++; }
        }
        return result;
    }

}
