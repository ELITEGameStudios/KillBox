using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillboxJoystickManager : MonoBehaviour
{
    public Joystick MoveJoystick, RotJoystick;
    public Touch moveTouch, rotTouch;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Input.touchCount);

        for (int i = 0; i < Input.touchCount; i++)
        {
            if (Input.GetTouch(i).position.x <= (float)Screen.width/2.0f)
            {
                moveTouch = Input.GetTouch(i);
            }
            else{
                rotTouch = Input.GetTouch(i);
            }
        }

        if(moveTouch.phase == TouchPhase.Began){

            MoveJoystick.gameObject.SetActive(true);
            //set joystick to the touch position
            MoveJoystick.gameObject.transform.position = new Vector3(moveTouch.position.x, moveTouch.position.y);
        }


        if(rotTouch.phase == TouchPhase.Began){

            RotJoystick.gameObject.SetActive(true);
            //set joystick to the touch position
            RotJoystick.gameObject.transform.position = new Vector3(rotTouch.position.x, rotTouch.position.y, RotJoystick.gameObject.transform.position.z);
        }
    }
}
