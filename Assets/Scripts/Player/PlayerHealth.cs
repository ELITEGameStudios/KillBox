using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class PlayerHealth : MonoBehaviour, ISelfResListener
{
    public UltraManager ultraManager;

    [SerializeField]
    private int MaxHealth; 
    public int CurrentHealth;
    public int netMaxHealth {get { return MaxHealth - (int)DebuffedHealth; }}
    public float DebuffedHealth
    {
        get { return debuffedHealth; }
        set
        {
            if (debuffedHealth < value) { debuffHealthRegenTime = 0; } // Resets regen timer if nessecary
            
            debuffedHealth = Mathf.Clamp(value, 0, MaxHealth * 4 / 5);  // Sets value with constraints
            if (CurrentHealth > netMaxHealth){ CurrentHealth = netMaxHealth; } // ensures current health is not higher than netMaxHealth
        }
    }
    private float debuffedHealth;


    
    private float debuffHealthRegenTime;
    public AnimationCurve debuffHealthRegenRate;

    public Volume hit_volume;

    public GameObject camera_obj;
    public bool IsPlayer, regen, Revived = false, immune = false, tutorialPlayer, hit_volume_on = false, isDamageless = false;
    public float RegenTime, immunityTime, camera_magnitude, camera_shake_duration;
    private float currentRegenTime, currentImmunityTime;
    private bool regenToggle = false, camera_is_shaking = false, health_has_changed = false, triggerDamageVolume, triggerCameraShake;
    public UnityEvent OnDie, AdRequestPopup, OnRestart;

    [SerializeField]
    private BuffsManager buffsManager;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private float normalized_weight_inv, hitVolumeCoefficient;

    [SerializeField]
    private AudioClip[] clips;

    [SerializeField]
    private StageManager stage_txt;

    [SerializeField]
    private Toggle dmgVolumeToggle;
    private Toggle cameraShakeToggle;
    public int[] defaultHealth;

    public bool isMaxHealth {get { return CurrentHealth == netMaxHealth; }}
    public bool hasHealthDebuff {get { return DebuffedHealth > 0; }}

    void Start(){
        dmgVolumeToggle = QualityControl.main.DmgVolumeToggle;
        cameraShakeToggle = QualityControl.main.CsVolumeToggle;

        int triggerDamageVolumeIndex = PlayerPrefs.GetInt("dmg_volume", 1);
        int triggerCameraShakeIndex = PlayerPrefs.GetInt("camera_shake", 1);
        
        if(triggerDamageVolumeIndex == 1){ triggerDamageVolume = true; }
        else{triggerDamageVolume = false;} 
        if(dmgVolumeToggle != null){
            dmgVolumeToggle.isOn = triggerDamageVolume;
        }
        

        if(triggerCameraShakeIndex == 1){ triggerCameraShake = true; }
        else{triggerCameraShake = false;} 
        if(cameraShakeToggle != null){
            cameraShakeToggle.isOn = triggerCameraShake;
        }

        hitVolumeCoefficient = 1;

        MaxHealth = 150;
        //MaxHealth = PlayerPrefs.GetInt("Health", 150);
        CurrentHealth = MaxHealth;
    }

    public void ToggleDmgVolume(){
        triggerDamageVolume = dmgVolumeToggle.isOn;
        if(triggerDamageVolume){
            PlayerPrefs.SetInt("dmg_volume", 1);
        }
        else{
            PlayerPrefs.SetInt("dmg_volume", 0);
            hit_volume_on = false;
            if(hit_volume.weight > 0){hit_volume.weight = 0;}
        }
        PlayerPrefs.Save();
    }

    public void ToggleCameraShake(){
        triggerCameraShake = cameraShakeToggle.isOn;
        if(triggerCameraShake){
            PlayerPrefs.SetInt("camera_shake", 1);
        }
        else{
            PlayerPrefs.SetInt("camera_shake", 0);
        }
        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        if(dmgVolumeToggle != null){
            if(dmgVolumeToggle.isOn != triggerDamageVolume)
            {ToggleDmgVolume();}
        }

        if(cameraShakeToggle != null){
            if(cameraShakeToggle.isOn != triggerCameraShake)
            {ToggleCameraShake();}
        }


        normalized_weight_inv = (float)(MaxHealth - CurrentHealth)/ (float)MaxHealth;
        //MaxHealth = manager.HealthCount;

        if (IsPlayer)
        {
            GameplayUI.instance.GetHealthSlider().maxValue = netMaxHealth;
            GameplayUI.instance.GetHealthSlider().value = CurrentHealth;
        }
        
        if (CurrentHealth >= netMaxHealth)
        {   
            // If the max possible current health is reached (fully regenerated)
            regen = false;
            CurrentHealth = netMaxHealth;
            currentRegenTime = RegenTime;
        }
        else if (currentRegenTime > 0)
        {
            // If the timer until regen is not yet elapsed
            regen = false;
            regenToggle = true;
            currentRegenTime -= Time.deltaTime;
        }
        else{
            // If the timer until regen is elapsed
            if(!regen && regenToggle){
                regen = true;
                regenToggle = false;
            }
        }

        if(currentImmunityTime > 0){
            immune = true;
            currentImmunityTime -= Time.deltaTime;
        }
        if(currentImmunityTime <= 0){
            if(immune){
                immune = false;
            }
        }

        if (hasHealthDebuff)
        {
            // Only if the player has a debuff
            debuffHealthRegenTime += Time.deltaTime;
            DebuffedHealth -= debuffHealthRegenRate.Evaluate(debuffHealthRegenTime) * Time.deltaTime;    
        }

        GameplayUI.instance.GetHealthAnimator().SetBool("regen", regen);
        GameplayUI.instance.GetHealthAnimator().SetBool("hurt", (float)CurrentHealth/MaxHealth <= 0.34f);
        // GameplayUI.instance.GetHealthAnimator().SetBool("hurt", false);

        //if(CurrentHealth / MaxHealth <= 0.15f && stage_txt.can_hurt_message){
        //    stage_txt.HurtDialogue();
        //}

        if (hit_volume.weight != normalized_weight_inv && hit_volume_on)
        {
            if (hit_volume.weight - Time.deltaTime < normalized_weight_inv)
            {
                hit_volume.weight = normalized_weight_inv;
            }
            else
                hit_volume.weight -= Time.deltaTime;
        }
        else if (!triggerDamageVolume)
        {
            hit_volume_on = false;
            hit_volume.weight = 0;
        }
        
    }
    void FixedUpdate()
    {
        if (regen)
        {
            if(!PauseHandler.main.paused){
                CurrentHealth += (int)(100 * Time.fixedDeltaTime);
                // CurrentHealth += 2;
            }
            //health_has_changed = true;
        }
    }

    public void AddDebuffHealth(float debuffHealth)
    {
        DebuffedHealth += debuffHealth;
        // debuffHealthRegenTime = 0;

    }

    public void SetDebuffHealth(float debuffHealth)
    {
        DebuffedHealth = debuffHealth;
        // debuffHealthRegenTime = 0;

    }

    public void TakeDmg(int Dmg, float immunity_time)
    {
        if (!(EquipmentManager.instance.equipmentType == EquipmentManager.EquipmentType.ULTRAMODE && EquipmentManager.instance.usingEquipment))
        {
            CurrentHealth -= (int)(Dmg * (1 - (0.05f * buffsManager.buff_strength[0])));
            immune = true;
            isDamageless = false;

            if (immunity_time == 0)
            {
                currentImmunityTime = immunityTime;
            }
            else
            {
                currentImmunityTime = immunity_time;
            }

            currentRegenTime = RegenTime;
            regen = false;

            if (triggerDamageVolume)
            {
                hit_volume.weight = (float)((MaxHealth - CurrentHealth) * 1.5f / (float)MaxHealth) * hitVolumeCoefficient;
                hit_volume_on = true;
            }
            else
            {
                hit_volume.weight = 0;
            }

            GameplayUI.instance.GetHealthAnimator().Play("hit");
            //audio.clip = clips[0];
            //audio.pitch = Random.Range(0.9f, 1.2f);
            //audio.Play();

            if (CurrentHealth <= 0)
            {
                AttemptExtraLife();
            }

            if (triggerCameraShake)
            {
                StartCoroutine(CameraShake(camera_shake_duration, camera_magnitude));
            }
        }
    }

    public void TakeImmediateDmg(int Dmg, float immunity_time)
    {
        if (!(EquipmentManager.instance.equipmentType == EquipmentManager.EquipmentType.ULTRAMODE && EquipmentManager.instance.usingEquipment))
        {
            // CurrentHealth -= (int)(Dmg * (1 - (0.05f * buffsManager.buff_strength[0])));
            CurrentHealth -= (Dmg);
            immune = true;

            if (immunity_time == 0)
            {
                currentImmunityTime = immunityTime;
            }
            else
            {
                currentImmunityTime = immunity_time;
            }

            GameplayUI.instance.GetHealthAnimator().Play("hit");
            currentRegenTime = RegenTime;
            regen = false;
            hit_volume.weight = (float)((MaxHealth - CurrentHealth) / (float)MaxHealth) * hitVolumeCoefficient;
            hit_volume_on = true;

            //audio.clip = clips[0];
            //audio.pitch = Random.Range(0.9f, 1.2f);
            //audio.Play();

            //if(!camera_is_shaking){
            if (CurrentHealth <= 0)
            {
                AttemptExtraLife();
            }

            StartCoroutine(CameraShake(camera_shake_duration, camera_magnitude));
            //}
        }
    }

    public void AttemptExtraLife(){

        //audio.clip = clips[1];
        //audio.pitch = Random.Range(0.9f, 1.2f);
        //audio.Play();

        if(GameManager.main.freeplay){
            CurrentHealth = MaxHealth;
        }
        else{
            if(SelfReviveTokenManager.main != null){
                if (SelfReviveTokenManager.main.CanSelfRes()){
                    Debug.Log("promped");
                    KillboxEventSystem.TriggerSelfResPromptEvent();
                }
            }  
            Die();
        }
    }

    public void InvokeRestart(){
        OnRestart.Invoke();
    }
    public void Die()
    {
        LeaderboardManager.main.SendScore();
        KillboxEventSystem.TriggerDeathEvent();
        hitVolumeCoefficient = 0.33f;
        OnDie.Invoke();
    }

    public void DestroyInvoke()
    {
        Destroy(gameObject);
    }

    public int GetMaxHealth(){
        return MaxHealth;
    }

    public void SetMaxHealth(int new_health, string key, bool set_current){
        if(key == "JWBVIHEWBCV*&T^&237236fg3gv38fvr3v3v6)*&"){
            MaxHealth = new_health;
            if(set_current){
                CurrentHealth = MaxHealth;
                Debug.Log("SETMACXHEALTH");
            }
                
        }
    }
    public void SetMaxHealth(int new_health, bool set_current){
        
        MaxHealth = new_health;
        if(set_current)
            CurrentHealth = MaxHealth;
    
    }

    public void ResetHealth()
    {
        CurrentHealth = MaxHealth;
        DebuffedHealth = 0;
    }

    public void MaxHealthCheck(){
        if(UpgradesManager.Instance.current_levels[1] > 0){
            float value = UpgradesList.health.values[UpgradesManager.Instance.current_levels[1] - 1];
            MaxHealth = defaultHealth[KillBox.currentGame.difficultyIndex] + ((int)value - 250);
            regen = true;
            regenToggle = false;
        }
    }


    public IEnumerator CameraShake(float duration, float magnitude){

        camera_is_shaking = true;

        Vector3 root_local_position = camera_obj.transform.localPosition;

        float time_elapsed = 0f;

        while(time_elapsed < duration)
        {
            camera_obj.transform.localPosition = new Vector3(Random.Range(-1, 1) * magnitude, Random.Range(-1, 1) * magnitude, 0);

            time_elapsed += Time.deltaTime;

            yield return null;
        }

        camera_obj.transform.localPosition = root_local_position;

        camera_is_shaking = false;
    }

    public void OnSelfResPrompt()
    {
        
    }

    public void OnSelfRes()
    {
        ResetHealth();
        hitVolumeCoefficient = 1;
    }

}
