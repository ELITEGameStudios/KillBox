using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowFieldAnimationScript : MonoBehaviour
{
    [SerializeField] private float elapsed;
    [SerializeField] private int warningFlashes, normalFlashes;
    [SerializeField] private Color warningColor, normalColor;
    [SerializeField] private SpriteRenderer renderer;
    [SerializeField] private bool isInContact;

    // Start is called before the first frame update
    void Start()
    { 
        renderer =  GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInContact){
            renderer.color = Color.Lerp(Color.clear, warningColor, 0.5f * CommonFunctions.WarningIndicator(elapsed, warningFlashes) + 0.5f);
            elapsed += Time.deltaTime;
        }
        else{
            renderer.color = Color.Lerp(Color.clear, normalColor,  0.5f * CommonFunctions.WarningIndicator(elapsed, normalFlashes) + 0.5f);
            elapsed += Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject == Player.main.obj){
            isInContact = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject == Player.main.obj){
            isInContact = false;
        }
    }
}
