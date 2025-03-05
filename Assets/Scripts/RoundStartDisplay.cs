using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundStartDisplay : MonoBehaviour
{
    [SerializeField] private Text text;
    [SerializeField] private Image bgImage, glow;
    public static RoundStartDisplay main;
    [SerializeField] private float timer, animTime;
    public bool isPlaying {get; private set;}
    [SerializeField] private Vector3 startScale, endScale;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Color glowColor;
    [SerializeField] private Color bgColor;

    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if(isPlaying){
            if(timer < animTime){
                text.gameObject.SetActive(true);
                glow.gameObject.SetActive(true);
                bgImage.enabled = true;
                text.transform.localScale = Vector3.Lerp(startScale, endScale, timer/animTime);
                glow.color = Color.Lerp(Color.clear, glowColor, CommonFunctions.WarningIndicator(1/animTime, timer));
                bgImage.color = Color.Lerp(Color.clear, bgColor, CommonFunctions.WarningIndicator(1/animTime, timer));
            }
            else{
                text.transform.localScale = endScale;
                glow.color = Color.clear;
                bgImage.color = Color.clear;
                isPlaying = false;

                text.gameObject.SetActive(false);
                glow.gameObject.SetActive(false);
                bgImage.enabled = false;
            }
            timer += Time.deltaTime;
        }
    }

    public void StartAnimation(){
        isPlaying = true;
        timer = 0;

        // if(isBoss){text.text = "BOSS TIME :)";}
        // else{text.text = "ROUND "+ GameManager.main.LvlCount.ToString();}
        text.text = "ROUND "+ GameManager.main.LvlCount.ToString();
    }
}
