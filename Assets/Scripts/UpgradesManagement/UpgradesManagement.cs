using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Upgrade
{
    public readonly int max_level, current_level;
    public readonly float[][] values;
    public readonly float[][] defaultValues; // [statIndex][difficulty of game]
    public readonly int[] costs;

    public readonly string name;
    public readonly string[] stat_names;

    public Upgrade(float[][] values_input, int[] costs_input, float[][] defaultValues, string _name, params string[] stat_names)
    {
        values = values_input;
        costs = costs_input;
        max_level = values[0].Length;
        this.stat_names = stat_names;
        this.defaultValues = defaultValues;
        name = _name;
    }

    public int Transaction(int request, int level, ShopScript caller)
    {
        if (request >= costs[level])
        {
            request -= costs[level];

            return request;
        }
        else
        {
            return -1;
        }
    }

    public bool Compare(int request, int level)
    {
        if (request >= costs[level])
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int CostDifference(int request, int level)
    {
        return costs[level] - request;
    }

}

public class UpgradesList
{
    
    public enum SpecialUpgradeEnum
    {
        NONE,
        DUAL_WIELD,
        GOLDEN,
        NECRO,
        MASTERY,
    }

    static public readonly Upgrade speed = new Upgrade(
        values_input: new float[][]{
            new float[]{5.5f, 5.75f, 6, 6.25f, 6.5f, 7f},
            new float[]{5,    4,     4, 3,     3,    2},
        },
        costs_input: new int[] { 1, 3, 8, 15, 20, 25, 30 },
        defaultValues: new float[][]{
            new float[]{5, 5, 5},
            new float[]{5, 5, 5},
        },
        _name: "SPEED",
        "MOVEMENT SPEED",
        "DASH COOLDOWN"

    );

    static public readonly Upgrade health = new Upgrade(
        values_input : new float[][]{
            new float[]{300, 350, 425, 500, 650, 800, 1000, 1250}
        },
        costs_input : new int[]{1, 3, 5, 8, 15, 20, 25, 35},
        defaultValues : new float[][]{
            new float[]{250, 250, 150},
        },
        _name : "HEALTH", 
        "MAX HEALTH"
    );

    static public readonly Upgrade capacity = new Upgrade(
        values_input : new float[][]{
            new float[]{120, 150, 200, 250, 300, 400}
        },
        costs_input : new int[]{1, 3, 6, 12, 18, 30},
        defaultValues : new float[][]{
            new float[]{100, 100, 100},
        },
        _name : "CAPACITY",
        "CAPACITY"
    );

    static public readonly Upgrade lifesteal = new Upgrade(
        values_input : new float[][]{
             new float[]{2, 5, 10, 15, 20}
        },
        costs_input : new int[]{1, 5, 15, 25, 40},
        defaultValues : new float[][]{
            new float[]{0, 0, 0},
        },
        _name : "LIFESTEAL",
        "HP ON KILL"
    );

    static public readonly Upgrade dual = new Upgrade(
        values_input : new float[][]{
            new float[]{1}
        },
        costs_input : new int[] {25},
        defaultValues : new float[][]{
            new float[]{0, 0, 0},
        },
        _name : "DUAL"
    );

    public static Upgrade GetUpgrade(int id, UpgradesManager caller = null){

        if (caller = null) {
            // return null;
        }

        switch (id) {
            case 0 : {
                return speed;
            }
            case 1 : {
                return health;
            }
            case 2 : {
                return capacity;
            }
            case 3 : {
                return lifesteal;
            }
            case 4 : {
                return dual;
            }
        }

        return null;
    }

    static public readonly int[] max_levels = new int[]{
        speed.max_level,
        health.max_level,
        capacity.max_level,
        lifesteal.max_level,
        dual.max_level
    };

    public static readonly string[] descriptions = new string[]{
        "Increases how fast you can move and dash across the map and even outrun enemies! ",
        "Increases your MAX HEALTH! This is essential as you go farther as attacks from enemies and bosses become much stronger later on",
        "Increases how long you're able to shoot before your gun needs to cool down! ",
        "Grants you a small amount of health per kill! Can serve as an extra way to regen your health through hordes of enemies",
        "Allows you to equip a second weapon!"
    };

}