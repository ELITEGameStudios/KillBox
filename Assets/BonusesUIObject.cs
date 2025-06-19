using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusUIObject : MonoBehaviour
{
    public string name;
    // public int tokensDisplaying;
    public GameObject root;
    public Transform tokenTransform;
    public List<Image> tokenUIList;
    public Text text;
    public float time, startTime;
    public bool active;
    public Color initColor;

    public void UpdateBonus(AnimationCurve opacityCurve)
    {
        if (time > 0)
        {
            for (int i = 0; i < tokenUIList.Count; i++)
            {
                tokenUIList[i].color = Color.Lerp(Color.clear, initColor, opacityCurve.Evaluate(1- (time / startTime)));
                text.color = Color.Lerp(Color.clear, initColor, opacityCurve.Evaluate(1- (time / startTime)));
                time -= Time.deltaTime;
            }
        }
        else
        {
            Deactivate();
        }
    }

    void Deactivate()
    {
        for (int i = 0; i < tokenUIList.Count; i++)
        {
            tokenUIList[i].color = Color.clear;
            text.color = Color.clear;
            tokenUIList[i].enabled = false;
        }

        active = false;
        root.SetActive(false);
    }

    public void Activate(int tokens, float time)
    {
        startTime = time;
        this.time = time;

        if (tokens > tokenUIList.Count)
        {
            for (int i = 0; i < tokens - tokenUIList.Count; i++)
            {
                tokenUIList.Add(BonusesUIManager.instance.CreateNewTokenGraphic(tokenUIList[0].gameObject, tokenTransform));
            }
        }

        for (int i = 0; i < tokenUIList.Count; i++)
        {
            tokenUIList[i].enabled = i < tokens;
        }

        root.SetActive(true);
        active = true;
    }

}