using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseHandler : MonoBehaviour
{
    private float fixedDeltaTime;
    [SerializeField] private GameObject pause_menu, pauseButton;
    public bool paused {get; private set;}
    public static PauseHandler main {get; private set;}
    // Start is called before the first frame update
    void Awake()
    {
        this.fixedDeltaTime = Time.fixedDeltaTime;

        if(main == null){
            main = this;
        }
        else{
            Destroy(this);
        }
    }

    // Update is called once per frame
    public void PausePlay(float speed)
    {
        if(GameManager.main.started_game){

            Time.timeScale = speed;
            Time.fixedDeltaTime = this.fixedDeltaTime * Time.timeScale;
            
            if(Time.timeScale == 0){
                paused = true;
                GunHandler.Instance.SetUIStatus(true);
            }
            else{
                paused = false;
            }

            pause_menu.SetActive(paused);
            pauseButton.SetActive(!paused);
            
            ChallengeSaveSystem.SaveChallenges();
        }
        else{
            paused = false;
            pause_menu.SetActive(paused);
            pauseButton.SetActive(!paused);
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if(GameManager.main.started_game){
            //if(!hasFocus)
            //    PausePlay(0);
            //else
            //    PausePlay(1);
            //PausePlay(0);
        }
    }
}
