using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class challengeClass : MonoBehaviour
{
    public GameManager manager;

    [SerializeField]
    private Text rank_text, xp_text;

    [SerializeField]
    private ChallengeCompleteClass completion_ui;

    [SerializeField]
    private Slider[] challenge_sliders;

    [SerializeField]
    private Slider xp_slider;

    [SerializeField]
    private ShopScript shop;

    public Color[] rank_colors, rank_colors_darker;

    [SerializeField]

    private int[] rank_requirements;
    public string[] rank_names;
    public GameObject[] rank_art, panels, completion_panels, elements_to_rank_color, elements_to_rank_color_darker;

    [SerializeField]
    private UnityEvent[] on_rank;



    // Start is called before the first frame update
    void Awake()
    {
        manager = gameObject.GetComponent<GameManager>();
        CheckRank();
    }

    // Update is called once per frame
    void Update()
    {

        CheckRank();
    }

    public void CheckRank(){
        for(int i = 0; i < rank_requirements.Length; i++){
            if(ChallengeFields.player_xp >= rank_requirements[i]){

                if(i != rank_requirements.Length - 1){
                    if(ChallengeFields.player_xp < rank_requirements[i+1]){

                        
                        xp_slider.minValue = rank_requirements[i];
                        xp_slider.maxValue = rank_requirements[i+1];
                        xp_slider.value = ChallengeFields.player_xp;

                        xp_text.text = "XP: " + ChallengeFields.player_xp.ToString() + " / " + rank_requirements[i+1].ToString();
                        ApplyRank(i);
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    ApplyRank(i);
                    xp_slider.minValue = 0;
                    xp_slider.maxValue = 1;
                    xp_slider.value = 1;

                    xp_text.text = "XP: " + ChallengeFields.player_xp.ToString() + " (MAX)";
                }
            }
        }
    }

    public void ApplyRank(int index){
        for(int i = 0; i < rank_names.Length; i++){
            rank_art[i].SetActive(false);
        }
        rank_art[index].SetActive(true);

        for(int i = 0; i < elements_to_rank_color.Length; i++)
        {
            if (elements_to_rank_color[i].GetComponent<Image>() != null)
            {
                elements_to_rank_color[i].GetComponent<Image>().color = rank_colors[index];
            }
            else
            {
                elements_to_rank_color[i].GetComponent<Text>().color = rank_colors[index];
            }
        }

        for (int i = 0; i < elements_to_rank_color_darker.Length; i++)
        {
            if (elements_to_rank_color_darker[i].GetComponent<Image>() != null)
            {
                elements_to_rank_color_darker[i].GetComponent<Image>().color = rank_colors_darker[index];
            }
            else
            {
                elements_to_rank_color_darker[i].GetComponent<Text>().color = rank_colors_darker[index];
            }
        }

        for(int i = 0; i < index + 1; i++)
        {
            on_rank[i].Invoke();
        }

        rank_text.text = rank_names[index];
    }
}
