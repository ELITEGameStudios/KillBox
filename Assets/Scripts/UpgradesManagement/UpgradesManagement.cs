using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Upgrade
{
    public readonly int max_level, current_level;
    public readonly float[] values;
    public readonly int[] costs;

    public readonly string name;

    public Upgrade(float[] values_input, int[] costs_input, string _name){
        values = values_input;
        costs = costs_input;
        max_level = values.Length;
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
    static public readonly Upgrade speed = new Upgrade(
        _name : "SPEED",
        values_input : new float[]{5.5f, 6, 7, 8f, 9 , 9.5f, 10f},
        costs_input : new int[]{1, 3, 8, 15, 20, 25, 30}
    );

    static public readonly Upgrade health = new Upgrade(
        _name : "HEALTH", 
        values_input : new float[]{300, 350, 425, 500, 650, 800, 1000, 1250},
        costs_input : new int[]{1, 3, 5, 8, 15, 20, 25, 35}
    );

    static public readonly Upgrade capacity = new Upgrade(
        _name : "CAPACITY",
        values_input : new float[]{120, 150, 200, 250, 300, 400},
        costs_input : new int[]{1, 3, 6, 12, 18, 30}
    );

    static public readonly Upgrade lifesteal = new Upgrade(
        _name : "LIFESTEAL",
        values_input : new float[] {2, 5, 10, 15, 20},
        costs_input : new int[]{1, 5, 15, 25, 40}
    );

    static public readonly Upgrade dual = new Upgrade(
        _name : "DUAL",
        values_input : new float[] {1},
        costs_input : new int[] {25}
    );

    public static Upgrade GetUpgrade(int id, UpgradesManager caller = null){

        if (caller = null) {
            return null;
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
        "SPEED | Increases how fast you can traverse across the map and outrun enemies!",
        "HEALTH | Increases your MAX HEALTH!",
        "CAPACITY | Increases how long you're able to shoot before your gun needs to cool down!",
        "LIFESTEAL | Grants health per kill!",
        "DUAL WIELD | Allows you to equip a second weapon!"
    };

}