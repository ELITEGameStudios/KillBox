using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class LvlStarter : MonoBehaviour
{
    public GameObject[] OffList, OnList, Teleporters;
    [SerializeField] private GameObject weapons, upgrades, starterObject;
    private Transform[] weaponsPos, upgradesPos;
    [SerializeField] private Vector2 targetWeaponsPos, targetUpgradesPos, playerPos, targetStarterPos;
    public Spawn GetSpawn;
    [SerializeField] private float timer, animationTime;
    public bool HasStarted {get; private set;}
    [SerializeField] private bool animationFinished;
    public Text text;
    public TpScript[] TpScripts;

    public static LvlStarter main {get; private set;}

    private IEnumerator lvlStartRefreshable;

    void Awake()
    {
        if(main == null){ main = this; }
        else{ Destroy(this); } 
    }

    public void ManualStartLvl(){
        HasStarted = true;
    }
    public void ManualStopLvl(){
        HasStarted = false;
    }

    void Update()
    {
        if(timer >= animationTime && !animationFinished){
            animationFinished = true;
            starterObject.transform.position = targetStarterPos;

            weapons.GetComponent<InGameButtonHandler>().Activate();
            upgrades.GetComponent<InGameButtonHandler>().Activate();
            starterObject.GetComponent<InGameButtonHandler>().Activate();

            KillboxEventSystem.TriggerShopButtonAnimationEndEvent();

        }
        if(!animationFinished){
            weapons.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, -180), Quaternion.Euler(0, 0, 0), CommonFunctions.SineEase(timer, animationTime));
            upgrades.transform.rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, -180), Quaternion.Euler(0, 0, 0), CommonFunctions.SineEase(timer, animationTime));
            // starterObject.transform.position = Vector2.Lerp(playerPos, targetStarterPos, CommonFunctions.SineEase(timer, animationTime));

            weapons.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, CommonFunctions.SineEase(timer, animationTime));
            upgrades.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, CommonFunctions.SineEase(timer, animationTime));
            // starterObject.transform.localScale = Vector2.Lerp(Vector2.zero, Vector2.one, CommonFunctions.SineEase(timer, animationTime));
            
            timer += Time.deltaTime;
        }
    }

    // Update is called once per frame
    public void ImmediateStart()
    {
        StartLvl();
    }

    void StartLvl()
    {
        weapons.SetActive(false);
        upgrades.SetActive(false);
        // starterObject.SetActive(false);

        RoundStartDisplay.main.StartAnimation();

        if (!BossRoundManager.main.isBossRound)
        {
            GetSpawn.StartSpawnSequence();
            GameplayUI.instance.GetLevelDisplayAnimator().SetBool("InGame", true);
        }
        else
        {
            MainAudioSystem.main.TriggerBossMusic(BossRoundManager.main.bossRoundTier);
            VolumeControl.main.SetSilentSnapshot(false, 0);
            BossRoomSpawnSystem.main.StartSpawnSequence();
            KillboxEventSystem.TriggerBossRoundStartEvent();
        }

        
        // GameplayUI.instance.GetUpgradeTabMaster().CloseTabs();
        GunHandler.Instance.SetUIStatus(false);

        for(int i = 0; i < OffList.Length; i++)
            OffList[i].SetActive(false);

        for (int ii = 0; ii < OnList.Length; ii++)
            OnList[ii].SetActive(true);

        //GetAudio.outputAudioMixerGroup = mixerGroup;

        // foreach (Door door in Door.doors){ 
        //     door.RoundStart();
        // } 

        MainAudioSystem.main.Action();
        //AudioSystemMaster.main.PlayAction();

        Teleporters = GameObject.FindGameObjectsWithTag("Teleporter");
        // for(int i = 0; i < TpScripts.Length; i++)
        // {
        //     TpScripts[i].IsActive = true;
        // }

        KillboxEventSystem.TriggerRoundStartEvent();
        GameManager.main.roundState = GameManager.RoundState.MIDROUND;
        HasStarted = true;
    }

    public void DisableInGameButtons()
    {
        weapons.SetActive(false);
        upgrades.SetActive(false);
    }

    public void InitiatePostRound(int mapId)
    {
        // if(KillBox.currentGame.round < 3){return;}
        timer = 0;

        targetWeaponsPos = GameManager.main.GetMapByID(mapId).Weapons.position;
        targetUpgradesPos = GameManager.main.GetMapByID(mapId).Upgrades.position;

        weapons.transform.position = targetWeaponsPos;
        upgrades.transform.position = targetUpgradesPos;

        // targetStarterPos= GameManager.main.GetMapByID(mapId).Starter.position;

        animationFinished = false;

        weapons.SetActive(true);
        upgrades.SetActive(true);
        // starterObject.SetActive(true);

        weapons.GetComponent<InGameButtonHandler>().Deactivate();
        upgrades.GetComponent<InGameButtonHandler>().Deactivate();
        // starterObject.GetComponent<InGameButtonHandler>().Deactivate();

        // this.playerPos = playerPos;

    }    

    public void InitiatePreround(int mapId, Vector3 playerPos)
    {
        // HasStarted = false;
        // timer = 0;

        // // try{
        // targetWeaponsPos = GameManager.main.GetMapByID(mapId).Weapons.position;
        // targetUpgradesPos = GameManager.main.GetMapByID(mapId).Upgrades.position;
        // targetStarterPos= GameManager.main.GetMapByID(mapId).Starter.position;
        // // }
        // // catch(NullReferenceException){
        // //     return;
        // // }

        // animationFinished = false;

        // weapons.SetActive(true);
        // upgrades.SetActive(true);
        // starterObject.SetActive(true);

        // weapons.GetComponent<InGameButtonHandler>().Deactivate();
        // upgrades.GetComponent<InGameButtonHandler>().Deactivate();
        // starterObject.GetComponent<InGameButtonHandler>().Deactivate();
        
        this.playerPos = playerPos;
        Invoke(nameof(StartLvl), 1);
        
    }    
}
