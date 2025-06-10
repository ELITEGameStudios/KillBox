using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusesUIManager : MonoBehaviour
{
    public static BonusesUIManager instance;
    public List<BonusUIObject> uiObjects;
    public AnimationCurve opacityCurve;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null) { instance = this; }
        else if (instance != this) { Destroy(this); }
    }

    public Image CreateNewTokenGraphic(GameObject prefab, Transform transform)
    {
        GameObject obj = Instantiate(prefab, transform);
        return obj.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (BonusUIObject item in uiObjects)
        {
            if (item.active) { item.UpdateBonus(opacityCurve); }
        }
    }

    public void ActivateBonus(string name, int tokens, float time = 5)
    {
        foreach (BonusUIObject item in uiObjects)
        {
            if (item.name == name)
            {
                item.Activate(tokens, time);
                return;
            }
        }
    }
}