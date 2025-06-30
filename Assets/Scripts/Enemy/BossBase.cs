using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using Pathfinding;

public class BossBase : MonoBehaviour
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


    [System.Serializable]
    public struct Phase
    {
        public BossStateData[] statesInPhase;
        public float minHealth;
    }

    // Start is called before the first frame update
    void Start()
    {

        if (phases.Length > 0)
        {
            SetPhase(phases[0]);
            PhaseCheck();
            SetState(currentPhase.statesInPhase[nextStateIndex]);
        }

        BossBarManager.Instance.AddToQueue(gameObject, name, displayColor, displaySprite);
        OnStart();
    }

    void SetPhase(Phase phase)
    {
        currentPhase = phase;
        statesInPhase = phase.statesInPhase;
        nextStateIndex = 0;
    }

    void SetState(BossStateData state)
    {
        nextStateIndex++;
        if (nextStateIndex >= statesInPhase.Length) { nextStateIndex = 0; }

        currentState = state;
        currentState.OnReset();
        currentState.Start();
    }

    void StateCheck()
    {
        if (currentState != null && !currentState.finished)
        {
            currentState.Update();
        }
        else
        {
            SetState(currentPhase.statesInPhase[nextStateIndex]);
        }
    }

    void PhaseCheck()
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

    void FixedUpdate()
    {
        if (currentState != null && !currentState.finished)
        { currentState.FixedUpdate(); }

        OnFixedUpdate();
    }
    
    protected virtual void OnUpdate(){}
    protected virtual void OnStart(){}
    protected virtual void OnFixedUpdate(){}
}   
    