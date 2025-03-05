using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneFortressIdle : State
{
    public StateMachine _state_machine {get; private set;}
    public GameObject _object {get; private set;}
    
    public override void OnInit() { }
    public override void OnUpdate() { }
    public override void OnEnter() { }
    public override void OnExit() { }

    public RuneFortressIdle(GameObject obj, StateMachine stateMachine){
        _object = obj;
        _state_machine = stateMachine;
        OnInit();
    }
}

public class RuneFortressAppear : State
{
    public StateMachine _state_machine {get; private set;}
    public GameObject _object {get; private set;}
    public Animator anim {get; private set;}
    
    public override void OnInit() { 
    }
    
    public override void OnUpdate() { 
    }

    public override void OnEnter() { 
        anim.Play("Appear");
    }

    public override void OnExit() { }

    public RuneFortressAppear(GameObject obj, StateMachine stateMachine, Animator _anim){
        _object = obj;
        _state_machine = stateMachine;
        anim = _anim;
        OnInit();
    }
}

public class RuneFortressActive : State
{
    public StateMachine _state_machine {get; private set;}
    public GameObject _object {get; private set;}
    
    private Vector3 target = new Vector3(1, 1, 1);
    private float timer = 0.5f;

    private float smoothing = 5;
    private bool is_full_sized = false;
    
    public override void OnInit() { 

    }
    
    public override void OnUpdate() { 
        // scale to go to 1.0

        if(timer <= 0 && !is_full_sized){
            //_object.GetComponent<RuneClass>().OnPortalInteraction();
            _object.transform.localScale = new Vector3(1, 1, 1);
            _object.GetComponent<Animator>().Play("portal_interaction");

            is_full_sized = true;
        }
        else{
            
        }
    }


    public override void OnEnter() { 
    
    }

    public override void OnExit() { 

    }

    public RuneFortressActive(GameObject obj, StateMachine stateMachine){
        _object = obj;
        _state_machine = stateMachine;
        OnInit();
    }
}

public class RuneFortressFollow : State
{
    public StateMachine _state_machine {get; private set;}
    public GameObject _object {get; private set;}
    private Transform player;
    private Vector3 target;

    private float smoothing = 5;
    
    public override void OnInit() { 

    }
    
    public override void OnUpdate() { 

        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = player.position;
        target+= Vector3.up;


        Vector2 target2Dpos = new Vector2(target.x, target.y);
        Vector2 self2Dpos = new Vector2(_object.transform.position.x, _object.transform.position.y);

        Vector2 target_pos = Vector2.Lerp(self2Dpos, target2Dpos, smoothing * Time.deltaTime);
        _object.transform.position = new Vector3(target_pos.x, target_pos.y, _object.transform.position.z);

    }


    public override void OnEnter() { }
    public override void OnExit() { }

    public RuneFortressFollow(GameObject obj, StateMachine stateMachine){
        _object = obj;
        _state_machine = stateMachine;
        OnInit();
    }
}
