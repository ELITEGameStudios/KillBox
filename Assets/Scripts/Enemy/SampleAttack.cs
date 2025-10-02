

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SampleAttack : BossStateData
{
    public DebugBoss debugBoss;

    public SampleAttack(DebugBoss bossBase) : base(bossBase) // Always include super(bossBase) in any child class constructors 
    {
        // IMPORTANT - THIS IS NOT USED TO REINITIALIZE OR RESET THE ATTACK FOR MULTIPLE USES. USE OnReset() TO REASSIGN DEFAULT VALUES ON STARTUP! (example: timers, counters, end conditions, etc.)
        debugBoss = bossBase;


        OnReset();
    }


    public override void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        base.OnReset();
    }

    // Called When the state object becomes active
    public override void Start()
    {
        
    }

    // Called every frame while the object is active
    public override void Update()
    {

    }    
    
    public override void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        base.End();
    }
}