using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankUpUI : MonoBehaviour
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
    private Text[] rank_name;

    [SerializeField]
    private Image rank_overlay, tint_img;

    [Header("Panel's Fill and BG Images")]
    [SerializeField]
    private Image fill, bg;

    [SerializeField]
    private Image[] set_bg_color;


    public void Show(int index)
    {


        GameObject sus = new GameObject("", typeof(Rigidbody), typeof(BoxCollider));

        StartAnim();
    }

    void StartAnim()
    {
        StopCoroutine("ShowCoroutine");

        StartCoroutine("ShowCoroutine");
    }

    IEnumerator ShowCoroutine()
    {
        float pos_lerp = 0, slider_val = 0, alpha_float = 0;
        yield return null;
        //normal_position.position = hidden_pos.position;
        //
        //while(pos_lerp < 1f)
        //{
        //    normal_position.position = Vector3.Lerp(hidden_pos.position, display_pos.position, (Mathf.Pow((pos_lerp - 1), 3) + 1));
        //
        //    pos_lerp += Time.deltaTime * 0.5f;
        //
        //    yield return null;
        //}
        //
        //pos_lerp = 1;
        //
        //while(slider_val < 1f)
        //{
        //    slider_val += Time.deltaTime;
        //    dummy_slider.value = Mathf.Pow(slider_val - 1, 3) + 1;
        //    yield return null;
        //}
        //
        //Color pulse_col = pulse_image.color;
        //pulse_col.a = 1f;
        //pulse_image.color = pulse_col;
        //
        //while (pulse_image.color.a > 0f)
        //{
        //    Color pulse_col0 = pulse_image.color;
        //    pulse_col0.a -= Time.deltaTime;
        //    pulse_image.color = pulse_col0;
        //    yield return null;
        //}
        //
        //while(alpha_float < 1f)
        //{
        //    Color tint_col = tint_img.color;
        //    tint_col.a = alpha_float;
        //    tint_img.color = tint_col;
        //
        //    Color txt_col = complete_text.color;
        //    txt_col.a = alpha_float;
        //    complete_text.color = txt_col;
        //
        //    alpha_float += Time.deltaTime * 2;
        //    yield return null;
        //}
        //
        //while(pos_lerp > 0f)
        //{
        //    normal_position.position = Vector3.Lerp(hidden_pos.position, display_pos.position, (Mathf.Pow((pos_lerp - 1), 3) + 1));
        //
        //    pos_lerp -= Time.deltaTime * 0.5f;
        //    yield return null;
        //}
    }
}
