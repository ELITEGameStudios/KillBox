using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KillboxWeaponClasses;

public class WeaponItemList : MonoBehaviour
{

    public List<Sprite> textures, inGameWeaponTextures;

    public readonly List<WeaponItem> weapon_items = new List<WeaponItem>
    {
        new WeaponItem("Pistol", WeaponLibrary.pistol, price_input: -1, tier_input: 0, owned_input: true, loopyDesc: "The gun where all KillBox runs begin with"),
        new WeaponItem("Combat Pistol", WeaponLibrary.combatPistol, price_input: 1, tier_input: 0, loopyDesc: "A fast firing pistol choice to get you though the first couple of rounds!"),
        new WeaponItem("Revolver", WeaponLibrary.revolver, price_input: 1, tier_input: 0, loopyDesc: "A slow but accurate and heavy hitting pistol choice to get you though the first couple of rounds!"),
        new WeaponItem("Speed Revolver", WeaponLibrary.speedRevolver, price_input: 15, tier_input: 2, loopyDesc: "A quick backup weapon made to get you out of tricky situations"),

        new WeaponItem("Light AR", WeaponLibrary.lightAR, price_input: 4, tier_input: 0, loopyDesc: "A well rounded weapon with decent fire rate and damage"),
        new WeaponItem("Tactical AR", WeaponLibrary.tacticalAR, price_input: 20, tier_input: 1, loopyDesc: "A strong and reliable weapon you can always count on!"),
        new WeaponItem("Heavy AR", WeaponLibrary.heavyAR, price_input: 30, tier_input: 2, loopyDesc: "A high damage weapon with a steady fire rate and can penetrate through walls"),
        new WeaponItem("Combat AR", WeaponLibrary.combatAR, price_input: 45, tier_input: 3, loopyDesc: "A fast firing and high damage output solution well rounded for the long run"),
        new WeaponItem("Golden AR", WeaponLibrary.goldenAR, price_input: 70, tier_input: 4, loopyDesc: "The ULTIMATE AR, will cut through anyting with insane proficiency!"),

        new WeaponItem("Light Burst AR", WeaponLibrary.lightBurstRifle, price_input: 4, tier_input: 0, loopyDesc: "A quick burst of 3 bullets with decent damage and aim"),
        new WeaponItem("Speedy Burst AR", WeaponLibrary.speedyBurstRifle, price_input: 20, tier_input: 1, loopyDesc: "Lightning speed bursts with even more strength and precision than the last!"),
        new WeaponItem("Heavy Burst AR", WeaponLibrary.heavyBurstRifle, price_input: 30, tier_input: 2),
        new WeaponItem("Combat Burst Rifle", WeaponLibrary.combatBurstRifle, price_input: 45, tier_input: 3, loopyDesc: "A fast firing demon built to mow through most of your problems in bursts of chaos" ),
        new WeaponItem("Golden Burst Rifle", WeaponLibrary.goldenBurstRifle, price_input: 80, tier_input: 4, loopyDesc: "Not even the toughest of enemies can withstand this absolute force of nature!"),

        new WeaponItem("Light SMG", WeaponLibrary.lightSmg, price_input: 3, tier_input: 0, loopyDesc: "A speedy choice with low damage bullets compensated with a fast rate of fire. Goodbye mini triads!" ),
        new WeaponItem("Tactical SMG", WeaponLibrary.tacticalSmg, price_input: 15, tier_input: 1, loopyDesc: "An upgraded version of the light SMG in every sense of the word. Very reliable in sustained close combat"),
        new WeaponItem("B.E.A.M SMG", WeaponLibrary.beamSmg, price_input: 15, tier_input: 2, loopyDesc: "This is a fun one..."),
        new WeaponItem("Combat SMG", WeaponLibrary.combatSmg, price_input: 32, tier_input: 3, loopyDesc: "Steady, speedy, and will fire through every enemy standing in front of you"),
        new WeaponItem("Golden SMG", WeaponLibrary.goldenSmg, price_input: 60, tier_input: 4, loopyDesc: "Through insane speed and reliability, the ultimate SMG is your greatest friend in the storm of a thousand foes"),

        new WeaponItem("Light Shotgun", WeaponLibrary.lightShotgun, price_input: 4, tier_input: 0, loopyDesc: "Shoots many bullets at once in a huge burst. Has a lot of kick and can obliterate the early rounds if used well! "),
        new WeaponItem("Tri-Shot", WeaponLibrary.triShotgun, price_input: 17, tier_input: 1, loopyDesc: "A much more optimized shotgun with 3 shells at a time, a moderate fire rate and good damage"),
        new WeaponItem("Penta-Shot", WeaponLibrary.pentaShotgun, price_input: 25, tier_input: 2, loopyDesc: "You thought 3 bullets wasnt enough with the tri-shotgun? Heres 5!"),
        new WeaponItem("Dual Action", WeaponLibrary.dualActionShotgun, price_input: 30, tier_input: 2, loopyDesc: "Quick Bursts of 2 deadly shots and massive reach"),
        new WeaponItem("Combat Shotgun", WeaponLibrary.combatShotgun, price_input: 29, tier_input: 3, loopyDesc: "Strong, fast firing bursts to cut through many crowds"),
        new WeaponItem("Golden Shotgun", WeaponLibrary.goldenShotgun, price_input: 70, tier_input: 4, loopyDesc: "Through the walls and plentiful crowds of foes, obliterate them all and let your problems vanish"),

        new WeaponItem("Light Marksman", WeaponLibrary.lightMarksman, price_input: 4, tier_input: 0, loopyDesc: "Fires precise, fast, and strong bullets made to deal with enemies swiftly one at a time"),
        new WeaponItem("Musket", WeaponLibrary.musket, price_input: 12, tier_input: 1, loopyDesc: "A still strong and precise bullet that goes through walls! "),
        new WeaponItem("Heavy Rifle", WeaponLibrary.heavyRifle, price_input: 27, tier_input: 2, loopyDesc: "Faster, stronger, and deadlier. Will absolutely tear through walls and enemies alike!"),
        new WeaponItem("Combat Marksman", WeaponLibrary.combatRifle, price_input: 28, tier_input: 3, loopyDesc: "This weapon lightly compensates heavy hitting force from each bullet for reliability and rate of fire"),
        new WeaponItem("Golden Marksman", WeaponLibrary.goldenRifle, price_input: 65, tier_input: 4, loopyDesc: "This gun simply goes through anything. All the walls and stacks of enemies at a time, there is no range or obstacle this gun cant reach"),

        new WeaponItem("Light Grenade Launcher", WeaponLibrary.lightGrenadeLauncher, price_input: 4, tier_input: 0, loopyDesc: "\"Im tired of all these bullets. Time to go with the heaviest solution of all\""),
        new WeaponItem("Double Launcher", WeaponLibrary.doubleLauncher, price_input: 15, tier_input: 1, loopyDesc:"You thought one explosion at a time was broken? Heres TWO!"),
        new WeaponItem("Tripwire Launcher", WeaponLibrary.tripwireLauncher, price_input: 25, tier_input: 2, loopyDesc: "The grenades are so slow the enemies trigger them before they hit anything! Maybe pair this with a shotgun..."),
        new WeaponItem("Burst Launcher", WeaponLibrary.burstLauncher, price_input: -1, tier_input: 3, attain_desc: "Found in Chests"),
        new WeaponItem("Heavy Launcher", WeaponLibrary.heavyLauncher,price_input: -1, tier_input: 4, attain_desc: "Found in Chests", loopyDesc: "The ultimate blast of sheer doom and despair"),
        
        new WeaponItem("KUNAIS", SpecialistLibrary.kunais_2, price_input: -1, tier_input: 5, attain_desc: "Find in chests", _special_key: "_kunai", loopyDesc: "The weapon of the one. The ultimate solution to all."),
        //new WeaponItem("KUNAIS", SpecialistLibrary.kunais, price_input: -1, tier_input: 4, attain_desc: "Craft with PRIME-RUNIC runes", _special_key: "_kunai"),

        // Support Weapons

        //new WeaponItem("Calm", SupportLibrary.slow_field_small, tier_input: 4, attain_desc: "Found in Chests"),
        new WeaponItem("Serenity", SupportLibrary.serenity, tier_input: 4, attain_desc: "Found in Chests", loopyDesc: "Fire a single projectile which casts a huge area slowing everything down. Made to be paired with a separate weapon"),
        // Reward Endgame Item
        //new WeaponItem("XOBLIX", WeaponLibrary.xoblix, tier_input: 5, attain_desc: "Only for the ELITE...")
        new WeaponItem("Golden Pistol", WeaponLibrary.goldenPistol, tier_input: 4, attain_desc: "Found in Chests", loopyDesc:"\"What? A Golden Pistol?! This is a JOKE!!\"... a joke that can shoot infinitely! Forget cooldown altogether, you dont need to worry about that anymore"),
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

    void Start(){
        InventoryUIManager.Instance.Initialize();
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
