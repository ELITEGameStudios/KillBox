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
    {//
        ////setting slider values
        //challenge_sliders[0].value = manager.player_kills;
        //challenge_sliders[1].value = manager.LvlCount;
        //challenge_sliders[2].value = manager.tokens_used;
//
        //challenge_sliders[3].value = manager.player_kills;
        //challenge_sliders[4].value = manager.LvlCount;
        //challenge_sliders[5].value = manager.tokens_used;
//
        //challenge_sliders[6].value = manager.player_kills;
        //challenge_sliders[7].value = manager.LvlCount;
        //challenge_sliders[8].value = manager.tokens_used;
//
        //challenge_sliders[9].value = manager.player_kills;
        //challenge_sliders[10].value = manager.LvlCount;
        //challenge_sliders[11].value = manager.tokens_used;
//
        //challenge_sliders[12].value = manager.player_kills;
        //challenge_sliders[13].value = manager.LvlCount;
        //challenge_sliders[14].value = manager.tokens_used;
//
        //for(int i = 0; i < manager.completed_challenges.Length; i++){
        //    if(manager.completed_challenges[i] && challenge_sliders[i].gameObject.activeInHierarchy){
        //        challenge_sliders[i].gameObject.SetActive(false);
        //        completion_panels[i].SetActive(true);
        //    }
        //}
//
        ////challenges logic
        //if(manager.player_kills >= 50 && !manager.completed_challenges[0]){
        //    challenge_sliders[0].gameObject.SetActive(false);
        //    completion_panels[0].SetActive(true);
        //    ChallengeFields.player_xp += 500;
//
        //    manager.completed_challenges[0] = true;
//
        //    completion_ui.Show(0);
        //}
//
        //if(manager.LvlCount >= 10 && !manager.completed_challenges[1]){
        //    challenge_sliders[1].gameObject.SetActive(false);
        //    completion_panels[1].SetActive(true);
        //    ChallengeFields.player_xp += 250;
//
        //    manager.completed_challenges[1] = true;
//
        //    completion_ui.Show(1);
        //}
//
        //if(manager.tokens_used >= 20 && !manager.completed_challenges[2]){
        //    challenge_sliders[2].gameObject.SetActive(false);
        //    completion_panels[2].SetActive(true);
        //    ChallengeFields.player_xp += 300;
//
        //    manager.completed_challenges[2] = true;
//
        //    completion_ui.Show(2);
        //}
//
        //if(manager.player_kills >= 200 && !manager.completed_challenges[3]){
        //    challenge_sliders[3].gameObject.SetActive(false);
        //    completion_panels[3].SetActive(true);
        //    ChallengeFields.player_xp += 1000;
//
        //    manager.completed_challenges[3] = true;
//
        //    completion_ui.Show(3);
        //}
//
        //if(manager.LvlCount >= 25 && !manager.completed_challenges[4]){
        //    challenge_sliders[4].gameObject.SetActive(false);
        //    completion_panels[4].SetActive(true);
        //    ChallengeFields.player_xp += 1000;
//
        //    manager.completed_challenges[4] = true;
//
        //    completion_ui.Show(4);
        //}
//
        //if(manager.tokens_used >= 50 && !manager.completed_challenges[5]){
        //    challenge_sliders[5].gameObject.SetActive(false);
        //    completion_panels[5].SetActive(true);
        //    ChallengeFields.player_xp += 750;
//
        //    manager.completed_challenges[5] = true;
//
        //    completion_ui.Show(5);
        //}
//
        //if(manager.player_kills >= 1000 && !manager.completed_challenges[6]){
        //    challenge_sliders[6].gameObject.SetActive(false);
        //    completion_panels[6].SetActive(true);
        //    ChallengeFields.player_xp += 2500;
//
        //    manager.completed_challenges[6] = true;
//
        //    completion_ui.Show(6);
        //}
//
        //if(manager.LvlCount >= 50 && !manager.completed_challenges[7]){
        //    challenge_sliders[7].gameObject.SetActive(false);
        //    completion_panels[7].SetActive(true);
        //    ChallengeFields.player_xp += 2500;
//
        //    manager.completed_challenges[7] = true;
//
        //    completion_ui.Show(7);
        //}
//
        //if(manager.tokens_used >= 125 && !manager.completed_challenges[8]){
        //    challenge_sliders[8].gameObject.SetActive(false);
        //    completion_panels[8].SetActive(true);
        //    ChallengeFields.player_xp += 2000;
//
        //    manager.completed_challenges[8] = true;
//
        //    completion_ui.Show(8);
        //}
//
//
        //if(manager.player_kills >= 5000 && !manager.completed_challenges[9]){
        //    challenge_sliders[9].gameObject.SetActive(false);
        //    completion_panels[9].SetActive(true);
        //    ChallengeFields.player_xp += 7500;
//
        //    manager.completed_challenges[9] = true;
//
        //    completion_ui.Show(9);
        //}
//
        //if(manager.LvlCount >= 100 && !manager.completed_challenges[10]){
        //    challenge_sliders[10].gameObject.SetActive(false);
        //    completion_panels[10].SetActive(true);
        //    ChallengeFields.player_xp += 6000;
//
        //    manager.completed_challenges[10] = true;
//
        //    completion_ui.Show(10);
        //}
//
        //if(manager.tokens_used >= 350 && !manager.completed_challenges[11]){
        //    challenge_sliders[11].gameObject.SetActive(false);
        //    completion_panels[11].SetActive(true);
        //    ChallengeFields.player_xp += 4000;
//
        //    manager.completed_challenges[11] = true;
//
        //    completion_ui.Show(11);
        //}
//
        //if(manager.player_kills >= 20000 && !manager.completed_challenges[12]){
        //    challenge_sliders[12].gameObject.SetActive(false);
        //    completion_panels[12].SetActive(true);
        //    ChallengeFields.player_xp += 20000;
//
        //    manager.completed_challenges[12] = true;
//
        //    completion_ui.Show(12);
        //}
//
        //if(manager.LvlCount >= 150 && !manager.completed_challenges[13]){
        //    challenge_sliders[13].gameObject.SetActive(false);
        //    completion_panels[13].SetActive(true);
        //    ChallengeFields.player_xp += 15000;
//
        //    manager.completed_challenges[13] = true;
//
        //    completion_ui.Show(13);
        //}
//
        //if(manager.tokens_used >= 750 && !manager.completed_challenges[14]){
        //    challenge_sliders[14].gameObject.SetActive(false);
        //    completion_panels[14].SetActive(true);
        //    ChallengeFields.player_xp += 20000;
//
        //    manager.completed_challenges[14] = true;
//
        //    completion_ui.Show(14);
        //}
//
        //if(shop.OwnedGuns[11] != 0 && !manager.completed_challenges[15]){
        //    manager.completed_challenges[15] = true;
        //    completion_panels[15].SetActive(true);
        //    ChallengeFields.player_xp += 8350;
//
        //    completion_ui.Show(15);
        //}
//
        //for(int i = 0; i < manager.completed_challenges.Length && manager.completed_challenges[i]; i++){
        //    if(i == 15 && !manager.completed_challenges[16]){
        //        manager.completed_challenges[16] = true;
        //        ChallengeFields.player_xp += 8350;
        //        completion_panels[16].SetActive(true);
        //        completion_ui.Show(16);
        //    }
        //}

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
