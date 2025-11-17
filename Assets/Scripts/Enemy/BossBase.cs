using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class BossBase : MonoBehaviour, IDeathHandler
{

    [Header("Display Information")]
    public string name;
    public Sprite displaySprite;
    public Color displayColor;


    [Header("Base Functional Information")]
    public Transform transform;
    public AIPath movement_script;
    public Rigidbody2D rb_self;
    public Collider2D self_collider;
    public EnemyHealth health;
    public Animator animator;
    public float normalizedHealth { get { return (float)health.CurrentHealth / health.maxHealth; } }
    public bool hasAnimator { get { return animator != null; } }


    [Header("State Machine")]
    public Phase[] phases; // Add phases in minHealth descending order.
    public BossStateData[] statesInPhase;
    public Phase currentPhase;
    public BossStateData currentState;
    public int nextStateIndex;
    public bool dontInstantlySetState, startedAttacks;
    public float stallTimer, introStallTimer;


    [System.Serializable]
    public struct Phase
    {
        public BossStateData[] statesInPhase;
        public float minHealth;
    }


    // Death Handler Addons
    public bool preventDefaultDeath;
    void IDeathHandler.OnDeath(bool to_player){ DeathEvent(to_player); }
    public virtual void DeathEvent(bool to_player = false)
    {
        Debug.Log(name + " Has Died");
    }

    // Start is called before the first frame update
    void Start()
    {

        if (phases.Length > 0)
        {
            SetPhase(phases[0]);
            PhaseCheck();
            if(!dontInstantlySetState) ChooseNextState();
            // SetState(currentPhase.statesInPhase[nextStateIndex], nextStateIndex+1);
        }

        
        BossBarManager.Instance.AddToQueue(gameObject, name, displayColor, displaySprite);
        OnStart();
    }

    public void SetPhase(Phase phase)
    {
        currentPhase = phase;
        statesInPhase = phase.statesInPhase;
        nextStateIndex = 0;

        OnSetPhase();
    }

    protected void SetState(BossStateData state, int nextIndex, bool ignoreStall = false)
    {
        nextStateIndex = nextIndex;
        if (nextStateIndex >= statesInPhase.Length) { nextStateIndex = 0; }

        currentState = state;
        currentState.OnReset();
        
        
        if (currentState.introWaitTime > 0 && !ignoreStall)
        {
            stallTimer = currentState.introWaitTime;
        }
        else
        {
            currentState.Start();
            startedAttacks = true;
        }
    }

    void StateCheck()
    {
        if (currentState != null && !currentState.finished)
        {
            if (!currentState.started)
            {
                // If there is some intro stall time before the state is actually active
                if (stallTimer <= 0)
                {
                    currentState.Start();
                    startedAttacks = true;
                    currentState.started = true;
                }
                else { stallTimer -= Time.deltaTime; }
            }
            else
            {
                // If the state is active and started
                currentState.Update();
            }
        }
        else
        {
            // If the state is finished
            if (stallTimer <= 0){ ChooseNextState(); }
            else{ stallTimer -= Time.deltaTime; }
        }
    }

    protected void PhaseCheck()
    {
        if (currentPhase.minHealth >= normalizedHealth)
        { // Detects wether a new phase should be chosen
            foreach (Phase phase in phases)
            {
                if (phase.minHealth < normalizedHealth)
                {
                    SetPhase(phase);
                    return;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PhaseCheck();
        StateCheck();
        OnUpdate();
    }

    void LateUpdate()
    {
        OnLateUpdate();
    }

    void FixedUpdate()
    {
        if (currentState != null && !currentState.finished)
        { currentState.FixedUpdate(); }

        OnFixedUpdate();
    }
    protected virtual void OnSetPhase(){}

    protected virtual void ChooseNextState()
    {
        SetState(currentPhase.statesInPhase[nextStateIndex], nextStateIndex+1);
    }
    

    protected virtual void OnLateUpdate(){}
    protected virtual void OnUpdate(){}
    protected virtual void OnStart(){}
    protected virtual void OnFixedUpdate(){}
}   
    