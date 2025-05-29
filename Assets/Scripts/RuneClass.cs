using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuneClass : MonoBehaviour
{

    private StateMachine states;
    private State _idle, _follow, _portal_int;

    private Transform _portal;
    private float _distance;

    [SerializeField] private bool isUpgradeRune;
    [SerializeField] private int mode, upgradeInt;


    [SerializeField] private GameObject claimParticleObject, idleParticleObject, glowGraphicObject;
    [SerializeField] private SpriteRenderer runeGraphicObject;

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

        if(_distance <= 2 && !isUpgradeRune){
            states.SwitchState(_portal_int);
        } 
        
    }

    public void OnPortalInteraction(){
        
        _portal.gameObject.GetComponent<PortalScript>().SetMode(mode);
        Destroy(gameObject);
    }

    public void ClaimUpgradeRune(){

        UpgradesManager.Instance.FreeUpgrade(upgradeInt);

        runeGraphicObject.enabled = false;
        glowGraphicObject.SetActive(false);
        idleParticleObject.SetActive(false);
        claimParticleObject.SetActive(true);

        Destroy(gameObject, 1f);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag == "Player"){
            if (isUpgradeRune) { ClaimUpgradeRune(); }
            else { states.SwitchState(_follow); }
        }
    }

}
