using System.Collections;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
using System.Threading.Tasks;
using static BossRoundManager;

public class PortalScript : MonoBehaviour
{
    public EnemyCounter enemyCounter;
    public LvlStarter lvlStarter;
    public float Delay, dist;
    public int currentMapIndex {get; private set;}

    [SerializeField]
    private Animator portalAnim;

    [SerializeField]
    private EnemyList enemy_entry_list;

    public bool loadingScene, portalIsUsable;

    [SerializeField]
    private FloorColorScript floor_color;

    public ChestSystemManager chestSystem;


    [SerializeField]
    private int _mode = 0, runic_map, prime_runic_map; 

    [SerializeField]
    private SpriteRenderer renderer;
    [SerializeField]
    private UnityEngine.Rendering.Universal.Light2D light;

    [SerializeField]
    private ParticleSystem particles, secondaryParticles;

    public Color[] mode_colors, map_styles;
    public static PortalScript main {get; private set;}
    public BossType? boss = null;
    public int Mode { get => _mode; private set => _mode = value; }
    public PortalType portalType;
    public BossType portalOptionalBossType;

    [SerializeField] bool inBlessingSequence;

    public enum PortalType // Assign a boss in BossType? boss if this is a bosstrial. otherwise set a custom map (still in the works)
    {
        MAIN,
        BOSSTRIAL,
        CUSTOM
    }


    void OnEnable()
    {
        if (Mode == 3 && main == this)
        {
            BossRoundCounterUI.main.UpdateDisplay(true);
        }

        portalAnim.Play("PortalAnim");
        portalIsUsable = true;
        
    }
    public void StopParticles(){
        particles.Stop();
        secondaryParticles.Stop();
    }
    
    public void StartParticles()
    {
        particles.Play();
        secondaryParticles.Play();
    }

    void Awake(){
        if(main == null){
            if(portalType == PortalType.MAIN) {
                main = this;
                BlessingDisplayManager.instance.Finished += EscapeBlessingSequence;    
            }
        }
        else if(main != this){
            SetMode(1);
            if(portalType == PortalType.BOSSTRIAL)
            {
                boss = portalOptionalBossType;
            }
            // Destroy(this);
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
        if (dist < 0.5 && portalIsUsable)
        { NextLvl(); }

        if(main == this)
        {
            if(BossRoundManager.main.timeUntilNextBoss == 1 || KillBox.currentGame.gamemode == Game.Gamemode.BOSSCHALLENGE){
                
                if(Mode != 3){
                    SetMode(3);
                }
            }
            else if(Mode == 3){
                SetMode(0);
            }
        }

        //Debug.Log(Maps.Count);
    }

    public void NextLvl()
    {
        portalIsUsable = false;
        print("Entering next level");

        StartCoroutine(LoadNextScene());
        Player.main.Dissapear();
        Player.main.movement.SetCanMove(false);
        LvlStarter.main.DisableInGameButtons();
        // PulseEffectManager.instance.AddEffect(transform.position, expandRate: 0.5f, strength:0.03f);
    }

    public void SetMode(int mode, bool open = true, BossType? bossType = null){
        Mode = mode;

        renderer.color = mode_colors[Mode];
        light.color = mode_colors[Mode];
        particles.startColor = mode_colors[Mode];

        if (bossType != null && mode == 1)
        {
            boss = bossType;
            portalAnim.Play("SpecialPortalAnim");
        }

        if (open)
        {
            portalAnim.Play("PortalAnim");
        }

        if(mode == 3)
        {
            if(BossRoundManager.main.GetNextBossRoundEntry() != null)
            {
                boss = ((BossRoundEntry)BossRoundManager.main.GetNextBossRoundEntry()).bossType;
            }
            else
            {
                // Something Must be done here, only happens when the player has passed all set boss rounds OR in boss challenge
            }
            
            // PulseEffectManager.instance.AddEffect(transform.position, expandRate: 0.25f, strength:-0.025f);
        }
    }

    void InitNewRound(int next_map = -1){
        switch (Mode)
        {
            case 0: { GameManager.main.InitNewRound(next_map); break; }
            case 1: { GameManager.main.InitBossRound((BossType)boss); break; }   
            case 3: { GameManager.main.InitBossRound((BossType)boss); break; }   
            
        }
    }

    void InitSpecialBossRound(BossType bossType){
        GameManager.main.InitBossRound(bossType);
    }


    IEnumerator LoadNextScene()
    {
        loadingScene = true;
        portalAnim.Play("portalDissapear");
        ChallengeLib.UpdateChallengeValues("HUNTER", "KILLS", ChallengeFields.kills);

        float time = Delay;
        MapData map = GameManager.main.GetCurrentMap();

        Color wallCol = map.wallTiles.color;
        Color floorCol = map.floorTiles.color;

        while (time > 0)
        {
            map.wallTiles.color = Color.Lerp(Color.clear, wallCol, time / Delay);
            map.floorTiles.color = Color.Lerp(Color.clear, floorCol, time / Delay);

            time -= Time.deltaTime;
            yield return null;
        }

        GameManager.main.GetCurrentMap().Root.SetActive(false);
        map.wallTiles.color = wallCol;
        map.floorTiles.color = floorCol;
        // yield return new WaitForSeconds(Delay);

        // Temporary blessing UI code
        if (BossRoundManager.main.timeUntilNextBoss == 0)
        {
            Debug.Log("Was a boss round, must do blessing ui");
            switch (boss)
            {
                case BossType.SHARD:
                    BlessingDisplayManager.instance.BeginBlessingSequence(BlessingDisplayManager.BlessingType.SHARD);
                    inBlessingSequence = true;
                    
                    break;
                
                case BossType.CUTTER:
                    BlessingDisplayManager.instance.BeginBlessingSequence(BlessingDisplayManager.BlessingType.CUTTER);
                    inBlessingSequence = true;
                    
                    break;

                case BossType.GUARDIANS:
                    BlessingDisplayManager.instance.BeginBlessingSequence(BlessingDisplayManager.BlessingType.GUARDIANS);
                    inBlessingSequence = true;
                    
                    break;

            }
        }

        while (inBlessingSequence)
        {
            yield return null;
        }

        InitNewRound();
        loadingScene = false;
        Player.main.Appear();
        Player.main.movement.SetCanMove(true);
        
    }
    void EscapeBlessingSequence()
    {
        inBlessingSequence = false;
    }

    void OnDestroy()
    {
        if(main == this){
            BlessingDisplayManager.instance.Finished -= EscapeBlessingSequence;    
        }
    }
}
