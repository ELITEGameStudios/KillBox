using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeFields {

    public static int player_xp {get; private set;}
    public static int kills {get; private set;}
    public static void UpdateKills(EnemyHealth enemy){kills += 1;}
    public static int max_upgrades {get; private set;}
    public static int[] guns_purchased {get; private set;}
    public static int guns_owned {get; private set;}
    public static int specialist_guns_owned {get; private set;}
    public static int support_guns_owned {get; private set;}
    public static int viable_best_round {get; private set;}
    public static int rounds_survived {get; private set;}
    public static int fire_rounds_survived {get; private set;}

    //0 = shard, 1=alpha triad, 2=cutter, 3=the one, 4=X
    public static int[] boss_kills {get; private set;}
    public static void UpdateBossKills(EnemyHealth hp, string boss_key)
    {
        switch (boss_key){
            case "SHARD":
                boss_kills[0]++;
                break;

            case "ALPHA TRIAD":
                boss_kills[1]++;
                break;

            case "CUTTER":
                boss_kills[2]++;
                break;

            case "THE ONE":
                boss_kills[3]++;
                break;

            case "X":
                boss_kills[4]++;
                break;

        }
        ChallengeLib.UpdateChallenge(ChallengeLib.HUNTER[1][0], boss_kills[0]);
        ChallengeLib.UpdateChallenge(ChallengeLib.HUNTER[1][1], boss_kills[1]);
        ChallengeLib.UpdateChallenge(ChallengeLib.HUNTER[1][2], boss_kills[2]);
        ChallengeLib.UpdateChallenge(ChallengeLib.HUNTER[1][3], boss_kills[3]);
        ChallengeLib.UpdateChallenge(ChallengeLib.HUNTER[1][4], boss_kills[4]);
    }

    //0=hp, 1=speed, 2=lifesteal, 3=capacity, 4=dual
    public static int[] upgrade_purchases{get; private set;}
    public static int[] upgrade_tier_purchases {get; private set;}

    public static bool[] wielded_specialists {get; private set;}

    public static bool[][] HUNTER_COMPLETED {get; private set;}
    public static bool[][] HUNTER_CLAIMED {get; private set;}

    public static bool[][] UPGRADES_COMPLETED {get; private set;}
    public static bool[][] UPGRADES_CLAIMED {get; private set;}

    public static bool[][] ARSENAL_COMPLETED {get; private set;}
    public static bool[][] ARSENAL_CLAIMED {get; private set;}

    public static bool[][] SURVIVOR_COMPLETED {get; private set;}
    public static bool[][] SURVIVOR_CLAIMED {get; private set;}

    public static bool[][] SPECIALIST_COMPLETED {get; private set;}
    public static bool[][] SPECIALIST_CLAIMED {get; private set;}

    public static bool[][] FEATURED_COMPLETED {get; private set;}
    public static bool[][] FEATURED_CLAIMED {get; private set;}

    public static void firstSavePrep(){
        player_xp = 0;
        guns_owned = 0;

        wielded_specialists = new bool[5];

        upgrade_purchases = new int[5];
        guns_purchased= new int[5];
        upgrade_tier_purchases = new int[4];
        boss_kills = new int[5];
        HUNTER_COMPLETED = new bool[][]{
            new bool[5],
            new bool[5]
        };
        HUNTER_CLAIMED = new bool[][]{
            new bool[5],
            new bool[5]
        };
        UPGRADES_COMPLETED = new bool[][]{
            new bool[5],
            new bool[5]
        };
        UPGRADES_CLAIMED = new bool[][]{
            new bool[5],
            new bool[5]
        };
        ARSENAL_COMPLETED = new bool[][]{
            new bool[5],
            new bool[3]
        };
        ARSENAL_CLAIMED = new bool[][]{
            new bool[5],
            new bool[3]
        };
        SURVIVOR_COMPLETED = new bool[][]{
            new bool[5],
            new bool[2]
        };
        SURVIVOR_CLAIMED = new bool[][]{
            new bool[5],
            new bool[2]
        };
        SPECIALIST_COMPLETED = new bool[][]{
            new bool[5],
            new bool[1]
        };
        SPECIALIST_CLAIMED = new bool[][]{
            new bool[5],
            new bool[1]
        };
        FEATURED_COMPLETED = new bool[][]{
            new bool[5],
            new bool[5]
        };
        FEATURED_CLAIMED = new bool[][]{
            new bool[5],
            new bool[5]
        };
    }

    public static void load(ChallengeData data){

        // firstSavePrep();
        //player_xp = data.player_xp;
        //HUNTER_COMPLETED = data.hunter_challenge_status;
        //HUNTER_CLAIMED = data.hunter_challenge_claimed;
        //UPGRADES_COMPLETED = data.upgrades_challenge_status;
        //UPGRADES_CLAIMED = data.upgrades_challenge_claimed;
        //ARSENAL_COMPLETED = data.arsenal_challenge_status;
        //ARSENAL_CLAIMED = data.arsenal_challenge_claimed;
        //SURVIVOR_COMPLETED = data.survivor_challenge_status;
        //SURVIVOR_CLAIMED = data.survivor_challenge_claimed;
        //SPECIALIST_COMPLETED = data.specialist_challenge_status;
        //SPECIALIST_CLAIMED = data.specialist_challenge_claimed;
//
        //kills = data.kills;
        //max_upgrades = data.max_upgrades;
        //guns_owned = 0;
        //specialist_guns_owned = data.specialist_guns_owned;
        //support_guns_owned = data.support_guns_owned;
        //viable_best_round = data.viable_best_round;
        //rounds_survived = data.rounds_survived;
        //fire_rounds_survived = data.fire_rounds_survived;
        //boss_kills = data.boss_kills;
        //upgrade_purchases = data.upgrade_purchases;
        //upgrade_tier_purchases = data.upgrade_tier_purchases;
        //wielded_specialists = data.wielded_specialists;
        //guns_purchased = data.guns_purchased;
        
        // AssignData();
    }

    public static void AssignData(){
        return;
        for (int a = 0; a < ChallengeLib.HUNTER.Length; a++) { 
            for (int i = 0; i < ChallengeLib.HUNTER[a].Length; i++) { ChallengeLib.HUNTER[a][i].InitData(a, i); };
        }

        for (int a = 0; a < ChallengeLib.UPGRADES.Length; a++) { 
            for (int i = 0; i < ChallengeLib.UPGRADES[a].Length; i++) { ChallengeLib.UPGRADES[a][i].InitData(a, i); };
        }

        for (int a = 0; a < ChallengeLib.ARSENAL.Length; a++) { 
            for (int i = 0; i < ChallengeLib.ARSENAL[a].Length; i++) { ChallengeLib.ARSENAL[a][i].InitData(a, i); };
        }

        for (int a = 0; a < ChallengeLib.SURVIVOR.Length; a++) { 
            for (int i = 0; i < ChallengeLib.SURVIVOR[a].Length; i++) { ChallengeLib.SURVIVOR[a][i].InitData(a, i); };
        }
        
        for (int a = 0; a < ChallengeLib.SPECIALIST.Length; a++) { 
            for (int i = 0; i < ChallengeLib.SPECIALIST[a].Length; i++) { ChallengeLib.SPECIALIST[a][i].InitData(a, i); };
        }
    }

    public static void CompleteChallenge(Challenge challenge){
        return;

        Debug.Log(challenge);
        switch (challenge.type){
            case "HUNTER":
                for (int a = 0; a < ChallengeLib.HUNTER.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.HUNTER[a].Length; i++) 
                    {
                        if(ChallengeLib.HUNTER[a][i] == challenge && HUNTER_COMPLETED[a][i] == false)
                        { 
                            ChallengeFields.HUNTER_COMPLETED[a][i] = true;
                            Debug.Log(ChallengeFields.HUNTER_COMPLETED[a][i]);
                            Debug.Log(ChallengeFields.kills);

                            player_xp += ChallengeLib.HUNTER[a][i].XP;
                            Debug.Log(player_xp);

                        }
                    };
                }
                break;
                
            case "UPGRADES":
                for (int a = 0; a < ChallengeLib.UPGRADES.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.UPGRADES[a].Length; i++) 
                    {
                        if(ChallengeLib.UPGRADES[a][i] == challenge && UPGRADES_COMPLETED[a][i] == false)
                        { 
                            UPGRADES_COMPLETED[a][i] = true;
                            player_xp += ChallengeLib.UPGRADES[a][i].XP;
                            Debug.Log(player_xp);
                        } 
                    };
                }
                break;
                
            case "ARSENAL":
                for (int a = 0; a < ChallengeLib.ARSENAL.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.ARSENAL[a].Length; i++) 
                    { 
                        if(ChallengeLib.ARSENAL[a][i] == challenge && ARSENAL_COMPLETED[a][i] == false)
                        { 
                            ARSENAL_COMPLETED[a][i] = true; 
                            player_xp += ChallengeLib.ARSENAL[a][i].XP;
                            Debug.Log(player_xp);
                        };
                    }
                }
                break;
                
            case "SURVIVOR":
                for (int a = 0; a < ChallengeLib.SURVIVOR.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.SURVIVOR[a].Length; i++) 
                    { 
                        if(ChallengeLib.SURVIVOR[a][i] == challenge && SURVIVOR_COMPLETED[a][i] == false)
                        { 
                            SURVIVOR_COMPLETED[a][i] = true;
                            player_xp += ChallengeLib.SURVIVOR[a][i].XP;
                            Debug.Log(player_xp);
                        } 
                    };
                }
                break;
                
            case "SPECIALIST":
                for (int a = 0; a < ChallengeLib.SPECIALIST.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.SPECIALIST[a].Length; i++) 
                    { 
                        if(ChallengeLib.SPECIALIST[a][i] == challenge && SPECIALIST_COMPLETED[a][i] == false)
                        { 
                            SPECIALIST_COMPLETED[a][i] = true;
                            player_xp += ChallengeLib.SPECIALIST[a][i].XP;
                            Debug.Log(player_xp);
                        } 
                    };
                }
                break;
        }
    }

    public static void UpdateUpgrades(ShopScript shopScript, Upgrade upgrade, int level)
    {
        return;
        level +=1 ;
        switch (upgrade.name){
            case "HEALTH":
                upgrade_purchases[0]++;

                if (upgrade_tier_purchases[0] < level){
                    upgrade_tier_purchases[0] = level;
                }
                break;
            case "SPEED":
                upgrade_purchases[1]++;

                if (upgrade_tier_purchases[1] < level){
                    upgrade_tier_purchases[1] = level;
                }
                break;
            case "LIFESTEAL":
                upgrade_purchases[2]++;

                if (upgrade_tier_purchases[2] < level){
                    upgrade_tier_purchases[2] = level;
                }
                break;
            case "CAPACITY":
                upgrade_purchases[3]++;

                if (upgrade_tier_purchases[3] < level){
                    upgrade_tier_purchases[3] = level;
                }
                break;
            case "DUAL":
                upgrade_purchases[4]++;
//
                //if (upgrade_tier_purchases[4] < level){
                //    upgrade_tier_purchases[4] = level;
                //}
                break;
        }

        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[1][0], upgrade_tier_purchases[0]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[1][1], upgrade_tier_purchases[1]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[1][2], upgrade_tier_purchases[2]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[1][3], upgrade_tier_purchases[3]);
        
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[0][0], upgrade_purchases[0]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[0][1], upgrade_purchases[1]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[0][2], upgrade_purchases[2]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[0][3], upgrade_purchases[3]);
        ChallengeLib.UpdateChallenge(ChallengeLib.UPGRADES[0][4], upgrade_purchases[4]);
    }

    public static void UpdateRound(GameManager gameManager, bool fire_round)
    {
        return;

        if(fire_round){
            fire_rounds_survived++;
        }
        else{
            rounds_survived++;
        }

        Debug.Log(rounds_survived);

        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[1][0], fire_rounds_survived);
        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[1][1], rounds_survived);
        
        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[0][0], gameManager.LvlCount);
        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[0][1], gameManager.LvlCount);
        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[0][2], gameManager.LvlCount);
        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[0][3], gameManager.LvlCount);
        ChallengeLib.UpdateChallenge(ChallengeLib.SURVIVOR[0][4], gameManager.LvlCount);
    }

    public static void UpdateArsenal(WeaponItem weapon)
    {

    //    guns_owned++;
    //    int tier = weapon.tier;
//
    //    if(GunHandler.Instance.owned_weapons.Count > 1){
    //        guns_purchased[tier]++;
    //    }
//
    //    if(tier < 5){
    //        ChallengeLib.UpdateChallenge(ChallengeLib.ARSENAL[0][tier], guns_purchased[tier]);
    //    }
    //    
    //    ChallengeLib.UpdateChallenge(ChallengeLib.ARSENAL[1][0], guns_owned);
    //    //ChallengeLib.UpdateChallenge(ChallengeLib.ARSENAL[1][1], rounds_survived);
        
    }
}

public class ChallengeLib{
    public static readonly Challenge[][] HUNTER = new Challenge[][]{
        new Challenge[]{
            new Challenge("HUNTER", "Kill Enemies", "KILLS", "50", 50, 25),
            new Challenge("HUNTER", "Kill Enemies", "KILLS", "250", 250, 100),
            new Challenge("HUNTER", "Kill Enemies", "KILLS", "1000", 1000, 500),
            new Challenge("HUNTER", "Kill Enemies", "KILLS", "5000", 5000, 2500),
            new Challenge("HUNTER", "Kill Enemies", "KILLS", "20000", 20000, 10000, true)
        },
        new Challenge[]{
            new Challenge("HUNTER", "Kill Bosses","BOSSES", "SHARD", 10, 1000),
            new Challenge("HUNTER", "Kill Bosses","BOSSES", "ALPHA-TRIAD", 5, 1000),
            new Challenge("HUNTER", "Kill Bosses","BOSSES", "CUTTER", 5, 1500),
            new Challenge("HUNTER", "Kill Bosses","BOSSES", "THE ONE", 2, 3500),
            new Challenge("HUNTER", "Kill Bosses","BOSSES", "X", 1, 10000,true)
        },
    };
    public static readonly Challenge[][] UPGRADES = new Challenge[][]{
        new Challenge[]{
            new Challenge("UPGRADES", "Purchase", "UPGRADES", "Health", 100, 1000),
            new Challenge("UPGRADES", "Purchase", "UPGRADES", "Speed", 100, 1000),
            new Challenge("UPGRADES", "Purchase", "UPGRADES", "Lifesteal", 50, 1000),
            new Challenge("UPGRADES", "Purchase", "UPGRADES", "Capacity", 75, 1000),
            new Challenge("UPGRADES", "Purchase", "UPGRADES", "Dual Wield", 10, 1000)
        },
        new Challenge[]{
            new Challenge("UPGRADES", "Purchase", "TIER", "Health max tier", 20, 5000),
            new Challenge("UPGRADES", "Purchase", "TIER", "Speed max tier", 6, 600),
            new Challenge("UPGRADES", "Purchase", "TIER", "Lifesteal max tier", 5, 1000),
            new Challenge("UPGRADES", "Purchase", "TIER", "Capacity max tier", 5, 750),
            new Challenge("UPGRADES", "Purchase", "TIER", "All max tiers in one game", 1, 10000, true)
        }
    };
    public static readonly Challenge[][] ARSENAL = new Challenge[][]{
        new Challenge[]{
            new Challenge("ARSENAL", "Obtain", "TIER","Tier 1 weapon", 20, 300),
            new Challenge("ARSENAL", "Obtain", "TIER","Tier 2 Weapon", 15, 500),
            new Challenge("ARSENAL", "Obtain", "TIER","Tier 3 Weapon", 10, 750),
            new Challenge("ARSENAL", "Obtain", "TIER","Tier 4 Weapon", 7, 1000),
            new Challenge("ARSENAL", "Obtain", "TIER", "GOLD weapon", 5, 4000)
        },

        new Challenge[]{
            new Challenge("ARSENAL", "Weapons in one game", "WEAPONS", "All standard weapons", 32, 5000),
            new Challenge("ARSENAL", "Weapons in one game", "WEAPONS", "All support weapons", 12, 7500),
            new Challenge("ARSENAL", "Weapons in one game", "WEAPONS", "All weapons", 49, 20000, true)
        }
    };
    public static readonly Challenge[][] SURVIVOR = new Challenge[][]{
        new Challenge[]{
            new Challenge("SURVIVOR", "Survive in one game", "ROUNDS", "5 Rounds", 5, 15),
            new Challenge("SURVIVOR", "Survive in one game", "ROUNDS", "20 Rounds", 20, 250),
            new Challenge("SURVIVOR", "Survive in one game", "ROUNDS", "40 Rounds", 40, 750),
            new Challenge("SURVIVOR", "Survive in one game", "ROUNDS", "60 Rounds", 60, 2500),
            new Challenge("SURVIVOR", "Survive in one game", "ROUNDS", "100 Rounds", 100, 10000,true),
        },
        new Challenge[]{
            new Challenge("SURVIVOR", "Survive overall", "ROUNDS", "25 fire Rounds", 25, 2500),
            new Challenge("SURVIVOR", "Survive overall", "ROUNDS", "1000 Rounds", 1000, 10000,true)
        }
    };
    public static readonly Challenge[][] SPECIALIST = new Challenge[][]{
        new Challenge[]{
            new Challenge("SPECIALIST", "Wield", "STEPS", "Kunai", 2, 1000),
            new Challenge("SPECIALIST", "Wield", "STEPS", "Chaos", 2, 1000),
            new Challenge("SPECIALIST", "Wield", "STEPS", "Prismatic Hyperwave", 1, 1000),
            new Challenge("SPECIALIST", "Wield", "STEPS", "Dracoscope", 3, 1000),
            new Challenge("SPECIALIST", "Wield", "STEPS", "Runic gun", 3, 1000)
        },
        new Challenge[]{
            new Challenge("SPECIALIST", "Wield", "WEAPONS", "All specialist weapons", 15, 10000,true)
        }
    };

    public static void UpdateChallengeValues(string type, string req_type, int value){
        return;
        List<Challenge> target_lists = GetTypes(type, req_type);

        for(int i = 0; i < target_lists.Count; i++){
            if(target_lists[i].completed){target_lists.RemoveAt(i);}
        }

        for(int i = 0; i < target_lists.Count; i++){
            if(target_lists[i].TestRequirement(value) == true)
            {
                ChallengeFields.CompleteChallenge(target_lists[i]);
            }
        }
    }
    public static void UpdateChallenge(Challenge challenge, int value){
        return;
        if(challenge.TestRequirement(value) == true)
        {
            ChallengeFields.CompleteChallenge(challenge);
        }
    }
    public static List<Challenge> GetTypes(string type, string req_type){
        return null;
        List<Challenge> return_list = new List<Challenge>();

        switch (type){

            case "HUNTER":
                for (int a = 0; a < ChallengeLib.HUNTER.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.HUNTER[a].Length; i++) 
                    { if(ChallengeLib.HUNTER[a][i].requirement_type == req_type) return_list.Add(ChallengeLib.HUNTER[a][i]); };
                }
                break;
                
            case "UPGRADES":
                for (int a = 0; a < ChallengeLib.UPGRADES.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.UPGRADES[a].Length; i++) 
                    { if(ChallengeLib.UPGRADES[a][i].requirement_type == req_type) return_list.Add(ChallengeLib.UPGRADES[a][i]); };
                }
                break;
                
            case "ARSENAL":
                for (int a = 0; a < ChallengeLib.ARSENAL.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.ARSENAL[a].Length; i++) 
                    { if(ChallengeLib.ARSENAL[a][i].requirement_type == req_type) return_list.Add(ChallengeLib.ARSENAL[a][i]); };
                }
                break;
                
            case "SURVIVOR":
                for (int a = 0; a < ChallengeLib.SURVIVOR.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.SURVIVOR[a].Length; i++) 
                    { if(ChallengeLib.SURVIVOR[a][i].requirement_type == req_type) return_list.Add(ChallengeLib.SURVIVOR[a][i]); };
                }
                break;
                
            case "SPECIALIST":
                for (int a = 0; a < ChallengeLib.SPECIALIST.Length; a++) { 
                    for (int i = 0; i < ChallengeLib.SPECIALIST[a].Length; i++) 
                    { if(ChallengeLib.SPECIALIST[a][i].requirement_type == req_type) return_list.Add(ChallengeLib.SPECIALIST[a][i]); };
                }
                break;
        }
        return return_list;
    }
}

public class Challenge
{
    public string type {get; private set;}
    public string subtype {get; private set;}
    public string description {get; private set;}
    public int stage {get; private set;}
    public int XP{get; private set;}
    public int requirement {get; private set;}
    public string requirement_type {get; private set;}
    public bool is_featured {get; private set;}

    public bool completed {get; private set;}
    public bool claimed {get; private set;}

    public Challenge(string _type, string _subtype, string _requirement_type, string _desctiption, int _requirement, int _XP, bool featured = false){
        type = _type;
        subtype = _subtype;
        description = _desctiption;
        requirement = _requirement;
        requirement_type = _requirement_type;
        is_featured = featured;
        XP = _XP;
    }

    public bool TestRequirement(int value){
        if(value >= requirement){ return true; }
        else{ return false; }
    }

    public void InitData(int index1, int index2){
        switch (type){
            case "HUNTER":
                completed = ChallengeFields.HUNTER_COMPLETED[index1][index2];
                claimed = ChallengeFields.HUNTER_CLAIMED[index1][index2];
                break;
            case "UPGRADES":
                completed = ChallengeFields.UPGRADES_COMPLETED[index1][index2];
                claimed = ChallengeFields.UPGRADES_CLAIMED[index1][index2];
                break;
            case "ARSENAL":
                completed = ChallengeFields.ARSENAL_COMPLETED[index1][index2];
                claimed = ChallengeFields.ARSENAL_CLAIMED[index1][index2];
                break;
            case "SURVIVOR":
                completed = ChallengeFields.SURVIVOR_COMPLETED[index1][index2];
                claimed = ChallengeFields.SURVIVOR_CLAIMED[index1][index2];
                break;
            case "SPECIALIST":
                completed = ChallengeFields.SPECIALIST_COMPLETED[index1][index2];
                claimed = ChallengeFields.SPECIALIST_CLAIMED[index1][index2];
                break;
        }
    }
}
