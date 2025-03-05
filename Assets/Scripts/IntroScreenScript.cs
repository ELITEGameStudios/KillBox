using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScreenScript : MonoBehaviour
{
    [SerializeField] private float timer, fluctuateSpeed, introSeconds;
    [SerializeField] private int state;
    [SerializeField] private Image logo;
    [SerializeField] private Text text;
    [SerializeField] private Color normalColor, interColor;
    [SerializeField] private Vector3 logoScalePre, logoScalePost;
    [SerializeField] private GameObject menus, fps;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey){
            menus.SetActive(true);
            fps.SetActive(true);
            gameObject.SetActive(false);
        }
        else{
            if(state == 0){
                if(timer < introSeconds){
                    float factor = Mathf.Sin(- (Mathf.PI * timer) / (2 * introSeconds)) + 1;
                    // Debug.Log(factor);
                    logo.transform.localScale = Vector3.Lerp(Vector3.one, logoScalePre, factor);
                    logo.color = Color.Lerp(normalColor, Color.clear, factor);
                    text.color = Color.Lerp(Color.white, Color.clear, factor);
                }
                else{
                    logo.transform.localScale = Vector3.one;
                    logo.color = normalColor;
                    state++;
                    timer = 0;
                }
            }
            else{
                float factor = 0.5f * Mathf.Sin(2*Mathf.PI * ( 1 / fluctuateSpeed  * timer - 0.25f)) + 0.5f;
                logo.transform.localScale = Vector3.Lerp(Vector3.one, logoScalePost, factor);
                logo.color = Color.Lerp(normalColor, interColor, factor);
                text.color = Color.Lerp(Color.white, normalColor, factor * 0.33f);
            }
        }


        timer += Time.deltaTime;
    }
}
