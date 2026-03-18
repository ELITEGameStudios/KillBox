
using Pathfinding;
using UnityEngine;

[System.Serializable]
public abstract class GuardianStateData : BossStateData
{
    protected GuardianBoss bossData;
    public int maxEntitiesSharingState;
    public string stateTag;
    public GuardianStateData(GuardianBoss bossBase, string stateTag, int maxEntitiesSharingState = -1) : base(bossBase) // Always include super(bossBase) in any child class constructors
    {
        bossData = bossBase;
        this.maxEntitiesSharingState = maxEntitiesSharingState;
        this.stateTag = stateTag;
    }
}