using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ChallengeData
{

    public int player_xp {get; private set;}
    public int kills {get; private set;}
    public int max_upgrades {get; private set;}
    public int guns_owned {get; private set;}
    public int specialist_guns_owned {get; private set;}
    public int support_guns_owned {get; private set;}
    public int viable_best_round {get; private set;}
    public int rounds_survived {get; private set;}
    public int fire_rounds_survived {get; private set;}

    //0 = shard, 1=alpha triad, 2=cutter, 3=the one, 4=X
    public int[] boss_kills {get; private set;}
    public int[] guns_purchased {get; private set;}

    //0=hp, 1=speed, 2=lifesteal, 3=capacity, 4=dual
    public int[] upgrade_purchases{get; private set;}
    public int[] upgrade_tier_purchases {get; private set;}
    public bool[] wielded_specialists {get; private set;}



    public bool[][] hunter_challenge_status {get; private set;}
    public bool[][] hunter_challenge_claimed {get; private set;}

    public bool[][] upgrades_challenge_status {get; private set;}
    public bool[][] upgrades_challenge_claimed {get; private set;}

    public bool[][] arsenal_challenge_status {get; private set;}
    public bool[][] arsenal_challenge_claimed {get; private set;}

    public bool[][] survivor_challenge_status {get; private set;}
    public bool[][] survivor_challenge_claimed {get; private set;}

    public bool[][] specialist_challenge_status {get; private set;}
    public bool[][] specialist_challenge_claimed {get; private set;}

    public ChallengeData(){

        player_xp = ChallengeFields.player_xp;

        hunter_challenge_status = ChallengeFields.HUNTER_COMPLETED;
        hunter_challenge_claimed = ChallengeFields.HUNTER_CLAIMED;
        upgrades_challenge_status = ChallengeFields.UPGRADES_COMPLETED;
        upgrades_challenge_claimed = ChallengeFields.UPGRADES_CLAIMED;
        arsenal_challenge_status = ChallengeFields.ARSENAL_COMPLETED;
        arsenal_challenge_claimed = ChallengeFields.ARSENAL_CLAIMED;
        survivor_challenge_status = ChallengeFields.SURVIVOR_COMPLETED;
        survivor_challenge_claimed = ChallengeFields.SURVIVOR_CLAIMED;
        specialist_challenge_status = ChallengeFields.SPECIALIST_COMPLETED;
        specialist_challenge_claimed = ChallengeFields.SPECIALIST_CLAIMED;

        
        if(ChallengeFields.guns_owned > guns_owned)
            guns_owned = ChallengeFields.guns_owned;
        if(guns_owned == null)
            guns_owned = 0;

        kills = ChallengeFields.kills;
        max_upgrades = ChallengeFields.max_upgrades;
        specialist_guns_owned = ChallengeFields.specialist_guns_owned;
        support_guns_owned = ChallengeFields.support_guns_owned;
        viable_best_round = ChallengeFields.viable_best_round;
        rounds_survived = ChallengeFields.rounds_survived;
        fire_rounds_survived = ChallengeFields.fire_rounds_survived;
        boss_kills = ChallengeFields.boss_kills;
        upgrade_purchases = ChallengeFields.upgrade_purchases;
        upgrade_tier_purchases = ChallengeFields.upgrade_tier_purchases;
        wielded_specialists = ChallengeFields.wielded_specialists;
        guns_purchased = ChallengeFields.guns_purchased;


        //= data.FEATURED_COMPLETED;
        //= data.FEATURED_CLAIMED;
    }
}
