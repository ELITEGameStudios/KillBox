using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestructor : MonoBehaviour
{
    public bool disable;
    public int range;
    // Start is called before the first frame update

    void Awake()
    {
        StartCoroutine("Timer");
    }

    IEnumerator Timer(){
        yield return new WaitForSecondsRealtime(range);
        if(disable){
            gameObject.SetActive(false);
        }
        else{
            Destroy(gameObject);
        }
    }
}
