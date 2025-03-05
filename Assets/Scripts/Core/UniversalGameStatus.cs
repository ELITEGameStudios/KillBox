using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniversalGameStatus
{
    public static bool HAS_LAUNCHED {get; private set;} = false;
    public static int TIMES_RESTARTED {get; private set;} = 3;

    public static void OnLaunch(){ HAS_LAUNCHED = true; }
    
}
