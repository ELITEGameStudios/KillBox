using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBgManager : MonoBehaviour
{
    [SerializeField] Camera cam;
    public Color defaultBg;
    [SerializeField] Color targetBg, lastBg;
    [SerializeField] float timer, time;
    public bool transitioning;
    public static CameraBgManager instance {get; private set;}



    void Awake() {
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
        targetBg = defaultBg;
    }


    void Update(){
        if(transitioning){
            if(timer <= 0){
                cam.backgroundColor = targetBg;
                transitioning = false;
            }
            else{
                cam.backgroundColor = Color.Lerp(targetBg, lastBg, timer/time);
                timer -= Time.deltaTime;
            }
        }
    }

    public void SetBackground(Color color, float time){
        timer = time;
        this.time = time;
        targetBg = color;
        lastBg = cam.backgroundColor;
        transitioning = true;
    }
}
