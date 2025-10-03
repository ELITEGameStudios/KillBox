using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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
    public static Vector3 getValidPosition(bool lineOfSight, float minDistance)
    {

        if (cutterMap != null) { return cutterMap.getClosestValidPosition(lineOfSight, minDistance); } // Default
        else
        {
            List<Vector2> positions = new List<Vector2>();
            // Fallback
            for (int i = 0; i < 16; i++)
            {
                float angle = 360 / 16 * i;
                Vector2 unitCircleVector = new Vector2(
                    Mathf.Cos(angle * Mathf.Deg2Rad),
                    Mathf.Sin(angle * Mathf.Deg2Rad)
                );
                Vector2 targetPos = (Vector2)Player.main.tf.position + unitCircleVector * minDistance;
                RaycastHit2D hit = Physics2D.Raycast(targetPos, unitCircleVector * -1, minDistance);

                if ((hit.collider.gameObject != Player.main.obj && hit.collider.gameObject != null)) { continue; }
                positions.Add(targetPos);
            }

            if (positions.Count > 0)
            {
                Vector2 winningPos = positions[0];
                float winningDifference = Vector2.Angle(winningPos.normalized, Player.main.rb.velocity.normalized);
                foreach (Vector2 position in positions)
                {
                    float diff = Vector2.Angle(position.normalized, Player.main.rb.velocity.normalized);
                    if (diff < winningDifference){ winningPos = position; }
                }   

                return winningPos;
            }
            else
            {
                return Player.main.tf.position;
            }
        }
    }

    public Vector3 getClosestValidPosition(bool lineOfSight, float minDistance){
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
        return targetTf.position;
    }
}
