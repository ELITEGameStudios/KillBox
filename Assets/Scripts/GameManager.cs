using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// using LootLocker.Requests;
using UnityEngine.Events;

public class GameManager : MonoBehaviour, ISelfResListener
{
    [SerializeField]
    private int _level;
    public int ScoreCount, Dualindex, player_kills, ultra_kills, equipment_index;
    public int[] personalBests {get; private set;}
    public float time_played {get; private set;}
    [SerializeField]
    private bool escapeRoom;

    //[SerializeField]
    //private int xp;
    public bool freeplay {get; private set;} = false;

    [SerializeField]
    private HotkeyManager hotkeyManager;

    [SerializeField]
    private EnemyList enemyList;
    
    // [SerializeField]
    private PortalScript portalScript;

    [SerializeField]
    private int boss_round_start;


    public string online_username, pb_run_id;
    public bool playedTutorial, player_is_ultra, player_is_overdrive, player_has_hotshot, player_has_ally, started_game;
    [SerializeField] private Transform head_start_transform, camera_tf;

    [SerializeField] private bool has_key, head_start, can_continue_game, _is_fire_round;

    [SerializeField] private int req_equipment_kills = 30, theme_index = 0, legacyPB;
    [SerializeField]
    private GameObject use_equipment_button, tutorial_panel, gameplayHUD;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private GameObject[] use_equipment_button_list, equipment_list, equippedEquipmentDisplay;

    public bool[] completed_challenges; 
    public Text LvlTxt, ScoreTxt, ScoreTxt2, LvlText2;
    public Text[] PBTxt, ScoreTxtArray;
    public PlayerHealth playerHealth;
    public ShopScript shopScript;
    public Animator FadeAnimator;
    public GameObject[] OffList, OnList, ultramode_unlocked_objects, grenade_launcher_objects;
    public GameObject Player;
    //public Vector3 min_size, max_size; // for joystick ui

    public Spawn GetSpawn;
    // public EscapeRoomSpawnSystem escapeRoomSpawnSystem;
    public UnityEvent onPlayerDeath;
    public PauseHandler pauseHandler;
    public SceneManagerScript sceneManager;

    public Slider equipment_slider, joystick_size;
    public Slider[] equipment_slider_list, joystick_sliders;

    [SerializeField]
    FloorColorScript map_colors;

    private float ultra_recharge_tick_timer;
    [SerializeField]
    private BuffsManager buffsManager;

    [SerializeField]
    private SpriteRenderer player_render, gun_renderer;

    [SerializeField]
    private float difficulty;
    public float difficulty_coefficient;

    public int constant;

    [SerializeField]
    private List<MapData> Maps;
    public List<MapData> GetMaps {get {return Maps;}} 

    // public Color default_map_color;

    [SerializeField]
    private Color[] color_themes;

    public static GameManager main {get; private set;}

    public int maxTokensPerRound {get; private set;}
    public int tokensThisRound {get; private set;}


    void Awake(){

        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        Application.runInBackground = false;
        personalBests = new int[3];
    }

    public void AddMap(MapData map){
        Maps.Add(map);
        // SortMaps();
    }

    void SortMaps(){
        
        List<MapData> new_list = new List<MapData>();

        for(int i = 0; i < Maps.Count; i++){
            for(int j = 0; j < Maps.Count; j++){
                if(Maps[j].Index == i){
                    new_list.Add(Maps[i]);
                    continue;
                }
            }
        }

        Maps = new_list;
    }

    public MapData GetMapByID(int id){
        foreach (MapData map in Maps)
        {
            if(map.Index == id) {return map;}
        }

        return null;
    }
    public List<int> GetAvailableMaps(){
        List<int> availableIndexes = new List<int>(); 
        foreach (MapData map in Maps)
        { 
            if(
                map.DebutRound <= _level &&
                (map.RetireRound > _level || map.RetireRound == 0) 
                && map.DebutRound != -1
                && map.Index != PortalScript.main.CurrentMap) 
            {availableIndexes.Add(map.Index);} 
        }

        return availableIndexes;
    }

    public void SetlegacyPB(PlayerData data){
        if(data.legacyPBInt != 0 || data.legacyPBInt != -1){
            legacyPB = data.legacyPBInt;
        }
    }
    public int GetlegacyPB(PlayerData data){
        return legacyPB;
    }

    void Start()
    {
        _level = 1;
        ScoreCount = 0;
        //HealthCount = 150;
        Dualindex = 0;
        Gun = 0;
        difficulty = 50;

        player_render = Player.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();

        
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false;
        // map_colors.ChangeColor(default_map_color, default_map_color, false);
        map_colors.ChangeWall(Color.white);

        joystick_size.value = PlayerPrefs.GetFloat("joystick_size", 0.2f);

        for (int i = 0; i < joystick_sliders.Length; i++)
        {
            if (joystick_sliders[i].gameObject.activeInHierarchy)
            {
                joystick_size = joystick_sliders[i];
                break;
            }
        }
        
        LoadPlayer();

        if(!playedTutorial){
            tutorial_panel.SetActive(true);
        }
    }

    void Update()
    {
        if (personalBests[DifficultyManager.main.index] <= LvlCount && !freeplay)
        {
            personalBests[DifficultyManager.main.index] = LvlCount;
            if(PBInt <= personalBests[DifficultyManager.main.index])
            { PBInt = personalBests[DifficultyManager.main.index]; }
        }

        if(LvlTxt != null) {LvlTxt.text = LvlCount.ToString();}
        if(LvlText2 != null) {LvlText2.text = LvlCount.ToString();}
        if(ScoreTxt != null) {ScoreTxt.text = ScoreCount.ToString();}
        if(ScoreTxt2 != null) {ScoreTxt2.text = ScoreCount.ToString();}

        for(int i = 0; i < ScoreTxtArray.Length; i++){
            ScoreTxtArray[i].text = ScoreCount.ToString();
        }

        for(int i = 0; i < joystick_sliders.Length; i++)
        {
            for (int l = 0; l < joystick_sliders.Length; l++)
            {
                if(joystick_sliders[l] != null) joystick_sliders[l].value = joystick_size.value;
            }

            if (joystick_sliders[i].gameObject.activeInHierarchy)
            {
                joystick_size = joystick_sliders[i];
                break;
            }

        }

        //set_joystick_size
        Vector2 new_size = Vector2.Lerp(min_size, max_size, 0.6f);//joystick_size.value);
        right_joystick.GetComponent<RectTransform>().sizeDelta = new_size;
        left_joystick.GetComponent<RectTransform>().sizeDelta = new_size;

        right_joystick.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new_size * 0.9f;

        left_joystick.transform.GetChild(1).gameObject.GetComponent<RectTransform>().sizeDelta = new_size * 0.9f;

        left_joystick.transform.position = Vector3.Lerp(l_joy_small.transform.position, l_joy_big.transform.position, 0.6f);//joystick_size.value);
        right_joystick.transform.position = Vector3.Lerp(r_joy_small.transform.position, r_joy_big.transform.position, 0.6f);//joystick_size.value);

        PlayerPrefs.SetFloat("joystick_size", 0.6f);//joystick_size.value);



        if (!player_is_ultra && !player_is_overdrive && !player_has_hotshot && !player_has_ally)
        {

            equipment_slider.maxValue = req_equipment_kills;

            if (ultra_kills >= req_equipment_kills)
            {
                equipment_slider.value = equipment_slider.maxValue;
                if (!use_equipment_button.activeInHierarchy)
                {
                    use_equipment_button.SetActive(true);
                    use_equipment_button.transform.GetChild(1).GetComponent<Button>().interactable = true;
                    use_equipment_button.GetComponent<Animator>().Play("equipment_button_startup");
                }
            }
            else
            {
                equipment_slider.value = ultra_kills;
            }
        }

        if (buffsManager.buff_strength[3] > 0)
        {
            if (ultra_recharge_tick_timer <= 0)
            {
                ultra_kills++;
                ultra_recharge_tick_timer = 6 - buffsManager.buff_strength[3];
            }
            else
            {
                ultra_recharge_tick_timer -= Time.deltaTime;
            }
        }

        time_played += Time.deltaTime;
    }

    // void LateUpdate(){
    //     if(escapeRoom && WeaponItemList.Instance.GetItem("Revolver").owned){
    //         inventoryUI.SetActive(false);
    //         if(!clickedStartGame){
    //             gameplayHUD.SetActive(false);
    //         }
    //     }
    // }

    public void StartGame()
    {
        FadeAnimator.Play("FadeAnim");

        SetMaxTokenCount();

        if(freeplay){
            req_equipment_kills = 0;
        }
        
        enemyList.OnStart();
        if(escapeRoom){ AstarPath.active.UpdateGraphs(GetMapByID(20).Obstacles.bounds); }
        else{ 
            AstarPath.active.UpdateGraphs(GetMapByID(0).Obstacles.bounds); 
            
        }
        StartCoroutine(StartNumerator());

    }

    public void RestartGame(){
        FadeObj.SetActive(true);
        FadeAnimator.Play("FadeAnim");
        _level = 1;    
        ScoreCount = 0;
        ultra_kills = 0;

        KillboxEventSystem.TriggerGameRestartEvent();

        playerHealth.InvokeRestart();
        GetSpawn.CancelAllSpawns();
        GetSpawn.init();
        use_equipment_button.SetActive(false);
        use_equipment_button.transform.GetChild(1).GetComponent<Button>().interactable = true;
        
        started_game = false;
        hotkeyManager.enabled = false;
        
        EnemyCounter.main.DestroyAllEnemies();
        EnemyCounter.main.Reset();
        LvlStarter.main.ManualStopLvl();
        ObjectPool.ResetAllPools();
        ToggleChannelManager.main.ResetChannels(true);
        SetMaxTokenCount();
        DifficultyManager.main.SetDifficulty(DifficultyManager.main.index);
        UpdateDifficulty();
        UpgradesManager.Instance.ResetUpgrades();
        GunHandler.Instance.ResetWeapons();

        if(freeplay){
            req_equipment_kills = 0;
        }

        for (int i = 0; i < Maps.Count; i++)
        { Maps[i].Root.SetActive(false); }
        GetMapByID(0).Root.SetActive(true);

        Player.transform.position = GetMapByID(0).Player.position;
        PortalScript.main.transform.position = GetMapByID(0).Portal.position;
        PortalScript.main.ManageSpawns(0);
        
        gameplayHUD.SetActive(false);
        enemyList.OnStart();

        if(escapeRoom){ AstarPath.active.UpdateGraphs(GetMapByID(20).Obstacles.bounds); }
        else{ 
            AstarPath.active.UpdateGraphs(GetMapByID(0).Obstacles.bounds); 
        }
        StartCoroutine(StartNumerator());
    }

    public void InitNextRound(){
        _level++;
        SetMaxTokenCount();
        UpdateDifficulty();

        LvlTxt.text = _level.ToString();
        ScoreTxt.text = ScoreCount.ToString();

    }

    void SetMaxTokenCount(){
        tokensThisRound = 0;
        if(_level < 15){ maxTokensPerRound = 5; }
        else if(_level < 24){ maxTokensPerRound = 8; }
        else if(_level < 30){ maxTokensPerRound = 11; }
        else {maxTokensPerRound = 15;}
    }

    public void InitHubMap(){
        portalScript.StartHubMap();
    }

    public void OnSelfResPrompt()
    {}

    public void OnSelfRes()
    {
        GameObject[] Enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // EnemyCounter.main.DestroyAllEnemies(false);
        
        ExtraLifeUI.SetActive(false);
        Player.SetActive(true);

        for (int i = 0; i < shopScript.Guns.Length; i++)
        {
            // shopScript.Guns[i].GetComponent<shooterScript2D>().CanShoot = true;
            // shopScript.SecondGun[i].GetComponent<shooterScript2D>().CanShoot = true;
        }
        //right_joystick.GetComponent<Joystick>().Reset();
        //left_joystick.GetComponent<Joystick>().Reset();

        pauseHandler.PausePlay(0);
    }


    public void SubmitScoreInvoke()
    {
        onPlayerDeath.Invoke();
    }

    public void OnPlayedTutorial(){
        playedTutorial = true;
        SavePlayer();
    }

    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        ChallengeSaveSystem.SaveChallenges();
    }

    public void DeleteSystem(){
        SaveSystem.DeletePlayer();
    }

    public void SetEquipmentIndex(int index)
    {
        equipment_index = index;

        for (int i = 0; i < equipment_list.Length; i++)
        {
            equipment_list[i].SetActive(false);
            equippedEquipmentDisplay[i].SetActive(false);
        }

        equipment_slider = equipment_slider_list[equipment_index];
        use_equipment_button = use_equipment_button_list[equipment_index];
        equipment_list[equipment_index].SetActive(true);
        equippedEquipmentDisplay[equipment_index].SetActive(true);
        SavePlayer();
    }


    public void UpdateReqEquipmentKills(){

        if(req_equipment_kills < 300){
            req_equipment_kills *= 2;
        }
        if(req_equipment_kills > 300){
            req_equipment_kills = 300;
        }

        ultra_kills = 0;

        if (equipment_index == 1)
        {
            player_is_ultra = true;
            equipment_slider.maxValue = 8;
            equipment_slider.value = 8;
        }
        else if (equipment_index == 0)
        {
            player_is_overdrive = true;
            equipment_slider.maxValue = 12;
            equipment_slider.value = 12;
        }
        else if (equipment_index == 2)
        {
            player_has_hotshot = true;
            equipment_slider.maxValue = 10;
            equipment_slider.value = 10;
        }
        
        else if (equipment_index == 3){
        
            player_has_ally = true;
            equipment_slider.maxValue = 20;
            equipment_slider.value = 20;
        
        }
        
        if(freeplay){
            req_equipment_kills = 0;
        }

    }

    public void ResetUltraKills(int kills){
        ultra_kills = kills;
    }

    public void UpdateDifficulty()
    {
        difficulty = constant + 100f * (float)Mathf.Pow(LvlCount, 2f) / 30 * difficulty_coefficient; //+ 25f * ((playerHealth.GetMaxHealth() / 50) - 3);
    }

    public void AddRound(int value = 1){ //for freeplay
        if(freeplay){
            if(BossRoundManager.main.isBossRound){return;}
            if(BossRoundManager.main.timeUntilNextBoss == value)
            { _level += 2; }
            else if(BossRoundManager.main.timeSinceLastBoss == -value)
            { _level -= 2; }
            
            else
            { _level += value; }
        }
        BossRoundManager.main.UpdateCounters();
    }

    public void AddScore(int value = 1){ //for freeplay

        if(freeplay){
            ScoreCount += value;
            TokenUI.main.InitPickupAnimation(value);
        }
    }

    public void SetFreeplay(){
        freeplay = true; 
    }

    public void Quit(){
        Application.Quit();
    }

    public void OnPickupToken(int tokens, bool isToken = true){
        if(maxTokensPerRound <= tokensThisRound && isToken)
        { return; }
        ScoreCount += tokens;
        if(isToken) {tokensThisRound++;};
        TokenUI.main.InitPickupAnimation(tokens);
    }

    public float Difficulty
    {
        get { return difficulty; }
    }

    public void SetLowerScore(int value)  
    {
        if(value < ScoreCount){
            ScoreCount = value;
        }
    }


    public int Theme
    {
        get { return theme_index; }
        set { theme_index = value; }
    }

    public int LvlCount
    {
        get { return _level; }
    }

    public int ReqUltraKills
    {
        get { return req_equipment_kills; }
    }

    public int BossRoundStart
    {
        get { return boss_round_start; }
        set { boss_round_start = value; }
    }

    public bool HasKey
    {
        get { return has_key; }
        set { has_key = value; }
    }

    public bool HeadStartVar
    {
        get { return head_start; }
        set { head_start = value; }
    }

    public bool CanContinueGame
    {
        get { return can_continue_game; }
        set { can_continue_game = value; }
    }
    public bool isFireRound 
    {
        get { return _is_fire_round; }
    }

    public Color[] ColorThemes{
        get {return color_themes;}
    }

    public bool EscapeRoom(){
        return escapeRoom;
    }


    IEnumerator StartNumerator()
    {
        float timer = 0;
        float Modifier = 2;
        RectMask2D mask = gameplayHUD.GetComponent<RectMask2D>();

        Player.SetActive(false);

        camera_tf.localEulerAngles = new Vector3(0, 0, 135);
        float camera_size = camera.orthographicSize;

        yield return new WaitForSeconds(1);

        if (head_start)
        {
            Player.transform.position = head_start_transform.position;
            _level = 9;
            ScoreCount = 9;
        }

        for (int i = 0; i < OffList.Length; i++)
        {
            OffList[i].SetActive(false);
        }

        for (int i = 0; i < OnList.Length; i++)
        {
            OnList[i].SetActive(true);
        }

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        timer = 1;

        camera.gameObject.SetActive(true);
        camera.orthographicSize = 0;
        while (timer > 0)
        {

            camera_tf.localEulerAngles = new Vector3(0, 0, 90f * (Mathf.Sin(1.5708f * Modifier - 1.5708f) + 1));

            camera.orthographicSize = (camera_size * (2 - Modifier))/2;

            timer -= Time.deltaTime;
            Modifier -= Time.deltaTime;
            yield return null;
        }

        FadeObj.SetActive(false);

        timer = 1;
        while (timer > 0)
        {

            camera_tf.localEulerAngles = new Vector3(0, 0, 180f * (Mathf.Sin(1.5708f * Modifier - 1.5708f) + 1));

            camera.orthographicSize = (camera_size * (2 - (Mathf.Sin(1.5708f * Modifier - 1.5708f) + 1))) / 2;

            timer -= Time.deltaTime/2;
            Modifier -= Time.deltaTime/2;
            yield return null;
        }

        gameplayHUD.SetActive(true);

        EnemyCounter.main.DestroyAllEnemies();

        camera_tf.localEulerAngles = new Vector3(0, 0, 0);

        Player.SetActive(true);
        GameObject effect = Instantiate(player_spawn_FX, Player.transform);
        effect.transform.SetParent(null);
        
        timer = 1;
        float pads = 200;

        Player.GetComponent<TwoDPlayerController>().SetCanMove(true);
        
        while (timer > 0)
        {
            pads = 200 * timer;
            mask.padding = new Vector4(pads, pads, pads, pads);
            mask.softness = new Vector2Int((int)(400 * timer), (int)(400 * timer));


            timer -= Time.deltaTime/2;
            yield return null;
        }

        started_game = true;
        hotkeyManager.enabled = true;

        BossRoundManager.main.UpdateCounters(); 

        

        if(escapeRoom){
            escapeRoomSpawnSystem.StartSpawnSequence();
            GunHandler.Instance.EquipWeapon();
            StopCoroutine(StartNumerator());
        }

        if (!head_start)
        {
            GetSpawn.StartSpawnSequence();
            LvlStarter.main.ManualStartLvl();
        }
        else
        {
            GetSpawn.instances = 0;
            LvlStarter.main.ManualStartLvl();
        }
    }


}
