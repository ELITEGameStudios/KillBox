using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviveToken : Booster{
    
    public ReviveToken()
    {
        _name = "Revive Token";
        _desc = "Use to revive yourself after dying. 3 Time max use in a game.";
        _tier = 1;
        _itemID = 0;
        _cooldown = 5;
        _cooldown_override = true;
    }

    public override void Function(){
        //doShit
    }
}
public class RandomWeapon : Booster{

    public RandomWeapon(int tier, int itemID, int cooldown)
    {
        _name = "Random Weapon";
        _desc = "Use to grant a random tier "+ (_tier+1).ToString() +" weapon!";
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
    }

    public override void Function(){
        
    }
}
public class FreeWeapon : Booster{

    public string _weapon_name {get; private set;}

    public WeaponItem _weapon {get; private set;}
    public int _weapon_type {get; private set;}

    public FreeWeapon(int tier, int itemID, int cooldown, string weapon_name, int weapon_type = 0)
    {
        _name = "Free " + weapon_name;
        _desc = "Grants a free "+ weapon_name;
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
        _weapon_name = weapon_name;
        _weapon = WeaponItemList.Instance.GetItem(_weapon_name);
    }

    public override void Function(){
        
    }
}

public class FreeSpecialist : Booster{

    public string _specialist {get; private set;}

    public FreeSpecialist(int tier, int itemID, int cooldown, string specialist)
    {
        _name = "Free Specialist";
        _desc = "Grants "+ specialist + "for free!";
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
        _specialist = specialist;
    }

    public override void Function(){
        
    }
}
public class TokenGrant : Booster{

    public int _tokens {get; private set;}
    public TokenGrant(int tier, int itemID, int cooldown, int tokens)
    {
        _name = "Token Boost";
        _desc = "Grants "+ tokens.ToString() +" Tokens!";
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
        _tokens = tokens;
    }

    public override void Function(){
        
    }
}
public class RoundSkip : Booster{

    public int _rounds {get; private set;}
    public int _max_round {get; private set;}
    public RoundSkip(int tier, int itemID, int cooldown, int rounds, int max_round)
    {
        _name = "Round Skip";
        _desc = "Skips "+ rounds.ToString() + " Rounds! Valid until round " + max_round.ToString();
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
        _rounds = rounds;
        _max_round = max_round;
    }
    

    public override void Function(){
        
    }
}

public class HeadStart : Booster{

    public int _rounds {get; private set;}
    public int _max_round {get; private set;}
    public HeadStart(int tier, int itemID, int cooldown, int rounds, int max_round)
    {
        _name = "Head Start";
        _desc = "Gain a "+ rounds.ToString() + " round head start! with 10 tokens and a random tier " + (tier+1).ToString() + " Weapon!";
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
        _rounds = rounds;
        _max_round = max_round;
    }
    

    public override void Function(){
        
    }
}
public class QuickCharge : Booster{

    public int _max_round {get; private set;}
    public QuickCharge(int tier, int itemID, int cooldown, int max_round)
    {
        _name = "Round Skip";
        _desc = "Instantly charges EQUIPMENT! Valid until round " + max_round.ToString();
        _tier = tier;
        _itemID = itemID;
        _cooldown = cooldown;
        _cooldown_override = false;
        _max_round = max_round;
    }

    public override void Function(){
        
    }
}

public static class BoosterTypes{
    public static readonly ReviveToken revive_token = new ReviveToken();
    public static readonly RandomWeapon random_weapon_tier_0= new RandomWeapon(0, 1, 5);
    public static readonly RandomWeapon random_weapon_tier_1= new RandomWeapon(1, 2, 10);
    public static readonly RandomWeapon random_weapon_tier_2= new RandomWeapon(2, 3, 15);
    public static readonly RandomWeapon random_weapon_tier_3= new RandomWeapon(3, 4, 25);
    public static readonly RandomWeapon random_weapon_tier_4= new RandomWeapon(4, 5, 35);
    public static readonly RandomWeapon random_specialist_tier_5= new RandomWeapon(4, 6, 35);
    public static readonly TokenGrant token_grant_0 = new TokenGrant(0, 7, 5, 3);
    public static readonly TokenGrant token_grant_1 = new TokenGrant(1, 8, 8, 5);
    public static readonly TokenGrant token_grant_2 = new TokenGrant(2, 9, 16, 12);
    public static readonly TokenGrant token_grant_3 = new TokenGrant(3, 10, 22, 15);
    public static readonly TokenGrant token_grant_4 = new TokenGrant(4, 11, 30, 25);
    public static readonly RoundSkip round_skip_0 = new RoundSkip(0, 12, 3, 1, -1);
    public static readonly RoundSkip round_skip_1 = new RoundSkip(1, 13, 6, 3, -1);
    public static readonly RoundSkip round_skip_2 = new RoundSkip(1, 14, 12, 6, -1);
    public static readonly RoundSkip round_skip_3 = new RoundSkip(1, 15, 10, 10, -1);
    public static readonly RoundSkip round_skip_4 = new RoundSkip(1, 16, 20, 20, -1);
    public static readonly RoundSkip head_start_0= new RoundSkip(1, 17, 10, 10, 2);
    public static readonly RoundSkip head_start_1= new RoundSkip(2, 18, 10, 20, 2);
    
    
    public static readonly FreeWeapon free_light_ar= new FreeWeapon(0, 19, 10, "Light AR", 2);
    public static readonly FreeWeapon free_tactical_ar= new FreeWeapon(1, 20, 10, "Tactical AR", 2);
    public static readonly FreeWeapon free_heavy_ar= new FreeWeapon(2, 21, 10, "Heavy AR", 2);
    public static readonly FreeWeapon free_combat_ar= new FreeWeapon(3, 22, 10, "Combat AR", 2);
    public static readonly FreeWeapon free_golden_ar= new FreeWeapon(4, 23, 10, "Golden AR", 2);
        //new WeaponItem("Pistol", WeaponLibrary.pistol, price_input: -1, tier_input: 0, owned_input: true),
        //new WeaponItem("Combat Pistol", WeaponLibrary.combatPistol, price_input: 1, tier_input: 0),
//
        //new WeaponItem("Light AR", WeaponLibrary.lightAR, price_input: 2, tier_input: 0),
        //new WeaponItem("Tactical AR", WeaponLibrary.tacticalAR, price_input: 10, tier_input: 1),
        //new WeaponItem("Heavy AR", WeaponLibrary.heavyAR, price_input: 18, tier_input: 2),
        //new WeaponItem("Combat AR", WeaponLibrary.combatAR, price_input: 22, tier_input: 3),
        //new WeaponItem("Golden AR", WeaponLibrary.goldenAR, price_input: 40, tier_input: 4),
//
        //new WeaponItem("Light Burst AR", WeaponLibrary.lightBurstRifle, price_input: 4, tier_input: 0),
        //new WeaponItem("Speedy Burst AR", WeaponLibrary.speedyBurstRifle, price_input: 9, tier_input: 1),
        //new WeaponItem("Heavy Burst AR", WeaponLibrary.heavyBurstRifle, price_input: 20, tier_input: 3),
        //new WeaponItem("Combat Burst Rifle", WeaponLibrary.combatBurstRifle, price_input: 25, tier_input: 3),
        //new WeaponItem("Golden Burst Rifle", WeaponLibrary.goldenBurstRifle, price_input: 40, tier_input: 4),
//
        //new WeaponItem("Light SMG", WeaponLibrary.lightSmg, price_input: 3, tier_input: 0),
        //new WeaponItem("Tactical SMG", WeaponLibrary.tacticalSmg, price_input: 8, tier_input: 1),
        //new WeaponItem("B.E.A.M SMG", WeaponLibrary.beamSmg, price_input: 12, tier_input: 2),
        //new WeaponItem("Combat SMG", WeaponLibrary.combatSmg, price_input: 19, tier_input: 3),
        //new WeaponItem("Golden SMG", WeaponLibrary.goldenSmg, price_input: 30, tier_input: 4),
//
        //new WeaponItem("Light Shotgun", WeaponLibrary.lightShotgun, price_input: 2, tier_input: 0),
        //new WeaponItem("Tri-Shot", WeaponLibrary.triShotgun, price_input: 7, tier_input: 1),
        //new WeaponItem("Penta-Shot", WeaponLibrary.pentaShotgun, price_input: 12, tier_input: 2),
        //new WeaponItem("Dual Action", WeaponLibrary.dualActionShotgun, price_input: 13, tier_input: 2),
        //new WeaponItem("Combat Shotgun", WeaponLibrary.combatShotgun, price_input: 23, tier_input: 3),
        //new WeaponItem("Golden Shotgun", WeaponLibrary.goldenShotgun, price_input: 39, tier_input: 4),
//
        //new WeaponItem("Light Marksman", WeaponLibrary.lightMarksman, price_input: 4, tier_input: 0),
        //new WeaponItem("Double Rifle", WeaponLibrary.doubleRifle, price_input: 8, tier_input: 1),
        //new WeaponItem("Heavy Rifle", WeaponLibrary.heavyRifle, price_input: 20, tier_input: 2),
        //new WeaponItem("Combat Marksman", WeaponLibrary.combatRifle, price_input: 22, tier_input: 3),
        //new WeaponItem("Golden Marksman", WeaponLibrary.goldenRifle, price_input: 40, tier_input: 4),
//
        //new WeaponItem("Light Grenade Launcher", WeaponLibrary.lightGrenadeLauncher, price_input: 4, tier_input: 0),
        //new WeaponItem("Double Launcher", WeaponLibrary.doubleLauncher, price_input: 13, tier_input: 1),
        //new WeaponItem("Tripwire Launcher", WeaponLibrary.tripwireLauncher, price_input: -1, tier_input: 2, attain_desc: "Found in Chests"),
        //new WeaponItem("Burst Launcher", WeaponLibrary.burstLauncher, price_input: -1, tier_input: 3, attain_desc: "Found in Chests"),
        //new WeaponItem("Heavy Launcher", WeaponLibrary.heavyLauncher,price_input: -1, tier_input: 4, attain_desc: "Found in Chests"),
        //// Support Weapons
        //new WeaponItem("Serenity", SupportLibrary.slow_field_large, tier_input: 4, attain_desc: "Found in Chests")

        // Reward Endgame Item
        //new WeaponItem("Serenity", SupportLibrary.slow_field_large, tier_input: 4, attain_desc: "Found in Chests")
}