using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    // The universal game manager across separate games
    // Should never be destroyed

    public static int[] personalBests { get; private set; }

    public int bestLevel, player_kills, tokens_used, guns_equipped, lvl_4_guns_retrieved, player_xp, equipment_index, theme_index, legacyPBInt, PBInt;
    public int bestEasyLevel {get; private set;}
    public int bestStandardLevel {get; private set;}
    public int bestExtremeLevel {get; private set;}
    public int ticketCount {get; private set;}
    public int playerXP {get; private set;}
    public int selfReviveTokens {get; private set;}
    public float time_played;
    public string Name, best_run_id;
    public static bool hasPlayedTutorial;
    public bool[] completed_challenges;

    public static KillBox main {get; private set;}
    public static Game currentGame {get; private set;}
    public static bool inGame {get {return currentGame != null;}}
    public string online_username, pb_run_id;

    void Awake(){
        if(main == null){main = this;}
        else if(main != this){Destroy(this);}
        
        DontDestroyOnLoad(gameObject);
        Application.runInBackground = false;
        personalBests = new int[3];
    }
    
    public static void Save()
    {
        SaveSystem.SavePlayer(main);
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

            pb_run_id = playerData.best_run_id;
            online_username = playerData.Name;
            hasPlayedTutorial = playerData.hasPlayedTutorial;
            
            player_kills = playerData.player_kills;
            tokens_used = playerData.tokens_used;

            equipment_index = playerData.equipment_index;

            //Temporary
            if(equipment_index == -1){ equipment_index = 1; }

            // theme_index = playerData.theme_index;
            // player_render.color = color_themes[theme_index];
            // gun_renderer.color = color_themes[theme_index];

            completed_challenges = new bool[17];
            completed_challenges = playerData.completed_challenges;
        }
        else{
            completed_challenges = new bool[17];
            equipment_index = -1;
            Save();

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


        // Not sure what this was for but could be important?...
        // if(
        //     PBInt != personalBests[0] &&
        //     PBInt != personalBests[1] &&
        //     PBInt != personalBests[2])
        // {

        //     int greatestIndex = 0;
        //     for(int i = 0; i < personalBests.Length; i++ ){
        //         if(personalBests[i] > personalBests[greatestIndex]){
        //             greatestIndex = i;
        //         }
        //     }
        //     PBInt = personalBests[greatestIndex];

        //     Save();
        // }

    }

    public static void StartNewGame(int difficulty){
        currentGame = new Game(difficulty);
    }

    public static void EndCurrentGame(){
        
        // Store game statistics and and save here before setting it to null
        
        currentGame = null;
    }

    public void OnPlayedTutorial(){
        hasPlayedTutorial = true;
        Save();
    }
}
