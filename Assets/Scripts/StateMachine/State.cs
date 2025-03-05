using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    //public virtual GameObject _object {get; private set;}
    public virtual void OnInit() { }
    public virtual void OnUpdate() { }
    public virtual void OnEnter() { }
    public virtual void OnExit() { }

    //public State(GameObject obj){
    //    _object = obj;
    //    OnInit();
    //}
}

// Abstraction ig