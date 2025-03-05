using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InGameButtonHandler : MonoBehaviour, IShopButtonStateListener, IMainRoundEventListener
{

    [SerializeField] private GameObject childButtonObj, buttonPrefab;
    [SerializeField] private Button child_button;
    [SerializeField] private float displayRange;
    [SerializeField] private bool active, anyShopButton;
    [SerializeField] private sine_movement movement;

    [SerializeField] private UnityEvent onClickEvent, onActivate, onDeactivate;

    // Start is called before the first frame update
    void Awake()
    {
        
        childButtonObj = Instantiate(buttonPrefab, transform);

        childButtonObj.SetActive(false);
        childButtonObj.transform.localPosition = Vector2.zero;
        childButtonObj.transform.SetParent(GameObject.Find("WorldUiCanvas").transform);
        childButtonObj.transform.Translate(0, 2, 0);
        childButtonObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        // child_button = childButtonObj.GetComponent<Button>();
        // child_button.interactable = false;

        childButtonObj.GetComponent<InGamePreGameButtonEventHelper>().SetHandler(this);
        // childButtonObj.transform.GetChild(0).gameObject.GetComponent<Text>().text = "Open Chest\n-" + cost.ToString() + " tokens";
    }

    public void Activate(){active = true; onActivate.Invoke();}
    public void Deactivate(){active = false; onDeactivate.Invoke();}
    

    void Update(){
        if(active){
            float distance = Vector2.Distance(Player.main.tf.position, transform.position);
            if(distance <= displayRange){
                if(!childButtonObj.activeInHierarchy) {
                    childButtonObj.SetActive(true);
                    childButtonObj.transform.position = transform.position;
                    childButtonObj.transform.Translate(0, 2, 0);
                
                }
                
                else if(CustomKeybinds.main.PressingInteract()){
                    InvokeObject();
                }
                // if(Input.GetKeyDown(CustomKeybinds.main.Interact)){
                //     InvokeObject();
                // }
                // else if(CustomKeybinds.main.PressingInteract()){
                //     InvokeObject();
                // }
            }
            else{
                if(childButtonObj.activeInHierarchy) {childButtonObj.SetActive(false);}

            }
        }
        else{
            if(childButtonObj.activeInHierarchy) {childButtonObj.SetActive(false);}
        }
    }

    public void InvokeObject(){
        onClickEvent.Invoke();
        if(anyShopButton){
            KillboxEventSystem.TriggerOpenShopEvent(0);
        }
    }

    public void InvokeEvent(int eventId){
        KillboxEventSystem.TriggerOpenShopEvent(eventId);
    }

    void OnDisable(){
        childButtonObj.SetActive(false);
    }

    public void OnAnimationFinished()
    {
        movement.SetRootPos();
        movement.enabled = true;
    }

    public void OnRoundChange()
    {
        movement.enabled = false;
    }

    public void OnRoundStart()
    {
        movement.enabled = false;
    }

    public void OnRoundEnd()
    {}

    public void OnPortalInteract()
    {}
}
