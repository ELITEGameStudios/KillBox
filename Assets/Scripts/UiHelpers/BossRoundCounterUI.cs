using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoundCounterUI : MonoBehaviour
{
    [SerializeField] private Image MainBg, backBg;
    [SerializeField] private Text counterText, supportingText;
    [SerializeField] private bool updateAnimation, fadeAnimation, faded;
    [SerializeField] private float mainTimer, updateAnimTime, fadeOutTime, textUpdateOffsset, initialTextSize;
    [SerializeField] public static BossRoundCounterUI main {get; private set;}
    [SerializeField] private Color updateTextColor, normalGraphicColor, updateGraphicColor, defaultBackBg, normalSupportTextColor;
    [SerializeField] private Vector3 updateScale;

    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        updateScale = new Vector3(textUpdateOffsset, textUpdateOffsset, textUpdateOffsset);
        defaultBackBg = backBg.color;
    
        counterText.rectTransform.localScale = Vector3.one;
        counterText.color = Color.white;
        MainBg.color = normalGraphicColor;
    }

    void Update()
    {
        if(updateAnimation){
            if(mainTimer > 0){
                counterText.rectTransform.localScale = Vector3.Lerp(Vector3.one, updateScale, mainTimer / updateAnimTime);
                counterText.color = Color.Lerp(Color.white, updateTextColor, mainTimer / updateAnimTime);
                MainBg.color = Color.Lerp(normalGraphicColor, updateGraphicColor, mainTimer / updateAnimTime);

                mainTimer -= Time.deltaTime;
            }
            else{
                counterText.rectTransform.localScale = Vector3.one;
                counterText.color = Color.white;
                MainBg.color = normalGraphicColor;
                updateAnimation = false;
            }
        }

        else if(fadeAnimation){
            if(mainTimer > 0){
                counterText.color = Color.Lerp(Color.clear, updateTextColor, mainTimer / fadeOutTime);
                supportingText.color = Color.Lerp(Color.clear, updateTextColor, mainTimer / fadeOutTime);
                MainBg.color = Color.Lerp(Color.clear, updateGraphicColor, mainTimer / fadeOutTime);
                backBg.color = Color.Lerp(Color.clear, defaultBackBg, mainTimer / fadeOutTime);
                mainTimer -= Time.deltaTime;
            }
            else{
                counterText.color = Color.clear;
                supportingText.color = Color.clear;
                MainBg.color = Color.clear;
                backBg.color = Color.clear;
                fadeAnimation = false;
                faded = true;
            }
        }
    }

    public void UpdateDisplay(bool fade){
        if(fade){
            if(!faded){
                mainTimer = fadeOutTime;
                fadeAnimation = true;
            }
        }
        else{
            int round = BossRoundManager.main.timeUntilNextBoss;
        
            counterText.text = round.ToString();
            
            counterText.rectTransform.localScale = updateScale;
            counterText.color = updateGraphicColor;
            MainBg.color = updateGraphicColor;

            mainTimer = updateAnimTime;
            updateAnimation = true;

            if(faded){
                backBg.color = defaultBackBg;
                supportingText.color = normalSupportTextColor;
                faded = false;
            }
        }
    }
}
