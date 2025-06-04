using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoopyScript : MonoBehaviour
{
    [SerializeField] private Image image, glow;
    [SerializeField] private TMP_Text textElement;
    [SerializeField] private Sprite[] poseSprite;
    [SerializeField] private Color[] poseColor;
    [SerializeField] private Status status = Status.INACTIVE;
    [SerializeField] private Queue<LoopyState> stateQueue;
    [SerializeField] private Animator anim;
    [SerializeField] private string introAnimName, outroAnimName;
    [SerializeField] private string[] stateAnimName;

    private float introTime = 0.5f, outroTime = 0.5f;
    public float visibleTime {get; private set;} = 0;
    public float scrollTime {get; private set;} = 0;
    public float scrollTimer {get; private set;} = 0;

    private bool isVisible {get{return visibleTime > 0;}}

    public static LoopyScript main {get; private set;}
    public LoopyState currentState {get; private set;}


    void Awake(){
        if(main == null) main = this;
        else if(main != this) Destroy(this);
        stateQueue = new();
        AddState(
            new LoopyState(
                LoopyPose.HAPPY, 
                "Hello There, Welcome to the KillBox!\nI'm LOOPY, and I am here to help you on your way!",
                4)
            );
    }

    void SetState(LoopyState state){
        currentState = state;
        // image.sprite = poseSprite[(int)state.pose];
        anim.Play((int)state.pose);
        image.color = poseColor[(int)state.pose];
        glow.color = Color.Lerp(Color.clear, poseColor[(int)state.pose], 0.66f);
        textElement.color = poseColor[(int)state.pose];

        StartCoroutine(TextDisplayCoroutine());
        
        visibleTime = state.lifetime;
        status = Status.ACTIVE;
    }

    void AddState(LoopyState state, bool priority = false){
        Debug.Log("adding state");
        if(priority){stateQueue = new Queue<LoopyState>();}
        stateQueue.Enqueue(state);
        CheckQueue();
    }

    void Update(){
        if(status == Status.ACTIVE){
            if(isVisible) visibleTime -= Time.deltaTime;
            else{  CheckQueue();  }
        }
    }

    void CheckQueue(){
        Debug.Log("checking queue");
        if(stateQueue.Count > 0){
            Debug.Log("something is in queue");
            LoopyState nextState = stateQueue.Dequeue();
            switch(status){
                case Status.INACTIVE: 
                    Debug.Log("awakening");
                    StartCoroutine(IntroSetStateCoroutine(nextState));
                    break;
                case Status.ACTIVE: 
                    SetState(nextState); 
                    break;
                case Status.OUTRO:
                    StopAllCoroutines(); 
                    SetState(nextState); 
                    break;
            }
        }
        else{
            StartCoroutine(OutroStateCoroutine());
            status = Status.OUTRO;
        }
    }

    public IEnumerator IntroSetStateCoroutine(LoopyState newState){
        ToggleElements(true);
        anim.Play(introAnimName);
        yield return new WaitForSeconds(introTime);
        // anim.StopPlayback();
        SetState(newState);
    }

    public IEnumerator OutroStateCoroutine(){
        anim.Play(outroAnimName);
        yield return new WaitForSeconds(outroTime);
        ToggleElements(false);
    }

    void ToggleElements(bool active){
        image.enabled = active;
        glow.enabled = active;
        textElement.enabled = active;

        if(active){
            image.color = poseColor[0];
            glow.color = Color.Lerp(Color.clear, poseColor[0], 0.66f);
            textElement.color = poseColor[0];
        }

        status = active ?
            Status.INTRO :
            Status.INACTIVE;
    }

    public IEnumerator TextDisplayCoroutine(){
        int length = currentState.text.Length;
        float timeBetweenCharacters = currentState.scrollTime / length;

        textElement.text = "";
        for (int i = 0; i < length; i++){
            char nextCharacter = currentState.text[i];
            textElement.text += nextCharacter;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
        
        textElement.text = currentState.text;
    }

    enum Status{
        INACTIVE,
        ACTIVE,
        INTRO,
        OUTRO,
    }
}

[System.Serializable]
public class LoopyState{
    public LoopyState(LoopyPose pose, string text, float scrollTime = 0, float lifetime = 5){
        this.pose = pose;
        this.text = text;
        this.scrollTime = scrollTime;
        this.lifetime = lifetime;
    }

    public LoopyPose pose;
    public string text;
    public float scrollTime, lifetime;
}

public enum LoopyPose{
    NEUTRAL,
    HAPPY,
    SAD,
    ANGRY,
    DEMENTED
}