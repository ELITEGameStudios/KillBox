using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneIdle : State
{
    public StateMachine _state_machine {get; private set;}
    public GameObject _object {get; private set;}
    
    public override void OnInit() { }
    public override void OnUpdate() { }
    public override void OnEnter() { }
    public override void OnExit() { }

    public RuneIdle(GameObject obj, StateMachine stateMachine){
        _object = obj;
        _state_machine = stateMachine;
        OnInit();
    }
}

public class RuneFollow : State
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

    public RuneFollow(GameObject obj, StateMachine stateMachine){
        _object = obj;
        _state_machine = stateMachine;
        OnInit();
    }
}
public class RunePortalInteract : State
{
    public StateMachine _state_machine {get; private set;}
    public GameObject _object {get; private set;}
    private Transform portal;
    private Vector3 target;

    private float timer = 1.05f;

    private float smoothing = 5;
    
    public override void OnInit() { 

    }
    
    public override void OnUpdate() { 

        portal = GameObject.FindGameObjectWithTag("Portal").transform;
        target = portal.position;
        target+= Vector3.up * 1.5f;


        Vector2 target2Dpos = new Vector2(target.x, target.y);
        Vector2 self2Dpos = new Vector2(_object.transform.position.x, _object.transform.position.y);

        Vector2 target_pos = Vector2.Lerp(self2Dpos, target2Dpos, smoothing * Time.deltaTime);
        _object.transform.position = new Vector3(target_pos.x, target_pos.y, _object.transform.position.z);


        timer -= Time.deltaTime;

        if(timer <= 0){
            _object.GetComponent<RuneClass>().OnPortalInteraction();
        }
    }


    public override void OnEnter() { 
        _object.GetComponent<Animator>().Play("portal_interaction");

    }

    public override void OnExit() { }

    public RunePortalInteract(GameObject obj, StateMachine stateMachine){
        _object = obj;
        _state_machine = stateMachine;
        OnInit();
    }
}
