using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using static UpgradesList;

public class SpecialUpgradeButton : MonoBehaviour
{
    public Sprite lockedSprite, unlockedSprite;
    public Image runeImage;
    public Animator anim;
    public bool unlocked;
    public SpecialUpgradeEnum targetUpgrade;

    public void SetLocked()
    {
        runeImage.sprite = lockedSprite;
        anim.SetBool("Unlocked", false);
        unlocked = false;
    }

    public void SetUnlocked()
    {
        runeImage.sprite = unlockedSprite;
        anim.SetBool("Unlocked", true);
        unlocked = true;
    }

}
