using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour, IMainRoundEventListener
{
    [SerializeField] private bool open = false, opens_on_round_start, closes_on_round_end, reset_on_next_round, open_by_default, opens_on_round_end, roundEnd;
    [SerializeField] private bool started_round;
    [SerializeField] private GameObject door_object;
    [SerializeField] private Collider2D door_collider;
    [SerializeField] private Animator animator;
    [SerializeField] private UnityEvent OnNextRound;
    [SerializeField] private static bool updated_graph_call;

    public static List<Door> doors {get; private set;} = new List<Door>();
    void Awake(){ doors.Add(this); open = open_by_default; ChangeState();}
    void Update(){
        if(updated_graph_call){
            PortalScript.main.UpdatePathfinding();
            updated_graph_call = false;
        }

        EnforceAnimations();
        
    }
    public void Open(){ open = true; ChangeState();}
    public void Close(){ open = false; ChangeState();}
    public void Toggle(){ open = !open; ChangeState();}

    public void ChangeState(){ 
        if(door_collider == null){
            return;
        }

        door_collider.enabled = !open;
        if(open){
            animator.Play("Opening");
        }
        else{
            animator.Play("Closing");
        }

        // Debug.Log(open);
        updated_graph_call = true;
        
    }
    public void Interact(){ 
        animator.Play("Interact");
    }

    void EnforceAnimations(){
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Closed") && open){
            animator.Play("Opening");
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("Open") && !open){
            animator.Play("Closing");
        }
    }

    
    public void RoundStart(){ 
        if(opens_on_round_start){
            open = true; door_object.SetActive(false);
        }

        started_round = true;
    }

    public void RoundEnd(){ 
        if(closes_on_round_end){
            Close();
        }
        else if(opens_on_round_end && !IsOpen){
            Open();
        }

        roundEnd = true;
    }

    public void NextRound(){ 
        if(reset_on_next_round){
            open = open_by_default;
            ChangeState();
        }

        OnNextRound.Invoke();
        started_round = false;
        roundEnd = false;
    }

    public void OnDestroy(){
        doors.Remove(this);
        
    }

    public void OnRoundChange()
    {
        NextRound();
    }

    public void OnRoundStart()
    {
        RoundStart();
    }

    public void OnRoundEnd()
    {
        RoundEnd();
    }

    public void OnPortalInteract()
    {
        
    }

    public bool ClosesOnNextRound{
        get {return reset_on_next_round;}
        set {}
    }

    public bool IsOpen 
    { 
        get => open;
    }
    public bool OpenByDefault { get => open_by_default; }
    public bool RoundEnded{ get => roundEnd; }
    public bool OpensOnRoundEnd{ get => opens_on_round_end; }
    public bool ClosesOnRoundEnd{ get => closes_on_round_end; }
}