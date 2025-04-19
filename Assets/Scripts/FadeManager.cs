using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Graphic[] graphics;
    [SerializeField] private Color[] colors;
    [SerializeField] private bool toVisible;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float targetTime, timer;

    public bool visible {get{return graphics[0].color.a > 0;}}
    public bool GetTarget(){return toVisible;}
    public bool fullVisible {get{return graphics[0].color == colors[0];}}
    public static FadeManager instance {get; private set;}

    void Awake(){
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(gameObject);}
        colors = new Color[graphics.Length];
        for (int i = 0; i < graphics.Length; i++){
            colors[i] = graphics[i].color;
        }
    }

    void Update(){

        if((visible && !toVisible) || (toVisible && !fullVisible)){
            // graphics.color = Color.Lerp(Color.clear, Color.black, timer/targetTime);
            for (int i = 0; i < graphics.Length; i++){
                graphics[i].color = Color.Lerp(Color.clear, colors[i], curve.Evaluate(timer/targetTime));
            }

            if(visible && !toVisible){timer -= Time.deltaTime;}
            else if(timer < targetTime){timer += Time.deltaTime;}        
        }

        if(visible){ 
            if(!graphics[0].gameObject.activeInHierarchy) {
                foreach (Graphic graphic in graphics){
                    graphic.gameObject.SetActive(true);
                }
            }
            if(!fullVisible && timer >= targetTime){ 
                for (int i = 0; i < graphics.Length; i++){
                    graphics[i].color = colors[i];
                }
            } 
        }
        else if(graphics[0].gameObject.activeInHierarchy) {
            foreach (Graphic graphic in graphics){
                graphic.gameObject.SetActive(false); 
            }
        }
    }

    public void SetTarget(bool toVisible, float time = -1, bool forceResetTimer = false){
        this.toVisible = toVisible;
        if(time >= 0){targetTime = time;}
        if(forceResetTimer){timer = toVisible ? 0 : targetTime;}
    }

    public void ForceState(bool visible){
        if(visible){
            timer = targetTime;
            for (int i = 0; i < graphics.Length; i++){
                graphics[i].color = colors[i];
                graphics[i].gameObject.SetActive(true);
            }
        }
        else{
            timer = 0;
            for (int i = 0; i < graphics.Length; i++){
                graphics[i].color = Color.clear;
                graphics[i].gameObject.SetActive(false);
            }  
        }
    }
}
