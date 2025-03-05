using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DetectInputDevice : MonoBehaviour
{
    public Keyboard keyboard {get; private set;}
    public Gamepad gamepad {get; private set;}
    public Mouse mouse {get; private set;}
    [SerializeField] private Vector2 rightStickValue, leftStickValue;
    public bool isKBM {get; private set;}
    public bool isController {get; private set;}

    public static DetectInputDevice main {get; private set;}

    // Start is called before the first frame update
    
    void Awake(){
        if(main == null){ main = this; }
        else if(main != this){Destroy(this);}

        // isKBM = true;
    }
    
    void Start()
    {
        keyboard = Keyboard.current;    
        gamepad = Gamepad.current;    
        mouse = Mouse.current;    
    }

    // Update is called once per frame
    void Update()
    {
        keyboard = Keyboard.current;    
        gamepad = Gamepad.current;    
        mouse = Mouse.current;    

        if(keyboard != null){
            if(keyboard.anyKey.isPressed && !isKBM){
                SwitchMode(0);
                return;
            }
            // Debug.Log("KBM detected");
        }
        if(mouse != null){
            if( (mouse.IsPressed() || mouse.IsActuated(0.15f)) && !isKBM){
            // if(mouse.IsPressed() && !isKBM){
                SwitchMode(0);
                return;
            }
            // Debug.Log("M detected");
        }
        if(gamepad != null){
            // if( (mouse.IsPressed() || mouse.IsActuated()) && !isKBM){
            rightStickValue = gamepad.rightStick.ReadValue();
            leftStickValue = gamepad.leftStick.ReadValue();
            rightStickValue = rightStickValue.magnitude > 0.15 ? rightStickValue : Vector2.zero;
            leftStickValue = leftStickValue.magnitude > 0.15 ? leftStickValue : Vector2.zero;

            // if( (gamepad.IsPressed() || gamepad.IsActuated(0.2f) ) && !isController){
            if( rightStickValue != Vector2.zero || leftStickValue != Vector2.zero ){
                SwitchMode(1);
                return;
            }
            // Debug.Log("Contr detected");
        }

        
    }

    void SwitchMode(int mode){
        if(mode == 0){
            isKBM = true;
            isController = false;
            Debug.Log("KBM");
        }
        else if(mode == 1){
            isKBM = false;
            isController = true;
            Debug.Log("Controller");
        }
    }
}
