using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ChannelButton : MonoBehaviour
{
    [SerializeField] private int channel;
    [SerializeField] private float resetTime, timer, maxDisplayRange, minDisplayRange;
    [SerializeField] private Text channelDisplay;
    [SerializeField] private SpriteRenderer buttonRenderer;
    [SerializeField] private Color defaultButtonColor, defaultDisplayColor;
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent OnNextRound, onClick, onResetClick;
    [SerializeField] private bool clicked = false, reset_on_next_round;
    [SerializeField] private bool started_round, toggleChannel, setChannelStatus;

    public static List<ChannelButton> buttons {get; private set;} = new List<ChannelButton>();

    void Awake(){ 
        buttons.Add(this); 
        timer = resetTime; 
        if(channelDisplay != null){
            channelDisplay.text = channel.ToString();
            defaultDisplayColor = channelDisplay.color;
        }
    }

    void Update(){
        if(clicked){
            if(timer <= 0){ ResetClickedState(); }
            else{ timer -= Time.deltaTime; }
        }

        if(channelDisplay != null && maxDisplayRange > 0 ){
            channelDisplay.color = Color.Lerp(defaultDisplayColor, Color.clear, 
                (Vector2.Distance(transform.position, Player.main.tf.position) - minDisplayRange) / (maxDisplayRange - minDisplayRange));
        }
        // EnforceAnimations();
    }
    public void Interact(){ 
        if(!clicked){
            clicked = true;

            if(toggleChannel){ ToggleChannelManager.main.ToggleChannel(channel); }
            else{ToggleChannelManager.main.SetChannel(channel, setChannelStatus); }

            ChangeState();
        }
    }
    public void ResetClickedState(){ 
        timer = resetTime; 
        clicked = false; 
        ChangeState();
    }
    // public void Toggle(){ clicked = !clicked; ChangeState();}

    public void ChangeState(){ 

        if(clicked){
            animator.Play("Click");
            if(buttonRenderer != null){buttonRenderer.color = Color.Lerp(defaultButtonColor, Color.black, 0.4f);}
            onClick.Invoke();
        }
        else{
            onResetClick.Invoke();
            if(buttonRenderer != null){buttonRenderer.color = defaultButtonColor;}
            // animator.Play("Closing");
        }   
    }

    void EnforceAnimations(){
        // if(animator.GetCurrentAnimatorStateInfo(0).IsName("Closed") && clicked){
        //     animator.Play("Opening");
        // }
        // else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Open") && !clicked){
        //     animator.Play("Closing");
        // }
    }

    
    public void RoundStart(){ 
        started_round = true;
    }

    public void NextRound(){ 
        if(reset_on_next_round){
            ResetClickedState();
        }

        OnNextRound.Invoke();
        started_round = false;
    }

    public void OnDestroy(){ buttons.Remove(this); }

    public bool ResetsOnNextRound{
        get {return reset_on_next_round;}
    }

    public bool IsInteractable {  get => !clicked; }
}