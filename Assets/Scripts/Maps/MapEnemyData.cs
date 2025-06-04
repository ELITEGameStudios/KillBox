using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEnemyData{

    //public static readonly MapEnemyData[] maps = new MapEnemyData[]{
//
    //    new MapEnemyData(new bool[] {false, false, false} ),
    //    new MapEnemyData(new bool[] {false, false, false} ),
    //    new MapEnemyData(new bool[] {false, false, false} ),
    //    new MapEnemyData(new bool[] {true, true, true} ),
//
    //    new MapEnemyData(new bool[] {true, true, true} ),
    //    new MapEnemyData(new bool[] {true, true, true} ),
    //    new MapEnemyData(new bool[] {true, true, true} ),
    //    new MapEnemyData(new bool[] {true, true, true} ),
//
    //    new MapEnemyData(new bool[] {true, true, true} ),
    //    new MapEnemyData(new bool[] {true, true, true} ),
    //    new MapEnemyData(new bool[] {true, false, false} ),
    //    new MapEnemyData(new bool[] {true, true, true} ),
//
    //    new MapEnemyData(new bool[] {true, true, true} ),
    //    new MapEnemyData(new bool[] {true, false, true} ),
    //    new MapEnemyData(new bool[] {true, false, false} ),
    //};
    public bool[] mini_boss_valid {get; private set;}

    MapEnemyData(bool[] _mini_boss_valid){

        mini_boss_valid = _mini_boss_valid;
    }
}
