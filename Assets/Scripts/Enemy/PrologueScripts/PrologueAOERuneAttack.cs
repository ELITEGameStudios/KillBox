using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrologueAOERuneAttack : BossStateData
{
    public PrologueBoss prologueData;

    public AOEAttackRune currentBomb;
    public int iterations, startIterations, damage;
    public float interval, startIntervals, timer;


    public PrologueAOERuneAttack(PrologueBoss bossBase, int damage = 250, int iterations = 1, float intervals = 3.5f) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        prologueData = bossBase;
        startIterations = iterations;
        startIntervals = intervals;

        this.interval = startIntervals;
        this.iterations = startIterations;
        this.damage = damage;

        timer = 0;
    }
    public override void Start() { }

    public override void OnReset()
    {
        interval = startIntervals;
        iterations = startIterations;
    }


    public override void Update()
    {
        if (timer <= 0)
        {
            currentBomb = prologueData.CreateAOERune();
            currentBomb.transform.SetParent(null);
            currentBomb.transform.position = Player.main.tf.position;
            currentBomb.damage = damage;
            iterations--;

            timer = interval;
            if (iterations <= 0) { End(); } // This is an interesting bit of logic.
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}
