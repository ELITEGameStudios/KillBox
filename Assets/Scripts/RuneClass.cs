using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneClass : MonoBehaviour
{

    private StateMachine states;
    private State _idle, _follow, _portal_int;

    private Transform _portal;
    private float _distance;

    [SerializeField]
    private int mode;

    // Start is called before the first frame update
    void Awake()
    {
        states = new StateMachine(gameObject);

        _idle = new RuneIdle(gameObject, states);
        _follow = new RuneFollow(gameObject, states);
        _portal_int= new RunePortalInteract(gameObject, states);

        states.SetFirstState(_idle);

        _portal = GameObject.FindGameObjectWithTag("Portal").transform;
    }

    // Update is called once per frame
    void Update()
    {
        states.Update();

        _distance = Vector3.Distance(transform.position, _portal.position);

        if(_distance <= 2){
            states.SwitchState(_portal_int);
        } 
        
    }

    public void OnPortalInteraction(){
        
        _portal.gameObject.GetComponent<PortalScript>().SetMode(mode);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player"){
            states.SwitchState(_follow);
        }
    }

}
