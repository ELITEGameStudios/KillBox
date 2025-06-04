using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeUiElement : MonoBehaviour
{
    [SerializeField]
    private int alpha_id, bravo_id, charlie_id, test;
    
    [SerializeField]
    private string slider_key, current_data;

    [SerializeField]
    private GameManager manager;


    [SerializeField]
    private Text type_txt, subtype_txt, desc_txt, requirement_txt, reward_txt, goal_txt, req_type_txt;

    [SerializeField]
    private Slider slider;
    
    [SerializeField]
    private GameObject reward_banner;

    // Start is called before the first frame update

    void Update(){
        UpdateUI();
    }

    void SliderValue(){


        switch (slider_key){
            case "kills":
                slider.value = ChallengeFields.kills;
                current_data = ChallengeFields.kills.ToString();
                break;
            case "bk0":
                slider.value = ChallengeFields.boss_kills[0];
                current_data = ChallengeFields.boss_kills[0].ToString();
                break;
            case "bk1":
                slider.value = ChallengeFields.boss_kills[1];
                current_data = ChallengeFields.boss_kills[1].ToString();
                break;
            case "bk2":
                slider.value = ChallengeFields.boss_kills[2];
                current_data = ChallengeFields.boss_kills[2].ToString();
                break;
            case "bk3":
                slider.value = ChallengeFields.boss_kills[3];
                current_data = ChallengeFields.boss_kills[3].ToString();
                break;
            case "bk4":
                slider.value = ChallengeFields.boss_kills[4];
                current_data = ChallengeFields.boss_kills[4].ToString();
                break;



            case "health upgrades":
                slider.value = ChallengeFields.upgrade_purchases[0];
                current_data = ChallengeFields.upgrade_purchases[0].ToString();
                break;
            case "health tiers":
                slider.value = ChallengeFields.upgrade_tier_purchases[0];
                current_data = ChallengeFields.upgrade_tier_purchases[0].ToString();
                break;
            case "speed upgrades":
                slider.value = ChallengeFields.upgrade_purchases[1];
                current_data = ChallengeFields.upgrade_purchases[1].ToString();
                break;
            case "speed tiers":
                slider.value = ChallengeFields.upgrade_tier_purchases[1];
                current_data = ChallengeFields.upgrade_tier_purchases[1].ToString();
                break;
            case "lifesteal upgrades":
                slider.value = ChallengeFields.upgrade_purchases[2];
                current_data = ChallengeFields.upgrade_purchases[2].ToString();
                break;
            case "lifesteal tiers":
                slider.value = ChallengeFields.upgrade_tier_purchases[2];
                current_data = ChallengeFields.upgrade_tier_purchases[2].ToString();
                break;
            case "capacity upgrades":
                slider.value = ChallengeFields.upgrade_purchases[3];
                current_data = ChallengeFields.upgrade_purchases[3].ToString();
                break;
            case "capacity tiers":
                slider.value = ChallengeFields.upgrade_tier_purchases[3];
                current_data = ChallengeFields.upgrade_tier_purchases[3].ToString();
                break;
            case "dual":
                slider.value = ChallengeFields.upgrade_purchases[4];
                current_data = ChallengeFields.upgrade_purchases[4].ToString();
                break;



            case "current_round":
                slider.value = manager.LvlCount;
                current_data = manager.LvlCount.ToString();
                break;

            case "fire_survived":
                slider.value = ChallengeFields.fire_rounds_survived;
                current_data = ChallengeFields.fire_rounds_survived.ToString();
                break;

            case "_survived":
                slider.value = ChallengeFields.rounds_survived;
                current_data = ChallengeFields.rounds_survived.ToString();
                break;
            //case "kills":
            //    slider.value = ChallengeFields.kills;
            //    break;
        }
    }

    public void UpdateUI()
    {
        Challenge challenge = ChallengeLib.HUNTER[bravo_id][charlie_id];;

        switch (alpha_id){
            case 0:
                challenge = ChallengeLib.HUNTER[bravo_id][charlie_id];
                subtype_txt.text = challenge.subtype + ChallengeFields.HUNTER_COMPLETED[bravo_id][charlie_id];
                break;
            case 1:
                challenge = ChallengeLib.UPGRADES[bravo_id][charlie_id];
                subtype_txt.text = challenge.subtype + ChallengeFields.UPGRADES_COMPLETED[bravo_id][charlie_id];
                break;
            case 2:
                challenge = ChallengeLib.ARSENAL[bravo_id][charlie_id];
                subtype_txt.text = challenge.subtype + ChallengeFields.ARSENAL_COMPLETED[bravo_id][charlie_id];
                break;
            case 3:
                challenge = ChallengeLib.SURVIVOR[bravo_id][charlie_id];
                subtype_txt.text = challenge.subtype + ChallengeFields.SURVIVOR_COMPLETED[bravo_id][charlie_id];
                break;
            case 4:
                challenge = ChallengeLib.SPECIALIST[bravo_id][charlie_id];
                subtype_txt.text = challenge.subtype + ChallengeFields.SPECIALIST_COMPLETED[bravo_id][charlie_id];
                break;
            //case 5:
            //    challenge = ChallengeLib.FEATURED[bravo_id][charlie_id];
            //    break;
        }

        type_txt.text = challenge.type;
        subtype_txt.text = challenge.subtype;
        desc_txt.text = challenge.description;
        SliderValue();
        requirement_txt.text = current_data +" | " + challenge.requirement.ToString(); //Will be changed after save system is developed
        reward_txt.text =challenge.XP.ToString() + " XP"; //Will be changed after save system is developed
        req_type_txt.text = challenge.requirement_type;

        slider.maxValue = challenge.requirement;
        if (challenge.completed && !challenge.claimed){
            if (!reward_banner.activeInHierarchy){
                reward_banner.SetActive(true);
            }
        }
         //Will be changed after save system is developed
    }
}
