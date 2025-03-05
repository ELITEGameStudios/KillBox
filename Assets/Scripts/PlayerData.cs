using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int bestLevel, player_kills, tokens_used, guns_equipped, lvl_4_guns_retrieved, player_xp, equipment_index, theme_index, legacyPBInt;
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

    public PlayerData (GameManager data){
        bestLevel = data.PBInt;
        Name = data.online_username;
        best_run_id = data.pb_run_id;
        hasPlayedTutorial = data.playedTutorial;

        player_kills = data.player_kills;
        tokens_used = data.tokens_used;
        guns_equipped = data.guns_equipped;
        lvl_4_guns_retrieved = data.lvl_4_guns_retrieved;
        //player_xp = data.player_xp;
        completed_challenges = new bool[17];
        completed_challenges = data.completed_challenges;
        equipment_index = data.equipment_index;
        theme_index = data.Theme;

        bestEasyLevel = data.personalBests[0];
        bestStandardLevel = data.personalBests[1];
        bestExtremeLevel = data.personalBests[2];

        selfReviveTokens = SelfReviveTokenManager.selfResTokens;
        playerXP = ProgressionSystem.playerXp;
        
        legacyPBInt = data.GetlegacyPB(this);
        
    }
}
