using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public List<EconomyRange> ranges;
    public List<int> tokensPerRound;
    public int[] finishedRoundBonus; 
    public int[] damagelessBonusValues; 
    public int[] bossRewardTokens; 
    public bool hasRange = false;
    public int tokensThisRound {
        get
        {
            try{ return tokensPerRound[KillBox.currentGame.round - 1];}
            catch{
                GenerateEconomy();
                if (hasRange)
                {
                    return tokensPerRound[KillBox.currentGame.round - 1];
                }
                else
                {
                    return -1;
                }
            }
        }
    }
    public static EconomyManager instance { get; private set; }
    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(this); }
        if(ranges == null){ ranges = new(); }
        tokensPerRound = new();
    }

    [System.Serializable]
    public struct EconomyRange{
        public int round;
        public int minScore;
        public int maxScore;
    }

    public void GenerateEconomy()
    {
        bool canProceed = false;
        for (int i = ranges.Count - 1; i >= 0; i--)
        {
            if (ranges[i].round < KillBox.currentGame.round)
            {
                ranges.RemoveAt(i); // removes outdated range
                canProceed = true;
            }
        }
        if(ranges.Count == 0){ hasRange = false; return; }
        if (!canProceed && tokensPerRound.Count > 0) return; // wont proceed if current range was not outdated
        hasRange = true;

        EconomyRange currentRange = ranges[0]; // ensure in the list that all ranges are in order of their round, ascending.

        int tokens = Random.Range(currentRange.minScore, currentRange.maxScore+1);
        int currentRound = KillBox.currentGame.round;
        int lastRoundOfRange = currentRange.round;
        int difference = lastRoundOfRange - currentRound;

        // Assigns a certain amount of tickets to each round up to the round this range applies to
        int[] tokensPerRoundLocal = new int[difference]; // +1 || for now it will (in game theory) be last round exclusive
        for (int i = 0; i < tokens; i++){
            tokensPerRoundLocal[Random.Range(0, tokensPerRoundLocal.Length)]++;
        }   

        tokensPerRound.AddRange(tokensPerRoundLocal);
    }

    public int GetFinishedRoundBonus()
    {
        List<int> bossRounds = EnemyList.instance.bossRounds;
        int round = KillBox.currentGame.round;
        for (int i = 0; i < finishedRoundBonus.Length; i++)
        {
            if (round < bossRounds[i]) { return finishedRoundBonus[i]; }
        }

        return 0;
    }

    public int GetBossBonus()
    {

        List<int> bossRounds = EnemyList.instance.bossRounds;
        int round = KillBox.currentGame.round;
        for (int i = 0; i < bossRewardTokens.Length; i++)
        {
            if ((int)BossRoundManager.main.bossType == i) { return bossRewardTokens[i]; }
        }

        return 0;
    }

    public int GetDamagelessBonus()
    {
        List<int> bossRounds = EnemyList.instance.bossRounds;
        int round = KillBox.currentGame.round;
        for (int i = 0; i < damagelessBonusValues.Length; i++)
        {
            if (round < bossRounds[i]) { return damagelessBonusValues[i]; }
        }

        return 0;
    }

    public int Get5RoundBonus()
    {
        // List<int> bossRounds = EnemyList.instance.bossRounds;
        // int round = KillBox.currentGame.round;
        // for (int i = 0; i < finishedRoundBonus.Length; i++)
        // {
        //     if (round < bossRounds[i]) { return finishedRoundBonus[i]; }
        // }

        return 5;
    }
}
