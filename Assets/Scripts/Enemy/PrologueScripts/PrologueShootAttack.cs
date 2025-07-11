
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
    public override void Start() // Called When the state object becomes active
    {
        prologueData.runesRotator.SetRotationRate(angularSpeed, 0.25f);
        timer = fireRate;
        currentRounds = rounds;
    }
    public override void Update() // Called every frame while the object is active
    {
        if(currentRounds <= 0){ End(); return; }
        if (timer <= 0)
        {
            foreach (GameObject rune in prologueData.runeList)
            {
                Vector2 direction = (rune.transform.position - transform.position).normalized;
                PrologueRuneProjectile runeProjectile = prologueData.GetNewRuneProjectile();
                runeProjectile.gameObject.SetActive(true);
                runeProjectile.StartSeek(
                    rune.transform.position,
                    (Vector2)transform.position + direction * Random.Range(rangeMin, rangeMax)
                );
                
            }

            currentRounds--;
            timer = fireRate;
        }
        else
        {
            timer -= Time.deltaTime;
        }
    }
}