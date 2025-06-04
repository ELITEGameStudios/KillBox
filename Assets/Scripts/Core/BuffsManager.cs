using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffsManager : MonoBehaviour
{
    
    public Text[] buff_text, buff_menu_text;

    public int[] buff_strength;

    public string[] numerals;
    //Buff 0: Resistance(take less damage), Buff 1: CooldownTime, Buff 2: Lifesteal Buff 3:Equipment Recharge Buff, Buff 4: Chance Penetration

    void Update()
    {

    }

    public void RecieveBuff(int index)
    {
        buff_strength[index]++;
        if (buff_strength[index] > 5)
            buff_strength[index] = 5;

        GunHandler.Instance.cooldown.UpdateCooldownTime();

        for (int i = 0; i < 5; i++)
        {
            if(buff_strength[index] == i+1)
            {
                buff_text[index].text = numerals[i];
                buff_menu_text[index].text = numerals[i];
                break;
            }
        }
    }
}
