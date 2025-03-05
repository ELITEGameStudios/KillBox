using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public State current_state {get; private set;}
    public GameObject root {get; private set;}

    public void Update(){
        current_state.OnUpdate();
    }

    public void SwitchState(State _new_state){

        current_state.OnExit();
        current_state = _new_state;
        current_state.OnEnter();

    }

    public void SetFirstState(State _new_state){

        current_state = _new_state;
        current_state.OnEnter();
    }

    public StateMachine(GameObject _root){
        root = _root;
    }
}
