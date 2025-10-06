using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class beamMaskGenerator : MonoBehaviour
{
    Collider2D collidedWall;
    TilemapCollider2D collidedWallTile;
    Vector2 beamScale;
    Vector2 beamDirection {get { return beamRoot.up; }}
    Transform leftBeamRoot, rightBeamRoot;
    Transform beamRoot {get { return transform; }}
    Vector2 leftmostPoint, rightmostPoint;
    List<Vector2> normals;
    bool needsTriangle
    {
        get
        {
            return false; // test if the beam direction is normal to the vector between the leftmost and rightmost point.
        }
    }

    void Update()
    {
        leftBeamRoot.localPosition = Vector2.left * beamScale.x / 2;
        rightBeamRoot.localPosition = Vector2.right * beamScale.x / 2;
    }


    void CalculatePoints(Collider2D collider)
    {
        // Get the closest point to both the beginning and end of the beam - A1A2A3 A4A52A6 -> A1 A2
        /* method 1
                Vector2[] closestPointsToStart = new Vector2[]{
                    collider.ClosestPoint(leftBeamRoot.position),
                    collider.ClosestPoint(beamRoot.position),
                    collider.ClosestPoint(rightBeamRoot.position)
                };

                // float[] distancesClosestPointsToStart = new float[]{
                //     (closestPointsToStart[0] - (Vector2)leftBeamRoot.position).magnitude,
                // };

                Vector2[] closestPointsToEnd = new Vector2[]{
                    collider.ClosestPoint(leftBeamRoot.position + beamRoot.up * beamScale.y),
                    collider.ClosestPoint(beamRoot.position + beamRoot.up * beamScale.y),
                    collider.ClosestPoint(rightBeamRoot.position + beamRoot.up * beamScale.y)
                };
        method 2 */

        Vector2[] closestPoints = new Vector2[]{
            collider.ClosestPoint(leftBeamRoot.position),
            collider.ClosestPoint(beamRoot.position),
            collider.ClosestPoint(rightBeamRoot.position),
            collider.ClosestPoint(leftBeamRoot.position + beamRoot.up * beamScale.y),
            collider.ClosestPoint(beamRoot.position + beamRoot.up * beamScale.y),
            collider.ClosestPoint(rightBeamRoot.position + beamRoot.up * beamScale.y)
        };

        // Get the midpoint of those two points - B
        Vector2 averagePoint = Vector2.zero;
        foreach (Vector2 point in closestPoints)
        {
            averagePoint += point;
        }
        averagePoint /= closestPoints.Length;

        // Calculate two points on each edge of the beam, perpendicular to the beam direction and containing the newly calculated point B. - C1 C2
        Vector2 leftmostToPoint = averagePoint - (Vector2)leftBeamRoot.position;
        float projection = Vector2.Dot((Vector2)leftBeamRoot.position, beamDirection);
        Vector2 projectionVector = (Vector2)leftBeamRoot.position + beamDirection;
        Vector2 rejection = leftmostToPoint - projectionVector;
        
        float rejectionLength = rejection.magnitude;
        float rightRejectionLength = beamScale.x - rejection.magnitude;
        Vector2 rejectionRight = (Vector2)rightBeamRoot.position + projectionVector.normalized * rightRejectionLength;

        Vector2 leftBeamPoint = averagePoint - rejection;
        Vector2 rightBeamPoint = averagePoint - rejectionRight;


        // the closest points from the collider to C1 and C2 will be your leftmost and rightmost points.
        leftmostPoint = collider.ClosestPoint(leftBeamPoint);
        rightmostPoint = collider.ClosestPoint(rightBeamPoint);
    }

    void CalculateNormals(Vector2 point1, Vector2 point2)
    {
        // Vector2 projA = point1 - beamRoot.position?
        // Vector2 projB = point1 - beamRoot.position?
        // For point one and two respectively, create another point for each in the direction of the beam, at the end of the beam.
        // with the set of now 4 points, create normal vectors for each edge, connecting point1 and point2, point1 and point1a, point2 and point2a, point 1a and point 2a

        // pass this into the normals vector2 list
    }

    void FeedShader()
    {
        Texture2D normalData = new Texture2D(normals.Count, 0 /* Will instead be the amount of separate shapes being computed.*/);
        // Give the shader the normal list(s) of shape(s)
        // the shader will test if each normal is facing towards or away from each pixel point and determine alpha based on this.

        // the shader will be limited to computing only one shape, but this may change if i represent each normal vector in color textures to use Texture2DArray, or script the shader altogether

        // This shader in question might genuinely need to be scripted if it wants functionality beyond just one shape
    }

}
