using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrasherBombScript : MonoBehaviour
{
    public int type{get; private set;}
    [SerializeField] private float deleteTime, timer;
    [SerializeField] private Collider2D collider;
    [SerializeField] private SpriteRenderer graphic;
    [SerializeField] private Vector3 initialScale;
    [SerializeField] private Color targetColor;

    public void ChangeType(int newType, CrasherScript instance){
        type = newType;
    }

    void Awake(){
        timer = 0;
        initialScale = graphic.transform.localScale;
        graphic.transform.localScale = Vector3.zero;
    }
    
    void Update(){
        graphic.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, timer / deleteTime);
        graphic.color = Color.Lerp(Color.clear, targetColor, Mathf.Sin( Mathf.PI/2 * ( 1/deleteTime * (timer) + 1)));
        
        if(timer < deleteTime){
            if (timer >= 0.15f){ DisableCollision(); }
            timer += Time.deltaTime;
        }
        else{ DeleteSelf(); }
    }   

    void DisableCollision(){
        collider.enabled = false;

    }

    void DeleteSelf(){
        Destroy(gameObject);
    }

}
