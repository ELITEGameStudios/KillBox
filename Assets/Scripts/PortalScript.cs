using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using System.Threading.Tasks;

public class PortalScript : MonoBehaviour
{
    public GameObject[] OnList;
    public GameObject portal;
    public Animator portalAnimator;
    public GameManager gameManagerVar;
    public EnemyCounter enemyCounter;
    public LvlStarter lvlStarter;
    public Spawn GetSpawn;
    public string AnimName, opening_anim;
    public float Delay, dist;
    public int currentMapIndex {get; private set;}

    [SerializeField]
    private Animator overlay_anim;

    [SerializeField]
    private ShopScript shop;

    [SerializeField]
    private EnemyList enemy_entry_list;

    public bool loadingScene, loadedScene;

    [SerializeField]
    private MapEvolutionManager mapEvolutionManager;

    [SerializeField]
    private FloorColorScript floor_color;

    public ChestSystemManager chestSystem;


    [SerializeField]
    private int _mode = 0, runic_map, prime_runic_map, prismatic_map; 

    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private UnityEngine.Rendering.Universal.Light2D light;
    [SerializeField]
    private ParticleSystem particles;

    public Color[] mode_colors, map_styles, wall_styles;

    public static PortalScript main {get; private set;}
    public int Mode { get => _mode; private set => _mode = value; }


    [SerializeField]
    private QuestionInitiator escapeRoomInitiator;

    void OnEnable(){
        if (Mode == 3){
            BossRoundCounterUI.main.UpdateDisplay(true);
        }
        
    }

    void Awake(){
        if(main == null){
            main = this;
        }
        else{
            Destroy(this);
        }
    }

    async void LoadPathfinding(){
        await Task.Run(() =>
        {
           AstarPath.active.UpdateGraphs(GameManager.main.GetMapByID(currentMapIndex).Obstacles.bounds);
           return;
        });
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(Player.main.tf.position, transform.position);
        if (dist < 0.5)
        { NextLvl(); }

        if(BossRoundManager.main.timeUntilNextBoss == 1){
            
            if(Mode != 3){
                SetMode(3);
            }
        }
        else if(Mode == 3){
            SetMode(0);
        }

        //Debug.Log(Maps.Count);
    }

    void NextLvl()
    {

        // portalAnimator.Play(AnimName);
//        loadingScene = true;
        StartCoroutine(LoadNextScene());

    }

    public void SetMode(int mode, bool open = true){
        Mode = mode;

        renderer.color = mode_colors[Mode];
        light.color = mode_colors[Mode];
        particles.startColor = mode_colors[Mode];

        if(open){
            portalAnimator.Play(opening_anim);
        }
    }

    public void StartHubMap(){

        SelectMap(17);
        LoadMap();
        SetPositions();
    }

    void SetRound(){

        ChallengeFields.UpdateRound(gameManagerVar, gameManagerVar.isFireRound);
        // Damageless Code:
        if(Player.main.health.isDamageless){
            // Damageless Bonus is increased the further you get in the game.
            if(KillBox.currentGame.round >= 3){gameManagerVar.OnPickupToken(1, false);}
            if(KillBox.currentGame.round >= 11){gameManagerVar.OnPickupToken(2, false);}
            if(KillBox.currentGame.round >= 18){gameManagerVar.OnPickupToken(3, false);}
            }
        Player.main.health.isDamageless = true;

        if(BossRoundManager.main.isBossRound){gameManagerVar.OnPickupToken(7, false);}
        else{gameManagerVar.OnPickupToken(1, false);}
        gameManagerVar.InitNextRound();
    }

    void SelectMap(int next_map = -1){

        // CurrentMap = 22;
        // return;

        if(next_map == -1){
            // CurrentMap = 4;
            List<int> availableMaps = GameManager.main.GetAvailableMaps();

            currentMapIndex = availableMaps[Random.Range(0, availableMaps.Count)];
            

            // if(CurrentMap == 1 && gameManagerVar.LvlCount >= 5)
            
            // { CurrentMap = 5; }

            // else if(CurrentMap == 2 && gameManagerVar.LvlCount >= 5)
           
            // { CurrentMap++; }

            // else if(CurrentMap == 9 && gameManagerVar.LvlCount < 20)
            
            // { CurrentMap = 8; }

            // else if(CurrentMap == 15)
            
            // { CurrentMap = 18; }
        }
        else{ currentMapIndex = next_map; }
    }

    void LoadMap(){
        GameManager.main.SetNewMap( GameManager.main.GetMapByID(currentMapIndex) );
    }

    public void UpdatePathfinding(){
        AstarPath.active.UpdateGraphs(GameManager.main.GetMapByID(currentMapIndex).Obstacles.bounds);
        Debug.Log("Pathfinding Updated");
    } 

    public void ManageSpawns(int map = -1){ // may be deprecated
        if(map == -1){ map = currentMapIndex;}

        // for(int i = 0; i < GetSpawn.spawns.Count; i++)
        // {
        //     if (GetSpawn.spawns[i].TargetLvl == map && gameManagerVar.LvlCount >= GetSpawn.spawns[i].unlock_on_level)
        //     {
        //         GetSpawn.spawns[i].ActiveSpawn = true;
        //         GetSpawn.spawns[i].RefreshEntries();
        //     }
        //     else
        //         GetSpawn.spawns[i].ActiveSpawn = false;
        // }
    }

    void SetPositions(){
        Player.main.tf.position = GameManager.main.GetMapByID(currentMapIndex).Player.position;
        transform.position = GameManager.main.GetMapByID(currentMapIndex).Portal.position;

        GameObject[] allies = GameObject.FindGameObjectsWithTag("Ally");

        if(allies.Length > 0)
        {
            for(int i = 0; i < allies.Length; i++) 
            
            { allies[i].transform.position = Player.main.tf.position; }
        }
    }

    void ToggleLists(){
        for (int i = 0; i < OnList.Length; i++)
            OnList[i].SetActive(true);
    }

    void RemainingTasks(){
        //floor_color.ChangeColor(false);
        ToggleChannelManager.main.ResetChannels();
        enemy_entry_list.BossAppearanceCheck(currentMapIndex);
        InventoryUIManager.Instance.UpdateUI();
        UpgradesManager.Instance.ChooseUpgrade();
        chestSystem.RefreshCheck();

        BossRoundManager.main.UpdateCounters();
        if(_mode != 3){ BossRoundManager.main.SetBossRound(false); }

        if(BossRoundManager.main.timeUntilNextBoss == 1){ SetMode(3); }
        else{ SetMode(0); }

    }

    void StartRoundCountdown(){
        GridAnimationManager.instance.DoIntroRoundAnimation();
        lvlStarter.InitiatePreround(currentMapIndex, GameManager.main.GetMapByID(currentMapIndex).Player.position);
        enemyCounter.Reset();
    }


    void InitNewRound(int next_map = -1){

        if(GameManager.main.EscapeRoom()){
            escapeRoomInitiator.EndGame();
            return;
        }

        if(Mode == 0){

            if(BossRoundManager.main.isBossRound){
                MainAudioSystem.main.PlayMainLoop();
                VolumeControl.main.SetSilentSnapshot(false, 2);
            }

            SetRound();
            SelectMap();
            LoadMap();
            ManageSpawns();
            SetPositions();
            ToggleLists();
            RemainingTasks();
            
            Player.main.NewRound();
            MainAudioSystem.main.Rest();
            GunHandler.Instance.SetUIStatus(true);
            
            
            // foreach (Door door in Door.doors){ door.NextRound(); } 
            StartRoundCountdown();
        }
        else{ 
            
            if(Mode == 1){

                SelectMap(runic_map);
                LoadMap();
                SetPositions();
                floor_color.ChangeActiveColor(map_styles[Mode], map_styles[Mode], true);
            }
            else if(Mode == 2){
                SelectMap(prime_runic_map);
                LoadMap();
                SetPositions();
                floor_color.ChangeActiveColor(map_styles[Mode], map_styles[Mode], false);
            }
            else if(Mode == 3){
                int mapToSelect;
                SetRound();
                if(GameManager.main.LvlCount == enemy_entry_list.bossRounds[0]){ mapToSelect = 101; }
                else if(GameManager.main.LvlCount == enemy_entry_list.bossRounds[1]){ mapToSelect = 102; }
                else if(GameManager.main.LvlCount == enemy_entry_list.bossRounds[2]){ mapToSelect = 103; }
                else { mapToSelect = 101; }
                
                SelectMap(mapToSelect);
                LoadMap();
                ManageSpawns();
                SetPositions();
                ToggleLists();
                RemainingTasks();
                
                Player.main.NewRound();
                GunHandler.Instance.SetUIStatus(true);
                BossRoundManager.main.SetBossRound(true);
                
                // foreach (Door door in Door.doors){ door.NextRound(); } 
                floor_color.ChangeColor(Color.black, Color.white);
                StartRoundCountdown();
                // KillboxEventSystem.TriggerBossRoundStartEvent();
            }

            overlay_anim.Play("Standard");
        }
    
        KillboxEventSystem.TriggerRoundChangeEvent();
    }

    IEnumerator LoadNextScene()
    {
        loadingScene = true;
        ChallengeLib.UpdateChallengeValues("HUNTER", "KILLS", ChallengeFields.kills);
        
        float time = Delay;
        MapData map = GameManager.main.GetCurrentMap();
        CameraBgManager.instance.SetBackground(Color.black, Delay/2);

        Color wallCol = map.wallTiles.color;
        Color floorCol = map.floorTiles.color;
        
        while (time > 0){
            map.wallTiles.color = Color.Lerp(Color.clear, wallCol, time / Delay);
            map.floorTiles.color = Color.Lerp(Color.clear, floorCol, time / Delay);

            time -= Time.deltaTime;
            yield return null;
        }

        GameManager.main.GetCurrentMap().Root.SetActive(false);
        map.wallTiles.color = wallCol;
        map.floorTiles.color = floorCol;
        // yield return new WaitForSeconds(Delay);

        InitNewRound();
        loadingScene = false;
    }
}
