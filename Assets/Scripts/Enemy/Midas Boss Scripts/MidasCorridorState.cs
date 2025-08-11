using UnityEngine;

[System.Serializable]
public class MidasCorridorState : BossStateData
{
    MidasBoss midasData;

    public MidasCorridorState(MidasBoss bossBase): base(bossBase){
        this.midasData = bossBase;
    }
    public override void Start(){
        GameObject corridorSummon = Object.Instantiate(midasData.corridor, midasData.transform);
        corridorSummon.transform.SetParent(null);
        End();
    }
    public override void Update(){
        
    }

}