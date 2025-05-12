using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridAnimationManager : MonoBehaviour
{
    
    public Color defaultWallColor, portalBrightWallCol; 
    public AnimationCurve brightCurve; // xMax will represent the range this effect spans
    public static GridAnimationManager instance {get; private set;}

    [SerializeField]float time;
    [SerializeField]float timer;
    [SerializeField]float normalizedTime;
    [SerializeField]float distance;
    [SerializeField]float range;
    [SerializeField] Tilemap floor;
    [SerializeField] Tilemap wall;


    public void Awake(){
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
    }

    public void DoEndRoundAnimation(){
        StartCoroutine(EndRoundCoroutine(PortalScript.main.transform.position, GameManager.main.GetCurrentMap(), portalBrightWallCol, defaultWallColor));
    } 

    public IEnumerator EndRoundCoroutine(Vector3 origin, MapData map, Color brightColor, Color normalColor, float speed = 10, float initDist = 1.5f){
        floor = map.floorTiles;
        wall = map.wallTiles;

        range = wall.cellBounds.size.magnitude;
        time = range / speed; 
        timer = 0;
        normalizedTime = timer/time;
        distance = initDist;

        Debug.Log("Begun some fancy animation or smth idk");

        while (timer < time){
            
            normalizedTime = timer/time;
            distance = Mathf.Lerp(0, range, normalizedTime); 
            foreach (Vector3Int point in map.wallPositions){

                float tileDist = Vector2.Distance(wall.CellToWorld(point), origin);
                if(tileDist < distance){
                    Color targetColor = Color.Lerp(
                        normalColor, brightColor, 
                        brightCurve.Evaluate(distance - tileDist)
                    );

                    wall.SetColor(point, targetColor);
                }
            }

            // wall.RefreshAllTiles();
            timer += Time.deltaTime;
            yield return null;
        }


        foreach (Vector3Int point in map.wallPositions){
            wall.SetColor(point, normalColor);
        }
    }
}
