using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TokenUI : MonoBehaviour
{
    // [SerializeField] private SpriteRenderer tokenImage;
    [SerializeField] private Text tokenPickupText;
    [SerializeField] private Gradient pickupTextGradient;
    [SerializeField] private Color currentColor;
    [SerializeField] private bool isPlaying;
    [SerializeField] private float animationTime, animationTimer;
    [SerializeField] private int tokenCount, countUntilMaxColor;
    
    
    public static TokenUI main {get; private set;}

    // Start is called before the first frame update
    void Awake()
    {  
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        tokenCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlaying){
            if(animationTimer <= 0){
                tokenCount = 0;
                tokenPickupText.color = Color.clear;
                isPlaying = false;
            }
            else{

                tokenPickupText.color = Color.Lerp(Color.clear, currentColor, animationTimer / animationTime);
                animationTimer -= Time.deltaTime;
            }
        }
    }

    public void InitPickupAnimation(int tokenCount){
        if(!isPlaying){ isPlaying = true; }

        this.tokenCount += tokenCount;
        animationTimer = animationTime;
        currentColor = pickupTextGradient.Evaluate(  (float)(this.tokenCount / (float)countUntilMaxColor)  );
        tokenPickupText.text = "+"+this.tokenCount.ToString();
    }
}
