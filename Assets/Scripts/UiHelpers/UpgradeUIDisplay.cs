using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUIDisplay : MonoBehaviour
{

    [SerializeField] private Color main_color, shaded_color;
    [SerializeField] private Color main_grey, shaded_grey;
    [SerializeField] private Graphic main_graphic, shaded_graphic;
    [SerializeField] private Button mainButton;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private int target;
    [SerializeField] private bool assigned;



    // Start is called before the first frame update
    void Awake()
    {
        if(!assigned){
            main_color = main_graphic.color;
            shaded_color = shaded_graphic.color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // if ((UpgradesManager.Instance.target_key != target) == particles.isPlaying){
        if (!particles.isPlaying)
        {
            if (UpgradesManager.Instance.target_key != target)
            {
                particles.Play();
            }
        }
        else if (UpgradesManager.Instance.target_key == target)
        {
            particles.Stop();
        }

        // // if the target upgrade is purchasable
        // { particles.startColor = main_color;}
        // else
        // { particles.startColor = shaded_color; }

        main_graphic.enabled = UpgradesManager.Instance.target_key != target;
        // mainButton.interactable = UpgradesManager.Instance.isPurchasable[target];
        Debug.Log("Purchasable: " + target + " | " + UpgradesManager.Instance.isPurchasable[target]);

        if (UpgradesManager.Instance.target_key != target)
        {
            // main_graphic.color = main_grey;
            // shaded_graphic.color = shaded_grey;
        }
        else
        {
            // main_graphic.color = main_color;
            // shaded_graphic.color = shaded_color;
        }
    }
}
