using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TpScript : MonoBehaviour
{
    public int id, targetId;
    public float distance, activate_timer;
    public GameObject Player, self;
    public GameObject[] Portals;
    public bool IsActive, activate;
    public TpScript OtherScript;
    public PortalScript portalScript;

    public UnityEvent OnPortalEnter;
    private Vector3 normal_scale;

    [SerializeField] private bool activated_by_trigger, no_animations; 

    // Start is called before the first frame update
    void Start()
    {
        normal_scale = transform.localScale;
        self = gameObject;
        portalScript = GameObject.Find("Portal").GetComponent<PortalScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(activate){
            if(activate_timer <= 0){
                activate = false;
                IsActive = true;

                if(!no_animations){
                    StartCoroutine("ReAppear");
                }
            }
            else
                activate_timer -= Time.deltaTime;
        }

        Player = GameObject.FindWithTag("Player");
        if(Player == null){
            return;
        }
        
        
        distance = Vector3.Distance(Player.transform.position, self.transform.position);
        if(distance < 0.5 &&  !activated_by_trigger)
        {
            for(int i = 0; i < Portals.Length; i++)
            {
                OtherScript = Portals[i].GetComponent<TpScript>();
                if (targetId == OtherScript.id && IsActive)
                {
                    Player.transform.position = Portals[i].transform.position;
                    IsActive = false;
                    OtherScript.IsActive = false;
                    StrtActivate();
                    OtherScript.StrtActivate();
                    OnPortalEnter.Invoke();
                }
            }
        }
//        if (portalScript.loadingScene && !portalScript.loadedScene)
//            IsActive = false;
//        if(portalScript.loadedScene)
//            IsActive = true;

        
    }
    public void StrtActivate()
    {
        activate = true;
        if(!activated_by_trigger){
            activate_timer = 5;
        }
        else{
            activate_timer = 2;
        }

        if(!no_animations){
            StartCoroutine("Activate");
        }
    }
    public IEnumerator Activate()
    {
        float value = 1;
        Vector3 Scale = transform.localScale;

        StopCoroutine("ReAppear");

        while ( value > 0)
        {
            
            transform.localScale = new Vector3(Scale.x * value, Scale.y * value, Scale.z * value);
            value -= Time.deltaTime;
            yield return null;
        }

        

    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "Player" && activated_by_trigger){
            for(int i = 0; i < Portals.Length; i++)
            {
                OtherScript = Portals[i].GetComponent<TpScript>();
                if (targetId == OtherScript.id && IsActive)
                {
                    Player.transform.position = Portals[i].transform.GetChild(0).position;
                    IsActive = false;
                    OtherScript.IsActive = false;
                    StrtActivate();
                    OtherScript.StrtActivate();
                    OnPortalEnter.Invoke();
                }
            }
        }
    }
        void OnTriggerStay2D(Collider2D collider){
        if(collider.tag == "Player" && activated_by_trigger){
            for(int i = 0; i < Portals.Length; i++)
            {
                OtherScript = Portals[i].GetComponent<TpScript>();
                if (targetId == OtherScript.id && IsActive)
                {
                    Player.transform.position = Portals[i].transform.GetChild(0).position;
                    IsActive = false;
                    OtherScript.IsActive = false;
                    StrtActivate();
                    OtherScript.StrtActivate();
                    OnPortalEnter.Invoke();
                }
            }
        }
    }

    public IEnumerator ReAppear()
    {
        float value = 0;

        while( value < 1)
        {
            transform.localScale = new Vector3(normal_scale.x * value, normal_scale.y * value, normal_scale.z * value);
            value += Time.deltaTime;
            yield return null;
        }
        transform.localScale = normal_scale;

    }
}
