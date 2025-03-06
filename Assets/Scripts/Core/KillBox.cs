using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    // The universal game manager across separate games
    // Should never be destroyed

    public int[] personalBests { get; private set; }

    public int bestLevel, player_kills, tokens_used, guns_equipped, lvl_4_guns_retrieved, player_xp, equipment_index, theme_index, legacyPBInt, PBInt;
    public int bestEasyLevel {get; private set;}
    public int bestStandardLevel {get; private set;}
    public int bestExtremeLevel {get; private set;}
    public int ticketCount {get; private set;}
    public int playerXP {get; private set;}
    public int selfReviveTokens {get; private set;}
    public float time_played;
    public string Name, best_run_id;
    public bool hasPlayedTutorial;
    public bool[] completed_challenges;

    public static KillBox main {get; private set;}


    void Awake(){
        if(main == null){main = this;}
        else if(main != this){Destroy(this);}
        personalBests = new int[3];
    }
    
    public void SavePlayer()
    {
        SaveSystem.SavePlayer(this);
        ChallengeSaveSystem.SaveChallenges();
    }

    public void LoadPlayer()
    {
        int playerXp = 0;

        if(SaveSystem.LoadPlayer() != null){
            PlayerData playerData = SaveSystem.LoadPlayer();
            PBInt = playerData.bestLevel;

            personalBests[0] = playerData.bestEasyLevel;
            personalBests[1] = playerData.bestStandardLevel;
            personalBests[2] = playerData.bestExtremeLevel;

            SetlegacyPB(playerData);

            pb_run_id = playerData.best_run_id;
            online_username = playerData.Name;
            playedTutorial = playerData.hasPlayedTutorial;
            
            player_kills = playerData.player_kills;
            tokens_used = playerData.tokens_used;

            equipment_index = playerData.equipment_index;

            //Temporary
            if(equipment_index == -1){ equipment_index = 1; }
            equipment_slider = equipment_slider_list[equipment_index];
            use_equipment_button = use_equipment_button_list[equipment_index];
            equipment_list[equipment_index].SetActive(true);
            equippedEquipmentDisplay[equipment_index].SetActive(true);

            theme_index = playerData.theme_index;

            player_render.color = color_themes[theme_index];
            gun_renderer.color = color_themes[theme_index];

            completed_challenges = new bool[17];
            completed_challenges = playerData.completed_challenges;


            if(completed_challenges[16]){
                for(int i = 0; i < ultramode_unlocked_objects.Length; i++){
                    ultramode_unlocked_objects[i].SetActive(false);
                }
            }
            else{
                for(int i = 0; i < ultramode_unlocked_objects.Length; i++){
                    ultramode_unlocked_objects[i].SetActive(true);
                }
            }

            if(completed_challenges[15]){
                for(int i = 0; i < grenade_launcher_objects.Length; i++){
                    grenade_launcher_objects[i].SetActive(true);
                }
            }
            else{
                for(int i = 0; i < grenade_launcher_objects.Length; i++){
                    grenade_launcher_objects[i].SetActive(false);
                }
            }


        }
        else{
            completed_challenges = new bool[17];
            equipment_index = -1;
            SavePlayer();

        }
        
        // ProgressionSystem.main.InitializeXP(playerXp, this);

        if(1 == 0){//(ChallengeSaveSystem.LoadChallenges() != null){
            ChallengeFields.load(ChallengeSaveSystem.LoadChallenges());
        }
        else{
            ChallengeFields.firstSavePrep();
            ChallengeSaveSystem.SaveChallenges();
            ChallengeFields.load(ChallengeSaveSystem.LoadChallenges());
            // Debug.Log("LLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLLL");
        }



        if(
            PBInt != personalBests[0] &&
            PBInt != personalBests[1] &&
            PBInt != personalBests[2])
        {
            legacyPB = PBInt;

            int greatestIndex = 0;
            for(int i = 0; i < personalBests.Length; i++ ){
                if(personalBests[i] > personalBests[greatestIndex]){
                    greatestIndex = i;
                }
            }
            PBInt = personalBests[greatestIndex];

            SavePlayer();
        }

    }
}
