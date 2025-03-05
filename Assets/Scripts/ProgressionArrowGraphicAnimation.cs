using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressionArrowGraphicAnimation : MonoBehaviour
{
    [SerializeField] private Image[] arrows;
    [SerializeField] private Color[] colors;
    [SerializeField] private Color targetColor, startColor;
    [SerializeField] private float loopTime;
    [SerializeField] private bool customOffset;
    [SerializeField] private float offset;
    [SerializeField] private float time;
    

    void Awake(){
        
        if(!customOffset){ 
            offset = loopTime / colors.Length; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < colors.Length; i++)
        {
            float x = time - (i*offset);

            colors[i] =  (int)(0.5 * Mathf.Sin( (x * Mathf.PI) / loopTime  ) + 1) == 1 ? 
                Color.Lerp(startColor, targetColor,  CommonFunctions.SineEase( x, loopTime )) :
                Color.Lerp(startColor, targetColor,  CommonFunctions.CosineEase( x, loopTime, 0));

            arrows[i].color = colors[i];

        }

        time += Time.unscaledDeltaTime;
    }

    public void SetTimings(float newLoopTime, bool customOffset = false, float offset = 0.1f){

        loopTime = newLoopTime;
        if(customOffset){ this.offset = offset; }
        else{ this.offset = loopTime / colors.Length; }

    }
}
