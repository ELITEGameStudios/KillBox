using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashElementScript : MonoBehaviour
{
    [SerializeField] float seconds = 5;
    [SerializeField] Text hotkeyText;
    [SerializeField] Animator animator;
    [SerializeField] Image[] arrows;
    [SerializeField] float timer;
    [SerializeField] Color mainColor, inactiveColor;

    public void UpdateDisplay(float seconds)
    {
        this.seconds = seconds;
        for (int i = 0; i < arrows.Length; i++) { arrows[i].gameObject.SetActive(i < seconds); }

    }

    // Update is called once per frame
    void Update()
    {
        timer = Player.main.movement.GetDashCooldownTimer();
        for (int i = arrows.Length-1; i >= 0; i--) { arrows[i].color = i <= timer ? inactiveColor: mainColor; }

        animator.SetBool("Ready", timer <= 0);
        hotkeyText.text = CustomKeybinds.main.GetKeybindString(CustomKeybinds.main.Dash);
    }
}
