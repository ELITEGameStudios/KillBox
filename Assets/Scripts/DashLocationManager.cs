using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashLocationManager : MonoBehaviour
{

    [SerializeField] private bool cutterMapScript;
    [SerializeField] private Transform[] positions;

    public static DashLocationManager cutterMap {get; private set;}

    // Start is called before the first frame update
    void Awake() {
        if(cutterMap == null && cutterMapScript){
            cutterMap = this;
        }
        else if(cutterMap != this && cutterMapScript){
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update(){}

    public Transform getClosestValidPosition(bool lineOfSight, float minDistance){
        List<Transform> filteredPositions = new List<Transform>();

        
        foreach (Transform position in positions)
        {
            float angle = Vector2.SignedAngle(position.position, Player.main.tf.position) * Mathf.Deg2Rad;
            float distance = Vector2.Distance(position.position, Player.main.tf.position);

            if(lineOfSight){
                RaycastHit2D hit = Physics2D.Raycast(position.position, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)), distance);
                
                if ( (hit.collider.gameObject != Player.main.obj && hit.collider.gameObject != null)){
                    continue;
                }
            }

            if ( distance < minDistance){
                // filteredPositions.Remove(position);
                Debug.Log("Distance removed: " + distance);
            }
            else{ 
                filteredPositions.Add(position); 
                Debug.Log("Distance Added "+ distance); }
        }

        float smallestDist = Vector2.Distance(filteredPositions[0].position, Player.main.tf.position);
        Transform targetTf = filteredPositions[0];
        
        foreach (Transform tf in filteredPositions)
        {
            float distance = Vector2.Distance(tf.position, Player.main.tf.position);
            if(distance < smallestDist){
                smallestDist = distance;
                targetTf = tf;
            }
        }

        Debug.Log("Point chosen: " + targetTf.position.x + " , " + targetTf.position.y );
        return targetTf;
    }
}
