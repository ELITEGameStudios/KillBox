using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TokenScriptedAnimation : MonoBehaviour
{

    [SerializeField] private float animTime, timer;
    [SerializeField] private bool finishedAnim;
    [SerializeField] private UnityEvent onFinishedAnim;
    [SerializeField] private Transform tf;
    [SerializeField] private float initAngle;
    [SerializeField] private Vector3 initScale, initPos;


    // Start is called before the first frame update
    void Awake()
    {
        initAngle = tf.eulerAngles.z;
        initScale = tf.localScale;
        initPos = tf.localPosition;
        timer = animTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 0 && !finishedAnim){

            tf.eulerAngles = new Vector3(0, 0, Mathf.Lerp(initAngle, initAngle+90, CommonFunctions.SineEase(timer/animTime)));
            tf.localScale = Vector3.Lerp(initScale, Vector3.zero, CommonFunctions.SineEase(timer/animTime));
            tf.localPosition = Vector3.Lerp(initPos, initPos - new Vector3(0, 3, 0), CommonFunctions.SineEase(timer/animTime));
            timer -= Time.deltaTime;
        }
        else if(!finishedAnim){
            finishedAnim = true;
            tf.eulerAngles = tf.eulerAngles = new Vector3(0, 0, initAngle);
            tf.localScale = initScale;
            tf.localPosition = initPos;

            onFinishedAnim.Invoke();

        }
    }
}
