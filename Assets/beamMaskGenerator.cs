using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class beamMaskGenerator : MonoBehaviour
{
    Collider2D collidedWall;
    TilemapCollider2D collidedWallTile;
    Vector2 beamDirection;
    Vector2 leftmostPoint, rightmostPoint;
    List<Vector2> normals;
    bool needsTriangle
    {
        get
        {
            return false; // test if the beam direction is normal to the vector between the leftmost and rightmost point.
        }
    }

    void CalculatePoints(Collider2D collider)
    {
        // Get the closest point to both the beginning and end of the beam - A1 A2
        // Get the midpoint of those two points - B

        // Calculate two points on each edge of the beam, perpendicular to the beam direction and containing the newly calculated point B. - C1 C2
        // the closest points from the collider to C1 and C2 will be your leftmost and rightmost points.
    }

    void CalculateNormals(Vector2 point1, Vector2 point2)
    {
        // For point one and two respectively, create another point for each in the direction of the beam, at the end of the beam.
        // with the set of now 4 points, create normal vectors for each edge, connecting point1 and point2, point1 and point1a, point2 and point2a, point 1a and point 2a

        // pass this into the normals vector2 list
    }

    void FeedShader()
    {
        // Give the shader the normal list(s) of shape(s)
        // the shader will test if each normal is facing towards or away from each pixel point and determine alpha based on this.

        // the shader will be limited to computing only one shape, but this may change if i represent each normal vector in color textures to use Texture2DArray, or script the shader altogether

        // This shader in question might genuinely need to be scripted if it wants functionality beyond just one shape
    }

}
