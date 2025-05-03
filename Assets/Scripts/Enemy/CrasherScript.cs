using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Pathfinding;
using UnityEngine;

public class CrasherScript : MonoBehaviour
{
    [SerializeField] private bool crashing;
    [SerializeField] private float timeToCrash, timer, predictorCoefficient, spread, warningFlashes, bombRadius;
    [SerializeField] private int crasherType;
    [SerializeField] private Vector2 initialPosition, targetPosition;
    [SerializeField] private GameObject bombPrefab, indicator;
    [SerializeField] private EnemyHealth hp;
    [SerializeField] private Color warningColor;
    [SerializeField] private FixedRotator rotator;
    [SerializeField] private AIPath pathing;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartCrash", Random.Range(0, 6));
    }

    // Update is called once per frame
    void Update()
    {
        if(crashing){
            if(timer > 0){
    
                transform.position = Vector2.Lerp(targetPosition, initialPosition, Mathf.Sin(( Mathf.PI/2) * ( 1/timeToCrash * (timeToCrash - timer) + 1)));
                indicator.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.clear, warningColor, 0.5f * Mathf.Sin(2*Mathf.PI * (warningFlashes * (timeToCrash - timer) - 0.25f)) + 0.5f);
                rotator.speed = Mathf.Lerp(180, 540, 1 - (timer / timeToCrash));

                timer -= Time.deltaTime;
            }
            else{ hp.Die(false); }

        }
    }

    void StartCrash(){
        timer = timeToCrash;
        pathing.enabled = false;

        initialPosition = transform.position;
        targetPosition = Player.main.tf.position + (Vector3)Player.main.rb.velocity/Time.fixedDeltaTime * predictorCoefficient;
        targetPosition += new Vector2(Random.Range((float)-spread, (float)spread), Random.Range((float)-spread, (float)spread));
        
        indicator.SetActive(true);
        indicator.transform.position = targetPosition;
        indicator.transform.SetParent(null);
        
        crashing = true;
    }

    public void Explode (){
        crashing = false;
        GameObject bomb = Instantiate(bombPrefab, transform);
        bomb.transform.SetParent(null);
        bomb.transform.localScale = new Vector3(bombRadius, bombRadius, 1);
        bomb.GetComponent<CrasherBombScript>().ChangeType(crasherType, this);

    }

    public void DeleteIndicator(){
        Destroy(indicator);
    }
}
