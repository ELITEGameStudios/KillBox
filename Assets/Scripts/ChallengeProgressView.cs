using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeProgressView : MonoBehaviour
{
    [SerializeField]
    private challengeClass challenge_manager;

    [SerializeField]
    private GameManager manager;

    //[SerializeField]
    //private Transform[] panel_transforms;

    [SerializeField]
    private GameObject[] panels, hunter_panels, survivor_panels, spender_panels;

    [SerializeField]
    private Text[] description_text, title_text, complete_text, reward_text;

    [SerializeField]
    private Slider[] main_sliders, hunter_sliders, survivor_sliders, spender_sliders;

    //[SerializeField]
    //private Image[] pulse_image, tint_img;

    [SerializeField]
    private Image[] fill, bg, slider_fill;

    [SerializeField]
    private Image[] sbgcol, sbgcol2, sbgcol3;

    [SerializeField]
    private int[] h_list, sv_list, sp_list;


    void Update()
    {
        int hunter_index = 0, survivor_index = 0, spender_index = 0;
        bool h_get = false, 
            sv_get = false, 
            sp_get = false;

        for(int i = 0; i < 5; i++)
        {
            //get current challenge
            if (!manager.completed_challenges[h_list[i]] && !h_get)
            {
                hunter_index = i;
                h_get = true;
            }

            if (!manager.completed_challenges[sv_list[i]] && !sv_get)
            {
                survivor_index = i;
                sv_get = true;
            }

            if (!manager.completed_challenges[sp_list[i]] && !sp_get)
            {
                spender_index = i;
                sp_get = true;
            }

            //if all challenges of any series is complete
            if(!h_get && i == 4)
            {
                hunter_index = 4;
            }

            if (!sv_get && i == 4)
            {
                survivor_index = 4;
            }

            if (!sp_get && i == 4)
            {
                spender_index = 4;
            }
        }
        
        description_text[0].text = hunter_panels[hunter_index].transform.GetChild(0).GetComponent<Text>().text;
        description_text[1].text = survivor_panels[survivor_index].transform.GetChild(0).GetComponent<Text>().text;
        description_text[2].text = spender_panels[spender_index].transform.GetChild(0).GetComponent<Text>().text;

        title_text[0].text = hunter_panels[hunter_index].transform.GetChild(1).GetComponent<Text>().text;
        title_text[1].text = survivor_panels[survivor_index].transform.GetChild(1).GetComponent<Text>().text;
        title_text[2].text = spender_panels[spender_index].transform.GetChild(1).GetComponent<Text>().text;

        reward_text[0].text = hunter_panels[hunter_index].transform.GetChild(3).GetComponent<Text>().text;
        reward_text[1].text = survivor_panels[survivor_index].transform.GetChild(3).GetComponent<Text>().text;
        reward_text[2].text = spender_panels[spender_index].transform.GetChild(3).GetComponent<Text>().text;

        fill[0].color = hunter_panels[hunter_index].GetComponent<Image>().color;
        fill[1].color = survivor_panels[survivor_index].GetComponent<Image>().color;
        fill[2].color = spender_panels[spender_index].GetComponent<Image>().color;

        bg[0].color = hunter_panels[hunter_index].transform.parent.GetComponent<Image>().color;
        bg[1].color = survivor_panels[survivor_index].transform.parent.GetComponent<Image>().color;
        bg[2].color = spender_panels[spender_index].transform.parent.GetComponent<Image>().color;

        slider_fill[0].color = fill[0].color;
        slider_fill[1].color = fill[1].color;
        slider_fill[2].color = fill[2].color;

        description_text[0].color = bg[0].color;
        description_text[1].color = bg[1].color;
        description_text[2].color = bg[2].color;

        main_sliders[0].value = hunter_sliders[hunter_index].value;
        main_sliders[1].value = survivor_sliders[survivor_index].value;
        main_sliders[2].value = spender_sliders[spender_index].value;

        main_sliders[0].maxValue = hunter_sliders[hunter_index].maxValue;
        main_sliders[1].maxValue = survivor_sliders[survivor_index].maxValue;
        main_sliders[2].maxValue = spender_sliders[spender_index].maxValue;

        title_text[0].color = bg[0].color;
        title_text[1].color = bg[1].color;
        title_text[2].color = bg[2].color;

        reward_text[0].color = bg[0].color;
        reward_text[1].color = bg[1].color;
        reward_text[2].color = bg[2].color;

        for (int i = 0; i < sbgcol.Length; i++)
        {
            sbgcol[i].color = bg[0].color;
            sbgcol2[i].color = bg[1].color;
            sbgcol3[i].color = bg[2].color;
        }
    }
}
