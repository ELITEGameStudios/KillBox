using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeCompleteClass : MonoBehaviour
{
    [SerializeField]
    private challengeClass challenge_manager;

    [SerializeField]
    private Transform normal_position, hidden_pos, display_pos;

    [Header("The special panel used to display the completed challenge")]
    [SerializeField]
    private GameObject panel;

    [Header("text boxes in the special panel")]
    [SerializeField]
    private Text description_text, title_text, complete_text;

    [SerializeField]
    private Slider dummy_slider;

    [SerializeField]
    private Image pulse_image, tint_img;

    [Header("Panel's Fill and BG Images")]
    [SerializeField]
    private Image fill, bg, slider_fill;

    [SerializeField]
    private Image[] set_bg_color;


    public void Show(int index)
    {
        description_text.text = challenge_manager.panels[index].transform.GetChild(0).GetComponent<Text>().text;
        title_text.text = challenge_manager.panels[index].transform.GetChild(1).GetComponent<Text>().text;

        fill.color = challenge_manager.panels[index].GetComponent<Image>().color;
        slider_fill.color = fill.color;

        bg.color = challenge_manager.panels[index].transform.parent.GetComponent<Image>().color;

        description_text.color = bg.color;
        title_text.color = bg.color;
        complete_text.color = bg.color;
        for(int i = 0; i < set_bg_color.Length; i++)
        {
            set_bg_color[i].color = bg.color;
        }


        StartAnim();
    }

    void StartAnim()
    {
        StopCoroutine("ShowCoroutine");

        normal_position.position = hidden_pos.position;

        Color pulse_col = pulse_image.color;
        pulse_col.a = 0f;
        pulse_image.color = pulse_col;

        Color tint_col = tint_img.color;
        tint_col.a = 0f;
        tint_img.color = tint_col;

        Color txt_col = complete_text.color;
        txt_col.a = 0f;
        complete_text.color = txt_col;

        dummy_slider.value = 0f;

        StartCoroutine("ShowCoroutine");
    }

    IEnumerator ShowCoroutine()
    {
        float pos_lerp = 0, slider_val = 0, alpha_float = 0;

        normal_position.position = hidden_pos.position;

        while(pos_lerp < 1f)
        {
            normal_position.position = Vector3.Lerp(hidden_pos.position, display_pos.position, (Mathf.Pow((pos_lerp - 1), 3) + 1));

            pos_lerp += Time.deltaTime * 0.5f;

            yield return null;
        }

        pos_lerp = 1;

        while(slider_val < 1f)
        {
            slider_val += Time.deltaTime;
            dummy_slider.value = Mathf.Pow(slider_val - 1, 3) + 1;
            yield return null;
        }

        Color pulse_col = pulse_image.color;
        pulse_col.a = 1f;
        pulse_image.color = pulse_col;

        while (pulse_image.color.a > 0f)
        {
            Color pulse_col0 = pulse_image.color;
            pulse_col0.a -= Time.deltaTime;
            pulse_image.color = pulse_col0;
            yield return null;
        }

        while(alpha_float < 1f)
        {
            Color tint_col = tint_img.color;
            tint_col.a = alpha_float;
            tint_img.color = tint_col;

            Color txt_col = complete_text.color;
            txt_col.a = alpha_float;
            complete_text.color = txt_col;

            alpha_float += Time.deltaTime * 2;
            yield return null;
        }

        while(pos_lerp > 0f)
        {
            normal_position.position = Vector3.Lerp(hidden_pos.position, display_pos.position, (Mathf.Pow((pos_lerp - 1), 3) + 1));

            pos_lerp -= Time.deltaTime * 0.5f;
            yield return null;
        }
    }
}
