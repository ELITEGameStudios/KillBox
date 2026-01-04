using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using static UpgradesList;

[DefaultExecutionOrder(-1001)]
public class BossChallengeGameManager : GameManager
{
    public BossRoundManager.BossType currentBoss;

    public override List<int> GetAvailableMaps()
    {
        List<int> availableIndexes = new List<int>();
        foreach (MapData map in GetMaps)
        {
            if (map.Index == 100 + (_level - 1) % 5 + 1) { availableIndexes.Add(map.Index); } // format for boss round maps 
        }

        return availableIndexes;
    }

    public void UpdateCurrentBoss()
    {
        currentBoss++;
    }

    public override void StartGame()
    {
        // FadeAnimator.Play("FadeAnim");
        // enemyList.OnStart();
        SetNewMap(GetMapByID(0));
        // BossRoundManager.main.SetBossRound(true);
        StartCoroutine(StartNumerator());
    }

}
