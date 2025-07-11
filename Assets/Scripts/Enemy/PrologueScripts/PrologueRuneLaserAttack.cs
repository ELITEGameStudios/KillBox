
using UnityEngine;
using static PrologueBoss;

[System.Serializable]
public class PrologueRuneLaserAttack : BossStateData
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


    public GameObject currentRuneObj, currentBeamObject;
    public Transform initialTransform;
    public Vector2 fireStartPos, fireEndPos, initialPos, initialPlayerPos;
    public float distanceFromPlayer;
    public FiringStatus beamState;


    public enum FiringStatus
    {
        WARNING,
        FIRING,
        RETURNING

    }

    public PrologueRuneLaserAttack(PrologueBoss bossBase, int rounds, float fireTime, float warningTime, float distance, float angularSpeed = 90) : base(bossBase) // need to test if this auto-calls the super constructor
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
        prologueData.runesRotator.SetRotationRate(angularSpeed, 0.25f);
        currentRounds = rounds;
        runeObjIndex = Random.Range(0, 4);
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
            case FiringStatus.RETURNING:
                ReturnUpdate();
                break;
        }

        currentBeamObject.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Vector2)initialPlayerPos - (Vector2)currentBeamObject.transform.position);
        timer -= Time.deltaTime;
    }

    void SetupNewBeam()
    {
        // Getting Objects
        runeObjIndex++; if(runeObjIndex > 3) { runeObjIndex = 0; }
        currentRuneObj = prologueData.runeList[runeObjIndex];
        initialPos = currentRuneObj.transform.position;

        currentBeamObject = prologueData.beamObject;
        currentBeamObject.transform.SetParent(currentRuneObj.transform);

        initialTransform = currentRuneObj.transform.parent;
        currentRuneObj.transform.SetParent(null);


        // Setting Positions and Markers
        fireStartPos = (Vector2)Player.main.tf.position
            + new Vector2(
                Mathf.Cos(Random.Range(0, 6.28f)),
                Mathf.Sin(Random.Range(0, 6.28f))
            ) * distanceFromPlayer;


        int number = Random.Range(0, 2);
        float angleToPlayer = Vector2.Angle(fireStartPos, Player.main.tf.position);
        fireEndPos = fireStartPos +

        new Vector2(
                Mathf.Cos(angleToPlayer * Mathf.Deg2Rad + number == 1 ? (3.14f / 2) : (-3.14f / 2)),
                Mathf.Sin(angleToPlayer * Mathf.Deg2Rad + number == 1 ? (3.14f / 2) : (-3.14f / 2))
            ) * distanceFromPlayer;

        initialPlayerPos = Player.main.tf.position;
        // Setting Beam Size
        // Beam width - x. Height - y
        currentBeamObject.transform.localScale = new Vector2(0, beamHeight);
        
        prologueData.sweepingIndicator.StartIndicator(warningTime, fireStartPos, (Vector2)Player.main.tf.position - fireStartPos);


        beamState = FiringStatus.WARNING;
        timer = warningTime;
        currentRounds--;
    }

    void WarnBeamUpdate()
    {

        if (timer > 0)
        {
            currentRuneObj.transform.position = Vector2.Lerp(initialPos, fireStartPos, prologueData.fireLerpCurve.Evaluate(1 - (timer / warningTime)));
        }
        else
        {
            currentRuneObj.transform.position = fireStartPos;

            currentBeamObject.transform.SetParent(currentRuneObj.transform);
            currentBeamObject.transform.localPosition = Vector2.zero;
            currentBeamObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = prologueData.debuffColor[runeObjIndex];
            currentBeamObject.SetActive(true);


            beamState = FiringStatus.FIRING;
            timer = fireTime;
        }
    }
    void FiringUpdate()
    {
        if (timer > 0)
        {
            currentRuneObj.transform.position = Vector2.Lerp(fireStartPos, fireEndPos, prologueData.fireLerpCurve.Evaluate(1 - (timer / fireTime)));
            currentBeamObject.transform.localScale = new Vector2(
                prologueData.beamWidthCurve.Evaluate(1 - (timer / fireTime)),
                beamHeight);

            Debug.Log(timer);

        }
        else
        {
            currentBeamObject.SetActive(false);
            currentBeamObject.transform.SetParent(prologueData.transform);
            currentBeamObject.transform.localPosition = Vector2.zero;


            currentRuneObj.transform.position = fireEndPos;
            beamState = FiringStatus.RETURNING;
            timer = 0.5f;
        }
    }
    void ReturnUpdate()
    {

        if (timer > 0)
        {
            currentRuneObj.transform.position = Vector2.Lerp(fireEndPos, initialTransform.position, prologueData.fireLerpCurve.Evaluate(1 - (timer / 0.5f)));
        }
        else
        {
            currentRuneObj.transform.SetParent(initialTransform);
            currentRuneObj.transform.localPosition = Vector2.zero;

            if (currentRounds <= 0) { End(); return; }
            SetupNewBeam();
        }
    }
}