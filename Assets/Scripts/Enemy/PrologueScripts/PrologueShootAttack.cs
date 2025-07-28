
using UnityEngine;
using static PrologueBoss;

[System.Serializable]
public class PrologueShootAttack : BossStateData
{


    PrologueBoss prologueData;

    public float timer;
    public DebuffType debuffType;


    public int rounds, currentRounds;
    public float fireRate;
    public float rangeMin;
    public float rangeMax;
    public float angularSpeed = 90;


    public PrologueShootAttack(PrologueBoss bossBase, int rounds, float fireRate, float rangeMin, float rangeMax, float angularSpeed = 90) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        prologueData = bossBase;

        this.rounds = rounds;
        this.fireRate = fireRate;
        this.rangeMax = rangeMax;
        this.rangeMin = rangeMin;
        this.angularSpeed = angularSpeed;


        // this.debuffType = debuffType;
    }
    public override void OnReset() // Called When the state object becomes active
    {
        timer = fireRate;
        currentRounds = rounds;
    }

    public override void Start() // Called When the state object becomes active
    {
        timer = fireRate;
        currentRounds = rounds;
        prologueData.SetRotSpeed(0.15f);
    }
    public override void Update() // Called every frame while the object is active
    {
        if (currentRounds <= 0) { End(); return; }
        if (timer <= 0)
        {
            int i = 0;
            foreach (PrologueRuneScript rune in prologueData.runeList)
            {

                Vector2 direction = (rune.transform.position - transform.position).normalized;
                PrologueRuneProjectile runeProjectile = prologueData.GetNewRuneProjectile();
                runeProjectile.gameObject.SetActive(true);
                runeProjectile.StartSeek(
                    rune.transform.position,
                    (Vector2)transform.position + direction * Random.Range(rangeMin, rangeMax),
                    (DebuffType)i
                );

                i++;
            }

            currentRounds--;
            timer = fireRate;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }

    public override void End(bool interrupted = false)
    {
        base.End(interrupted);
        PrologueArenaSpawnSystem.SpawnEnemies(prologueData.entitiesToSpawn[1], 0.2f, 2, 0, false, 5);
    }
}