using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeManager : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private bool toVisible;
    [SerializeField] private AnimationCurve curve;
    [SerializeField] private float targetTime, timer;

    public bool visible {get{return panel.color.a > 0;}}
    public bool GetTarget(){return toVisible;}
    public bool fullVisible {get{return panel.color == Color.black;}}
    public static FadeManager instance {get; private set;}

    void Awake(){
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(gameObject);}
    }

    void Update(){

        if((visible && !toVisible) || (toVisible && !fullVisible)){
            // panel.color = Color.Lerp(Color.clear, Color.black, timer/targetTime);
            panel.color = Color.Lerp(Color.clear, Color.black, curve.Evaluate(timer/targetTime));

            if(visible && !toVisible){timer -= Time.deltaTime;}
            else if(timer < targetTime){timer += Time.deltaTime;}        
        }

        if(visible){ 
            if(!panel.gameObject.activeInHierarchy) {panel.gameObject.SetActive(true);}
            if(!fullVisible && timer >= targetTime){ panel.color = Color.black; } 
        }
        else if(panel.gameObject.activeInHierarchy) {panel.gameObject.SetActive(false);}
    }

    public void SetTarget(bool toVisible, float time = -1, bool forceResetTimer = false){
        this.toVisible = toVisible;
        if(time >= 0){targetTime = time;}
        if(forceResetTimer){timer = toVisible ? 0 : targetTime;}
    }

    public void ForceState(bool visible){
        if(visible){
            timer = targetTime;
            panel.color = Color.black;    
            panel.gameObject.SetActive(true);
        }
        else{
            timer = 0;
            panel.color = Color.clear;    
            panel.gameObject.SetActive(false);
        }
    }
}
