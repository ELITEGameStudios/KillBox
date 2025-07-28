using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PrologueBoss;

public class PrologueRuneScript : MonoBehaviour
{
    public SpriteRenderer mainRenderer, glowRenderer, whiteOverlayRenderer;
    public Sprite mainSprite, specialSprite, specialSpriteFilled;
    public Animator animator;
    public DebuffType debuff;
    public Color mainColor, sColor;

    public bool isSpecial;
    public bool isVisible;

    // Start is called before the first frame update
    void Awake(){
        if(animator == null){ animator = GetComponent<Animator>(); }
    }

    void Start()
    {
        isVisible = true;
        isSpecial = false;
    }

    void Update(){
        animator.SetBool("isSpecial", isSpecial);
        animator.SetBool("isVisible", isVisible);
    }

    public void TransformToSpecial()
    {
        isSpecial = true;
        mainRenderer.sprite = specialSprite;
        whiteOverlayRenderer.sprite = specialSpriteFilled;

        whiteOverlayRenderer.color = sColor;
        mainRenderer.color = sColor;
        glowRenderer.color = sColor;
    }

    public void Dissapear(){
        isVisible = false;
    }

    public void SpecialDissappear()
    {
        animator.Play("FinalDissapear");
        isVisible = false;
    }
    
    public void Appear()
    {
        isVisible = true;
        // animator.SetTrigger("Appear");
        animator.Play("Intro");
    }

    internal void ToggleDrain(bool drainStatus)
    {
        animator.SetBool("isDraining", drainStatus);
        // isVisible = true;
    }
}
