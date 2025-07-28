
using UnityEngine;
using static PrologueBoss;

[System.Serializable]
public class PrologueSpecialRuneLaserAttack : BossStateData
{
    PrologueBoss prologueData;

    public float timer;
    public DebuffType debuffType;


    public int rounds, currentRounds;
    public int runeObjIndex;
    public float fireTime, warningTime;
    public float rangeMin;
    public float rangeMax;
    public float angularSpeed = 90;
    public float beamHeight = 40;
    public float beamWidth = 6;


    public GameObject[] beamObjects;
    public Transform[] runeObjs;
    public Transform initialTransform;
    public Vector2 fireStartPos, fireEndPos, initialPos, initialPlayerPos;
    public float distanceFromPlayer;
    public FiringStatus beamState;


    public enum FiringStatus
    {
        WARNING,
        FIRING
    }

    public PrologueSpecialRuneLaserAttack(PrologueBoss bossBase, int rounds, float fireTime, float warningTime, float distance, float angularSpeed = 30) : base(bossBase) // need to test if this auto-calls the super constructor
    {
        prologueData = bossBase;

        this.rounds = rounds;
        this.fireTime = fireTime;
        this.warningTime = warningTime;
        // this.rangeMax = rangeMax;
        distanceFromPlayer = distance;
        this.angularSpeed = angularSpeed;

        beamWidth = 6;


        // this.debuffType = debuffType;
    }
    public override void Start() // Called When the state object becomes active
    {
        // prologueData.runesRotator.SetRotationRate(angularSpeed, 0.25f);
        currentRounds = rounds;
        runeObjs = prologueData.runeParentTf;
        beamObjects = prologueData.specialBeamObjects;
        SetupNewBeam();
    }
    public override void Update() // Called every frame while the object is active
    {

        switch (beamState)
        {
            case FiringStatus.WARNING:
                WarnBeamUpdate();
                break;
            case FiringStatus.FIRING:
                FiringUpdate();
                break;
        }

        // beamObjects[i].transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector2)beamObjects[i].transform.position - (Vector2)transform.position);
        timer -= Time.deltaTime;
    }

    void SetupNewBeam()
    {
        // Getting Objects

        for (int i = 0; i < beamObjects.Length; i++)
        {
            // initialTransform = runeObjs[i].transform.parent;
            beamObjects[i].transform.SetParent(runeObjs[i]);
            beamObjects[i].transform.localScale = new Vector2(0, beamHeight);

            // prologueData.sweepingIndicator.StartIndicator(warningTime, fireStartPos, (Vector2)Player.main.tf.position - fireStartPos);

            beamState = FiringStatus.WARNING;
            timer = warningTime;
        }
    }

    void WarnBeamUpdate()
    {

        for (int i = 0; i < beamObjects.Length; i++)
        {
            if (timer > 0)
            {
                // runeObjs[i].transform.position = Vector2.Lerp(initialPos, fireStartPos, prologueData.fireLerpCurve.Evaluate(1 - (timer / warningTime)));
            }
            else
            {
                beamObjects[i].transform.SetParent(runeObjs[i].transform);
                beamObjects[i].transform.localPosition = Vector2.zero;
                beamObjects[i].transform.GetChild(0).GetComponent<SpriteRenderer>().color = prologueData.specialColor;
                beamObjects[i].SetActive(true);


                beamState = FiringStatus.FIRING;
                timer = fireTime;
            }
        }
    }
    void FiringUpdate()
    {
        for (int i = 0; i < beamObjects.Length; i++)
        {
            if (timer > 0)
            {
                beamObjects[i].transform.localScale = new Vector2(
                    prologueData.beamWidthCurve.Evaluate(1 - (timer / fireTime)),
                    beamHeight);

                Debug.Log(timer);
            }
            else
            {
                beamObjects[i].SetActive(false);
                beamObjects[i].transform.SetParent(prologueData.transform);
                beamObjects[i].transform.localPosition = Vector2.zero;
                End();
                // timer = 0.5f;
            }
        }
    }
}