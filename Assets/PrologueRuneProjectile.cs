using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PrologueBoss;

public class PrologueRuneProjectile : MonoBehaviour
{

    [Header("Graphic info")]
    public Animator animator;
    public SpriteRenderer circleRenderer, runeRenderer, arrowRenderer;
    public Color[] debuffColor;
    public Color runeColor, circleColor, arrowColor;


    [Header("Functional info")]
    public DebuffType debuffType;
    public Vector2 startPos, endPos;
    public State state;
    public float radius;
    public float explosionTime, seekTime, currentTimer;
    public AnimationCurve seekLerpCurve;


    [Header("Alpha curves")]
    public AnimationCurve circleAlphaOnExplosion;
    public AnimationCurve runeAlphaOnExplosion;
    public AnimationCurve arrowAlphaOnExplosion;


    public AnimationCurve circleAlphaOnSeek;
    public AnimationCurve runeAlphaOnSeek;
    public AnimationCurve arrowAlphaOnSeek;


    public enum State
    {
        INACTIVE,
        SEEKING,
        EXPLODING
    }

    void Awake()
    {
        runeRenderer.enabled = false;
        circleRenderer.enabled = false;
        arrowRenderer.enabled = false;
        state = State.INACTIVE;
    }

    public void StartSeek(Vector2 startPos, Vector2 endPos)
    {
        this.startPos = startPos;
        this.endPos = endPos;
        animator.Play("Intro");

        runeRenderer.enabled = true;
        circleRenderer.enabled = true;
        arrowRenderer.enabled = true;

        runeRenderer.color = runeColor;

        state = State.SEEKING;
        currentTimer = seekTime;


            switch (debuffType)
            {
                case DebuffType.HEALTH:
                
                    circleColor = debuffColor[0];
                    runeColor = debuffColor[0];
                    arrowColor = debuffColor[0];
                
                    break;

                case DebuffType.SPEED:

                    circleColor = debuffColor[1];
                    runeColor = debuffColor[1];
                    arrowColor = debuffColor[1];
                    
                    break;

            }
    }


    void Update()
    {
        switch (state)
        {
            case State.SEEKING:

                if (currentTimer <= 0) { Explode(); }
                else
                {
                    transform.position = Vector2.Lerp(startPos, endPos, seekLerpCurve.Evaluate(currentTimer / seekTime));

                    circleRenderer.color = Color.Lerp(Color.clear, circleColor, circleAlphaOnSeek.Evaluate(currentTimer / seekTime));
                    runeRenderer.color = Color.Lerp(Color.clear, runeColor, runeAlphaOnSeek.Evaluate(currentTimer / seekTime));
                    arrowRenderer.color = Color.Lerp(Color.clear, arrowColor, arrowAlphaOnSeek.Evaluate(currentTimer / seekTime));
                }

                currentTimer -= Time.deltaTime;
                break;

            case State.EXPLODING:

                if (currentTimer <= 0) { Deactivate(); }
                else
                {
                    circleRenderer.color = Color.Lerp(Color.clear, circleColor, circleAlphaOnExplosion.Evaluate(currentTimer / explosionTime));
                    runeRenderer.color = Color.Lerp(Color.clear, runeColor, runeAlphaOnExplosion.Evaluate(currentTimer / explosionTime));
                    arrowRenderer.color = Color.Lerp(Color.clear, arrowColor, arrowAlphaOnExplosion.Evaluate(currentTimer / explosionTime));
                }

                currentTimer -= Time.deltaTime;
                break;

        }
    }

    void Explode()
    {
        state = State.EXPLODING;
        animator.Play("Explosion");

        Collider2D possiblePlayer = Physics2D.OverlapCircle(transform.position, radius, LayerMask.NameToLayer("Player"));
        if (possiblePlayer != null)
        {
            switch (debuffType)
            {
                case DebuffType.HEALTH:
                    Player.main.health.AddDebuffHealth(Player.main.health.GetMaxHealth() / 5);
                    break;

                case DebuffType.SPEED:
                    Player.main.movement.AddDebuff(1);
                    break;

            }

        }
        currentTimer = explosionTime;
    }

    
    void Deactivate()
    {
        state = State.INACTIVE;
        gameObject.SetActive(false);       
    }
}
