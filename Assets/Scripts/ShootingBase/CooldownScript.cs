using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownScript : MonoBehaviour, IRestartListener
{
    public float count {get; private set;}

    [SerializeField]
    private float passive_cooldown, time_since_last_shot;
    
    [SerializeField]
    private float cooldown_time, timer;

    [SerializeField]
    private int count_max, id;

    public float CurrentChargeNormalized {get {return count / count_max;}}
    public string self_key;

    public bool cooling_down {get; private set;}

    private bool shot;
    [SerializeField] private bool primaryGun;

    [SerializeField]
    private BuffsManager buffsManager;

    [SerializeField] private Animator animator;
    [SerializeField] private Gradient cooldownGradient, shootingGradient;


    void Start(){
        count_max = 100;
        GameplayUI.instance.GetCooldownSlider().maxValue = count_max;
        animator = GameplayUI.instance.GetCooldownSlider().GetComponent<Animator>();
        shot = false;

        if(id == 0 && GunHandler.Instance.primary_cooldown != null){
            Destroy(this);
        }
        else if(id == 0){
            GunHandler.Instance.SetCooldown(id, this);
        }

        if(id == 1 && GunHandler.Instance.secondary_cooldown != null){
            Destroy(this);
        }

        else if(id == 1){
            GunHandler.Instance.SetCooldown(id, this);
        }
    }

    public void AnimCheck(){
        if(cooling_down){
            animator.Play("CoolingDown");
        }
    } 

    // Update is called once per frame
    void Update()
    {
        if(count >= count_max && !cooling_down){
            TriggerCooldown(cooldown_time);
        }

        if(cooling_down){
            if(timer <= 0){
                count = 0;
                //GameplayUI.instance.GetCooldownSlider().wholeNumbers = true;
                if(GunHandler.Instance.cooldown.self_key == self_key){
                    GameplayUI.instance.GetCooldownSlider().maxValue = count_max;
                }

                

                else if(id == 1){InventoryUIManager.Instance.secondary_element.Flip();}
                if(id == 0){InventoryUIManager.Instance.primary_element.Flip();}
                (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).maxValue = count_max;

                cooling_down = false;
                animator.CrossFade("Normal", 0.75f);

                if(id==0){
                    KillboxEventSystem.TriggerCooldownEnd(new WeaponEventData(GunHandler.Instance.primary_weapon, true, GunHandler.Instance.has_dual));
                }
                else{
                    KillboxEventSystem.TriggerCooldownEnd(new WeaponEventData(GunHandler.Instance.backup_weapon, false, GunHandler.Instance.has_dual));
                }
                
            }
            else{
                timer -= Time.deltaTime;
                if(GunHandler.Instance.cooldown.self_key == self_key){

                    GameplayUI.instance.GetCooldownText().text = "COOLING DOWN";
                    GameplayUI.instance.GetCooldownText().color = cooldownGradient.Evaluate((float)timer/cooldown_time);
                    
                    GameplayUI.instance.GetCooldownSlider().maxValue = cooldown_time;
                    GameplayUI.instance.GetCooldownSlider().value = timer;
                    GameplayUI.instance.GetPrimaryCooldownSliderImage().color = cooldownGradient.Evaluate((float)timer/cooldown_time);
                }

                (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).maxValue = cooldown_time;
                (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).value = timer;
                GameplayUI.instance.GetSecondaryCooldownSliderImage().color = cooldownGradient.Evaluate((float)timer/cooldown_time);
            }
        }
        else{
            if(time_since_last_shot >= 1){
                if(count > 0){
                    count -= passive_cooldown * Time.deltaTime * (time_since_last_shot * time_since_last_shot);
                }
                else{
                    count = 0;
                }
                
            }
            if(GunHandler.Instance.cooldown.self_key == self_key){
                GameplayUI.instance.GetCooldownSlider().maxValue = count_max;
                GameplayUI.instance.GetCooldownSlider().value = count;

                GameplayUI.instance.GetPrimaryCooldownSliderImage().color = shootingGradient.Evaluate((float)count/count_max);
            }
            if(id == 0){ 
                GameplayUI.instance.GetCooldownText().text = GunHandler.Instance.primary_weapon.name.ToUpper(); 
                GameplayUI.instance.GetCooldownText().color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.primary_weapon.tier];
            }
            else if(id == 1){ 
                if(GunHandler.Instance.backup_weapon != null){
                    GameplayUI.instance.GetCooldownText().text = GunHandler.Instance.backup_weapon.name.ToUpper(); 
                    GameplayUI.instance.GetCooldownText().color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.backup_weapon.tier];
                }
                else{
                    GameplayUI.instance.GetCooldownText().color = Color.white;
                    GameplayUI.instance.GetCooldownText().text = "SECONDARY"; 
                }
            }

            GameplayUI.instance.GetSecondaryCooldownSliderImage().color = shootingGradient.Evaluate((float)count/count_max);
            (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).maxValue = count_max;
            (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).value = count;

            
        }

        if(shot){
            time_since_last_shot += Time.deltaTime;
        }
    }

    void TriggerCooldown(float set_time){
        count = 0;
        //GameplayUI.instance.GetCooldownSlider().wholeNumbers = false;
        GameplayUI.instance.GetCooldownSlider().maxValue = set_time;
        GameplayUI.instance.GetCooldownSlider().value = set_time;

        (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).maxValue = set_time;
        (id == 0 ? GameplayUI.instance.GetPrimaryCooldownSlider() : GameplayUI.instance.GetSecondaryCooldownSlider()).value = set_time;
        
        timer = set_time;
        cooling_down = true;
        //animator.Play("CoolingDown");
        if(id == 0){
            InventoryUIManager.Instance.primary_element.StartCooldown();
            KillboxEventSystem.TriggerCooldownBegin(new WeaponEventData(GunHandler.Instance.primary_weapon, true, GunHandler.Instance.has_dual));
        }
        else if(id == 1){
            InventoryUIManager.Instance.secondary_element.StartCooldown();
            KillboxEventSystem.TriggerCooldownBegin(new WeaponEventData(GunHandler.Instance.backup_weapon, false, GunHandler.Instance.has_dual));
        }

        animator.CrossFade("CoolingDown", 0.75f);

    }

    public void UpdateCooldownTime()
    {
        if(id == 0) {cooldown_time = GunHandler.Instance.primary_weapon.weapon.cooldown_time;}
        else if(id == 1) {
            if(GunHandler.Instance.backup_weapon != null){
                cooldown_time = GunHandler.Instance.backup_weapon.weapon.cooldown_time;}
            }
        //cooldown_time = 2.5f - (0.25f * buffsManager.buff_strength[0]);
    }

    public bool IsCoolingDown(){
        return cooling_down;
    }

    public void ResetCooldown(){
        count = 0;
    }

    public void AddCount(shooterScript2D caller, float value){
        count += value;
        time_since_last_shot = 0;
        animator.Play("Shot");

        if(!shot){
            shot = true;
        }
    }
    public void AddCount(KunaiShooterScript caller, float value){
        count += value;
        time_since_last_shot = 0;
        animator.Play("Shot");


        if(!shot){
            shot = true;
        }
    }

    public void CheckUpgrades(bool reset = false){

        if(reset){
            count_max = 100;
            GameplayUI.instance.GetCooldownSlider().maxValue = count_max;
            return;
        }

        if(UpgradesManager.Instance.current_levels[2] > 0){
            float value = UpgradesList.capacity.values[UpgradesManager.Instance.current_levels[2] - 1];
            count_max = (int)value;
            GameplayUI.instance.GetCooldownSlider().maxValue = count_max;
        }
        else{
            count_max = 100;
        }
    }

    public void OnRestartGame()
    {
        if(id == 1) {GameplayUI.instance.GetCooldownText().color = Color.white;}
    }
    public void OnReturnToMenu()
    {}

}
