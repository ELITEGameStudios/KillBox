using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KillboxWeaponClasses
{
    public class Weapon
    {
        public readonly float fire_rate, cooldown_units, velocity, spread, range, burst_interval, cooldown_time;
        public readonly int damage, bullets_per_shot, burst_quantity, pool, penetration, support_type;

        public readonly bool burst, is_support, launcher, uniform, misc;

        public Weapon(
            float fr, float cooldown, float vel, float spr, float rnge, int dmg, int bps = 1, 
            bool burst_input = false, int burst_quantity_input = 3, float burst_interval_input = 0.33f, 
            int pool_input = 0, int penetration_input = 0, bool _is_support = false, int _support_type = 0, float _cooldown_time = 2.5f, bool _launcher = false, bool _uniform = false, bool _misc = false)
        {
            fire_rate = fr;
            cooldown_units = cooldown;
            velocity = vel;
            spread = spr;
            range = rnge;
            damage = dmg;
            bullets_per_shot = bps;
            burst = burst_input;
            burst_quantity = burst_quantity_input;
            burst_interval = burst_interval_input;
            
            pool = pool_input; //Use only for special Weapons
            penetration = penetration_input; //Use only for piercing weapons

            is_support = _is_support;
            support_type = _support_type;
            cooldown_time = _cooldown_time;
            launcher = _launcher;
            uniform = _uniform;
            misc = _misc;
        }
    }



    class WeaponLibrary
    {
        public static readonly Weapon pistol = new Weapon(0.2f, 10, 150, 15, 1, 25);
        public static readonly Weapon combatPistol = new Weapon(0.08f, 10, 120, 10, 1, 20);
        public static readonly Weapon revolver = new Weapon(0.3f, 20, 160, 5, 0.7f, 50);
        public static readonly Weapon speedRevolver = new Weapon(0.1f, 18, 170, 20, 0.7f, 150, penetration_input: 1);
        // public static readonly Weapon goldenPistol = new Weapon(0.17f, 7, 180, 15, 1.5f, 160);
        public static readonly Weapon goldenPistol = new Weapon(0.17f, 0, 180, 15, 1.5f, 100);
        //public static readonly Weapon devils_blaster = new Weapon(0.2f, 5, 150, 20, 1, 40);

        public static readonly Weapon lightAR = new Weapon(0.15f, 5, 120, 10, 1, 45);
        public static readonly Weapon tacticalAR = new Weapon(0.1f, 3.13f, 150, 15, 1, 80);
        public static readonly Weapon heavyAR = new Weapon(0.1f, 5f, 175, 4, 1, 140, penetration_input: 1);
        public static readonly Weapon combatAR = new Weapon(0.15f, 3.13f, 200, 15, 1, 160, penetration_input: 1);
        public static readonly Weapon goldenAR = new Weapon(0.1f, 2f, 250, 10, 1, 200, penetration_input: 2);

        public static readonly Weapon lightBurstRifle = new Weapon(0.12f, 6, 150, 15, 1, 55, burst_input: true);
        public static readonly Weapon speedyBurstRifle = new Weapon(0.05f, 4, 150, 20, 1, 60, burst_input: true, burst_interval_input: 0.25f);
        public static readonly Weapon heavyBurstRifle = new Weapon(0.08f, 5, 200, 10, 1, 115, burst_input: true);
        public static readonly Weapon combatBurstRifle = new Weapon(0.05f, 2, 200, 20, 1, 130, burst_input: true, burst_quantity_input: 5, burst_interval_input: 0.25f);
        public static readonly Weapon goldenBurstRifle = new Weapon(0.01f, 2, 220, 10, 1, 200, burst_input: true, burst_quantity_input: 5, burst_interval_input: 0.2f);

        public static readonly Weapon lightSmg = new Weapon(0.1f, 3, 120, 20, 0.5f, 20);
        public static readonly Weapon tacticalSmg = new Weapon(0.07f, 3, 120, 20, 0.5f, 35);
        public static readonly Weapon beamSmg = new Weapon(0.01f, 0.75f, 125, 0, 0.5f, 20);
        public static readonly Weapon combatSmg = new Weapon(0.04f, 1.5f, 135, 25, 0.30f, 70);
        public static readonly Weapon goldenSmg = new Weapon(0.05f, 1.5f, 200, 35, 0.8f, 100);

        public static readonly Weapon lightShotgun = new Weapon(0.6f, 3f, 75, 50, 0.7f, 25, bps: 8);
        public static readonly Weapon triShotgun = new Weapon(0.2f, 5f, 135, 30, 0.5f, 50, bps: 3, _uniform: true);
        public static readonly Weapon pentaShotgun = new Weapon(0.25f, 2, 150, 25, 0.5f, 50, bps: 5);
        public static readonly Weapon dualActionShotgun = new Weapon(0.1f, 1.5f, 150, 50, 0.5f, 50, bps: 8, burst_input: true, burst_quantity_input: 2, burst_interval_input: 0.33f);
        public static readonly Weapon combatShotgun = new Weapon(0.3f, 0.7f, 150, 50, 0.3f, 45, bps: 12);
        public static readonly Weapon goldenShotgun = new Weapon(0.4f, 0.5f, 150, 30, 0.9f, 75, bps: 15, penetration_input: 1);

        public static readonly Weapon lightMarksman = new Weapon(0.5f, 10f, 170, 0, 2, 100);
        public static readonly Weapon musket = new Weapon(0.5f, 10f, 230, 0, 3, 100, bps: 1, penetration_input: 5);
        public static readonly Weapon heavyRifle = new Weapon(0.4f, 7.5f, 170, 0, 2, 300, penetration_input: 2);
        public static readonly Weapon combatRifle = new Weapon(0.25f, 6f, 230, 0, 2, 200, penetration_input: 1);
        public static readonly Weapon goldenRifle = new Weapon(0.5f, 5.5f, 300, 0, 2, 600, penetration_input: 4);

        public static readonly Weapon lightGrenadeLauncher = new Weapon(0.75f, 25f, 120, 10, 5f, 150, pool_input: 10);
        public static readonly Weapon doubleLauncher = new Weapon(0.75f, 20f, 140, 10, 5f, 300, bps: 2, pool_input:10);
        public static readonly Weapon tripwireLauncher = new Weapon(0.2f, 2f, 3, 50, 15f, 75, bps: 6, pool_input: 10);
        public static readonly Weapon burstLauncher = new Weapon(0.1f, 10f, 150, 20, 10f, 350, burst_input: true, burst_quantity_input: 3, burst_interval_input: 0.5f, pool_input: 10);
        public static readonly Weapon heavyLauncher = new Weapon(0.5f, 20f, 150, 20, 10f, 1000, pool_input: 10);
        public static readonly Weapon goldenLauncher = new Weapon(0.1f, 1f, 150, 20, 10f, 1000, burst_input: true, burst_quantity_input: 5, burst_interval_input: 0.5f, pool_input: 10); // Shouldnt hurt you on explosion

        public static readonly Weapon xoblix = new Weapon(0.05f, 2f, 250, 20, 1, 200, penetration_input: 1);
    }

    class SupportLibrary
    {
        //These weapons will get their own class type to account for cooldown, projectile, etc

        //Stats displayed are irrelavent
        public static readonly Weapon slow_field_small = new Weapon(10f, 5f, 25, 30, 10f, 1, _is_support: true, _support_type: 0); //CALM, slows enemies within 10f radius by * 0.5f
        public static readonly Weapon slow_field_large = new Weapon(0.1f, 500f, 100, 0, 2f, 5, _is_support: true, _support_type: 0, _cooldown_time: 10f); //SERENITY, slows enemies within 20f radius by * 0.2f

        public static readonly Weapon health_pulse_small = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //HELPING_HAND, heals 200hp for the player in a 5f area every second for 8 pulses
        public static readonly Weapon health_pulse_large = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //LIFE_SUPPORT, heals 200hp for the player in a 5f area every second for 8 pulses

        public static readonly Weapon radiation_pulse_small = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //CRIPPLER, Deals DOT of 150 over a period of 4 seconds in a radius of 7f
        public static readonly Weapon radiation_pulse_large = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //DECIMATOR, Deals DOT of 250 over a period of 4 seconds in a radius of 12f

        public static readonly Weapon weaken_pulse_small = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //WEAKEN, Weakens all incoming attacks by 50% to the player in the 7f zone
        public static readonly Weapon weaken_pulse_large = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //HUMALIATION, Weakens all incoming attacks by 80% to the player in the 12f zone

        public static readonly Weapon shock_pulse_small = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //PARALYSUS, fires one electric current(with cool dash effect) that paralyses enemies for 3 seconds
        public static readonly Weapon shock_pulse_large = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //ELECTROSHOCK, fires two electric current(with cool dash effect) that paralyses enemies for 5 seconds

        public static readonly Weapon enhancer_pulse_small = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //ENHANCER, 1.5* players fire rate, doubles cooldown tolerance (100 -> 200), in a radius of 5
        public static readonly Weapon enhancer_pulse_large = new Weapon(10f, 5f, 25, 30, 10f, 0, _is_support: true, _support_type: 0); //REAPER,   Doubles players fire rate, triples cooldown tolerance (100 -> 300), in a radius of 10
    }

    class SpecialistLibrary
    {
        //Will also get its own unique class, seperate from weapon class entirely

        //Stats displayed are irrelavent
        public static readonly Weapon chaos = new Weapon(10f, 5f, 25, 30, 10f, 0, pool_input: 9); //CHAOS, cool black hole
        public static readonly Weapon kunais = new Weapon(0.75f, 5f, 75, 75, 0.75f, 0, bps:5,  pool_input: 13, _uniform: true, _misc: true, penetration_input: 5); //KUNAIS, 3 functions. trapper function, throwable, boomerang piercer
        public static readonly Weapon kunais_2 = new Weapon(0.33f, 0f, 25, 0, 2f, 0,  pool_input: 14, _misc: true, penetration_input: 5); //KUNAIS, 3 functions. trapper function, throwable, boomerang piercer
        public static readonly Weapon runic_gun = new Weapon(10f, 5f, 25, 30, 10f, 0, pool_input: 9); //RUNIC GUN, random runes do different things
        public static readonly Weapon prismatic_hyperwave= new Weapon(10f, 5f, 25, 30, 10f, 0, pool_input: 9); //prismatic_hyperwave, laser deflecting off walls
        public static readonly Weapon dracoscope = new Weapon(10f, 5f, 25, 30, 10f, 0, pool_input: 9); //dracoscope, hehe
        public static readonly Weapon grass = new Weapon(10f, 5f, 25, 30, 10f, 0, pool_input: 9); //grass, you dont want to touch this one.
    }
}
