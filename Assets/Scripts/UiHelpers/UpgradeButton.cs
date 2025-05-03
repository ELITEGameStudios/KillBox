using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeButton : MonoBehaviour
{
    
    public Color text_color;
    [SerializeField] private Image purchase_button_graphic;
    [SerializeField] private Button purchase_button;
    [SerializeField] private Text purchase_cost_display;
    [SerializeField] private int target;

    // Start is called before the first frame update
    void Awake()
    {
        text_color = purchase_cost_display.color;
    }

    // Update is called once per frame
    void Update()
    {
        UpgradesManager.Instance.CheckUpgrade(target, purchase_cost_display,purchase_button,purchase_button_graphic, text_color);
    }
}
