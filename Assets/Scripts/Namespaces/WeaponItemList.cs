using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;

public class WeaponItemList : MonoBehaviour
{

    public List<Sprite> textures, inGameWeaponTextures;

    public readonly List<WeaponItem> weapon_items = new List<WeaponItem>
    {
        new WeaponItem("Pistol", WeaponLibrary.pistol, price_input: -1, tier_input: 0, owned_input: true),
        new WeaponItem("Combat Pistol", WeaponLibrary.combatPistol, price_input: 1, tier_input: 0),
        new WeaponItem("Revolver", WeaponLibrary.revolver, price_input: 1, tier_input: 0),
        new WeaponItem("Speed Revolver", WeaponLibrary.speedRevolver, price_input: 13, tier_input: 2),

        new WeaponItem("Light AR", WeaponLibrary.lightAR, price_input: 4, tier_input: 0),
        new WeaponItem("Tactical AR", WeaponLibrary.tacticalAR, price_input: 12, tier_input: 1),
        new WeaponItem("Heavy AR", WeaponLibrary.heavyAR, price_input: 22, tier_input: 2),
        new WeaponItem("Combat AR", WeaponLibrary.combatAR, price_input: 45, tier_input: 3),
        new WeaponItem("Golden AR", WeaponLibrary.goldenAR, price_input: 70, tier_input: 4),

        new WeaponItem("Light Burst AR", WeaponLibrary.lightBurstRifle, price_input: 3, tier_input: 0),
        new WeaponItem("Speedy Burst AR", WeaponLibrary.speedyBurstRifle, price_input: 10, tier_input: 1),
        new WeaponItem("Heavy Burst AR", WeaponLibrary.heavyBurstRifle, price_input: 20, tier_input: 2),
        new WeaponItem("Combat Burst Rifle", WeaponLibrary.combatBurstRifle, price_input: 45, tier_input: 3),
        new WeaponItem("Golden Burst Rifle", WeaponLibrary.goldenBurstRifle, price_input: 80, tier_input: 4),

        new WeaponItem("Light SMG", WeaponLibrary.lightSmg, price_input: 3, tier_input: 0),
        new WeaponItem("Tactical SMG", WeaponLibrary.tacticalSmg, price_input: 10, tier_input: 1),
        new WeaponItem("B.E.A.M SMG", WeaponLibrary.beamSmg, price_input: 13, tier_input: 2),
        new WeaponItem("Combat SMG", WeaponLibrary.combatSmg, price_input: 32, tier_input: 3),
        new WeaponItem("Golden SMG", WeaponLibrary.goldenSmg, price_input: 60, tier_input: 4),

        new WeaponItem("Light Shotgun", WeaponLibrary.lightShotgun, price_input: 4, tier_input: 0),
        new WeaponItem("Tri-Shot", WeaponLibrary.triShotgun, price_input: 10, tier_input: 1),
        new WeaponItem("Penta-Shot", WeaponLibrary.pentaShotgun, price_input: 18, tier_input: 2),
        new WeaponItem("Dual Action", WeaponLibrary.dualActionShotgun, price_input: 20, tier_input: 2),
        new WeaponItem("Combat Shotgun", WeaponLibrary.combatShotgun, price_input: 29, tier_input: 3),
        new WeaponItem("Golden Shotgun", WeaponLibrary.goldenShotgun, price_input: 70, tier_input: 4),

        new WeaponItem("Light Marksman", WeaponLibrary.lightMarksman, price_input: 4, tier_input: 0),
        new WeaponItem("Musket", WeaponLibrary.musket, price_input: 10, tier_input: 1),
        new WeaponItem("Heavy Rifle", WeaponLibrary.heavyRifle, price_input: 26, tier_input: 2),
        new WeaponItem("Combat Marksman", WeaponLibrary.combatRifle, price_input: 28, tier_input: 3),
        new WeaponItem("Golden Marksman", WeaponLibrary.goldenRifle, price_input: 65, tier_input: 4),

        new WeaponItem("Light Grenade Launcher", WeaponLibrary.lightGrenadeLauncher, price_input: 4, tier_input: 0),
        new WeaponItem("Double Launcher", WeaponLibrary.doubleLauncher, price_input: 8, tier_input: 1),
        new WeaponItem("Tripwire Launcher", WeaponLibrary.tripwireLauncher, price_input: 20, tier_input: 2),
        new WeaponItem("Burst Launcher", WeaponLibrary.burstLauncher, price_input: -1, tier_input: 3, attain_desc: "Found in Chests"),
        new WeaponItem("Heavy Launcher", WeaponLibrary.heavyLauncher,price_input: -1, tier_input: 4, attain_desc: "Found in Chests"),
        
        new WeaponItem("KUNAIS", SpecialistLibrary.kunais_2, price_input: -1, tier_input: 5, attain_desc: "Find in chests", _special_key: "_kunai"),
        //new WeaponItem("KUNAIS", SpecialistLibrary.kunais, price_input: -1, tier_input: 4, attain_desc: "Craft with PRIME-RUNIC runes", _special_key: "_kunai"),

        // Support Weapons

        //new WeaponItem("Calm", SupportLibrary.slow_field_small, tier_input: 4, attain_desc: "Found in Chests"),
        new WeaponItem("Serenity", SupportLibrary.slow_field_large, tier_input: 4, attain_desc: "Found in Chests"),
        // Reward Endgame Item
        //new WeaponItem("XOBLIX", WeaponLibrary.xoblix, tier_input: 5, attain_desc: "Only for the ELITE...")
        new WeaponItem("Golden Pistol", WeaponLibrary.goldenPistol, tier_input: 4, attain_desc: "Found in Chests"),
    };


    public static WeaponItemList Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        for(int i = 0; i < textures.Count; i++)
        {
            Instance.weapon_items[i].SetGraphic(textures[i]);
        }
    }

    public WeaponItem GetItem(string key)
    {
        for(int i = 0; i < Instance.weapon_items.Count; i++)
        {
            if(Instance.weapon_items[i].name == key)
            {
                return Instance.weapon_items[i];
            }
        }

        return null;
    }

    public List<WeaponItem> GetItemsOfTier(int tier)
    {
        List<WeaponItem> result = new List<WeaponItem>();

        for(int i = 0; i < Instance.weapon_items.Count; i++)
        {
            if(Instance.weapon_items[i].tier == tier)
            {
                result.Add(Instance.weapon_items[i]);
            }
        }

        return result;
    }
}
