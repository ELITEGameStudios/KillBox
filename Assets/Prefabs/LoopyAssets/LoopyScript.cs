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

    // Static singleton setup
    public static LoopyScript[] instances { get; private set; }
    public static LoopyScript main {get{ return instances[0]; }}
    public static LoopyScript upgrades {get{ return instances[1]; }}
    public static LoopyScript weapons {get{ return instances[2]; }}
    public static LoopyScript inGame {get{ return instances[3]; }}
    

    public LoopyState currentState { get; private set; }
    public LoopyState nextState;
    public LoopyInstance loopyType;


    void Awake(){
        if(instances == null){
            instances = new LoopyScript[4]; // count of types
        }


        if (instances[(int)loopyType] == null) instances[(int)loopyType] = this;
        else if (instances[(int)loopyType] != this) Destroy(this);


        stateQueue = new();

        // Plays the startup state
        if (main == this)
        {
            AddState(
                new LoopyState(
                    LoopyPose.HAPPY, 
                    "Hello There, Welcome to the KillBox!\nI'm LOOPY, and I am here to help you on your way!", 2)
                );
            
        }
    }

    void SetState(LoopyState state){

        currentState = state;
        // image.sprite = poseSprite[(int)state.pose];

        // anim.Play((int)state.pose);
        anim.SetFloat("Blend", 0.3f * (int)state.pose);
        anim.SetBool("Active", true);

        image.color = poseColor[(int)state.pose];
        glow.color = Color.Lerp(Color.clear, poseColor[(int)state.pose], 0.66f);
        textElement.color = poseColor[(int)state.pose];

        StopAllCoroutines();

        StartCoroutine(TextDisplayCoroutine());
        
        visibleTime =
            main == this || inGame == this
            ? state.lifetime 
            : Mathf.Infinity;
        
        status = Status.ACTIVE;
    }

    public void AddState(LoopyState state, bool priority = false){
        // Adds a state to the queue
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
        // Sets the state to the next in line
        Debug.Log("checking queue");
        
        if (stateQueue.Count > 0)
        {
            Debug.Log("something is in queue");
            nextState = stateQueue.Dequeue();
            switch (status)
            {
                case Status.INACTIVE:
                    // Plays intro if was inactive
                    Debug.Log("awakening");
                    StartCoroutine(IntroSetStateCoroutine(nextState));
                    break;

                case Status.ACTIVE:
                    // Sets to next state
                    SetState(nextState);
                    break;

                // Not sure if this is nessecary...
                // case Status.OUTRO:
                //     // Plays outro
                //     StopAllCoroutines();
                //     SetState(nextState);
                //     break;
            }
        }
        else
        {
            nextState = null;

            // Plays outro if queue is empty
            StartCoroutine(OutroStateCoroutine());
            status = Status.OUTRO;
        }
    }

    public void SetToNextStateSprite(){
        image.sprite = poseSprite[(int)nextState.pose];
    }

    public IEnumerator IntroSetStateCoroutine(LoopyState newState)
    {
        anim.SetFloat("Blend", 0.3f * (int)newState.pose);

        // Plays intro 
        ToggleElements(true);
        
        anim.SetBool("Active", true);
        anim.SetTrigger("Intro");
        
        yield return new WaitForSeconds(introTime);
        // anim.StopPlayback();
        SetState(newState);
    }

    public IEnumerator OutroStateCoroutine(){
        anim.SetBool("Active", false);
        // anim.Play(outroAnimName);
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

public enum LoopyPose
{
    NEUTRAL,
    HAPPY,
    SAD,
    ANGRY,
    DEMENTED
}

public enum LoopyInstance{
    MAIN, // Game UI
    UPGRADESMENU,
    WEAPONSMENU,
    INGAME,
}