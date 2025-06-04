using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridAnimationManager : MonoBehaviour
{
    
    public Color defaultWallColor, portalBrightWallCol, worldGenBlue; 
    public AnimationCurve brightCurve, floorFadeinCurve; // xMax will represent the range this effect spans
    public static GridAnimationManager instance {get; private set;}

    [SerializeField]float time;
    [SerializeField]float timer;
    [SerializeField]float normalizedTime;
    [SerializeField]float distance;
    [SerializeField]float range;
    [SerializeField] Tilemap floor;
    [SerializeField] Tilemap wall;
    [SerializeField] FloorColorScript floorColorScript;


    public void Awake(){
        if(instance == null){instance = this;}
        else if(instance != this){Destroy(this);}
    }

    public void DoEndRoundAnimation(){
        StartCoroutine(EndRoundCoroutine(PortalScript.main.transform.position, GameManager.main.GetCurrentMap(), portalBrightWallCol, defaultWallColor));
    } 

    public void DoIntroRoundAnimation(){
        // StartCoroutine(MapIntroRandomizedCoroutine(GameManager.main.GetCurrentMap(), worldGenBlue, defaultWallColor));
        StartCoroutine(MapIntroLinearCoroutine(GameManager.main.GetCurrentMap(), worldGenBlue, defaultWallColor));
    } 

    public IEnumerator EndRoundCoroutine(Vector3 origin, MapData map, Color brightColor, Color normalColor, float speed = 12.5f, float initDist = 1.5f){
        floor = map.floorTiles;
        wall = map.wallTiles;

        range = wall.cellBounds.size.magnitude;
        time = range / speed; 
        timer = 0;
        normalizedTime = timer/time;
        distance = initDist;


        Debug.Log("Begun some fancy animation or smth idk");

        List<Color> originalFloorColors = new();
        int i = 0;
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
            foreach (Vector3Int point in map.floorPositions){
                
                float tileDist = Vector2.Distance(wall.CellToWorld(point), origin);
                Color currentfloorColor;
                if(originalFloorColors.Count < map.floorPositions.Length){
                    currentfloorColor = map.floorTiles.GetColor(point);
                    originalFloorColors.Add(currentfloorColor);
                }
                else{
                    try { currentfloorColor = originalFloorColors[i]; }
                    catch { currentfloorColor = originalFloorColors[0]; };

                }

                if(tileDist < distance){
                    
                    Color floorBrightColor = Color.Lerp(currentfloorColor, brightColor, 0.7f);
                    
                    Color targetColor = Color.Lerp(
                        currentfloorColor, floorBrightColor,
                        brightCurve.Evaluate(distance - tileDist)
                    );

                    floor.SetColor(point, targetColor);
                }
            }

            // wall.RefreshAllTiles();
            i++;
            timer += Time.deltaTime;
            yield return null;
        }


        foreach (Vector3Int point in map.wallPositions){
            wall.SetColor(point, normalColor);
        }
    }

    public IEnumerator MapIntroLinearCoroutine(MapData map, Color brightColor, Color normalColor, float speed = 20){
        floor = map.floorTiles;
        wall = map.wallTiles;

        float modifiedOffset = 5;
        range = wall.cellBounds.size.y + modifiedOffset;
        time = range / speed; 
        timer = 0;
        normalizedTime = timer/time;
        distance = wall.cellBounds.min.y;
        Color endFloorColor = floorColorScript.GetColorFromGradient(KillBox.currentGame.round);

        foreach (Vector3Int point in map.wallPositions){
            wall.SetColor(point, Color.clear);
        }
        foreach (Vector3Int point in map.floorPositions){
            floor.SetColor(point, Color.clear);
        }

        Debug.Log("Begun some fancy animation or smth idk");


        while (timer < time){
            
            float modifiedDistance = distance-modifiedOffset;
            normalizedTime = timer/time;
            distance = Mathf.Lerp(wall.cellBounds.min.y, range, normalizedTime); 

            foreach (Vector3Int point in map.wallPositions){

                if(point.y < distance){
                    Color targetColor = Color.Lerp(
                        normalColor, brightColor, 
                        brightCurve.Evaluate(distance - point.y)
                    );

                    wall.SetColor(point, targetColor);
                }
            }
            foreach (Vector3Int point in map.floorPositions){
                
                if(point.y < distance){
                    
                    Color targetColor = Color.Lerp(
                        Color.clear, endFloorColor,
                        floorFadeinCurve.Evaluate(modifiedDistance - point.y)
                    );

                    floor.SetColor(point, targetColor);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        foreach (Vector3Int point in map.wallPositions){
            wall.SetColor(point, normalColor);
        }
        foreach (Vector3Int point in map.floorPositions){
            floor.SetColor(point, endFloorColor);
        }
        CameraBgManager.instance.SetBackground(CameraBgManager.instance.defaultBg, 1);

    }


    public IEnumerator MapIntroRandomizedCoroutine(MapData map, Color brightColor, Color normalColor, float speed = 20){
        floor = map.floorTiles;
        wall = map.wallTiles;
        List<Vector3Int> wallPositions = CommonFunctions.RandomizeList(map.wallPositions.ToList()); 
        List<Vector3Int> floorPositions = CommonFunctions.RandomizeList(map.floorPositions.ToList()); 
        Debug.Log("WALLPOSITIONS: " + wallPositions.Count + " COMPARED TO "+ map.wallPositions.Length);

        float modifiedOffset = 10;
        int step = 20;
        range = 
            wallPositions.Count > floorPositions.Count 
            ? (wallPositions.Count + modifiedOffset) / step 
            : (floorPositions.Count + modifiedOffset) / step;

        time = range / speed; 
        timer = 0;
        normalizedTime = timer/time;
        distance = 0;
        Color endFloorColor = floorColorScript.GetColorFromGradient(KillBox.currentGame.round);

        foreach (Vector3Int point in map.wallPositions){
            wall.SetColor(point, Color.clear);
        }
        foreach (Vector3Int point in map.floorPositions){
            floor.SetColor(point, Color.clear);
        }

        Debug.Log("Begun some fancy animation or smth idk");


        while (timer < time){
            
            normalizedTime = timer/time;
            distance = Mathf.Lerp(0, range, normalizedTime); 
            
            for (int i = 0; i < wallPositions.Count; i++){
                Vector3Int point = wallPositions[i];

                wall.SetColor(point, Color.clear);
                if(point.y < distance){
                    Color targetColor = Color.Lerp(
                        normalColor, brightColor, 
                        brightCurve.Evaluate(distance - (int)(i / step))
                    );

                    wall.SetColor(point, targetColor);
                }
            }
                
            for (int i = 0; i < floorPositions.Count; i++){
                Vector3Int point = floorPositions[i];
                floor.SetColor(point, Color.clear);
                if(point.y < distance){
                    
                    Color targetColor = Color.Lerp(
                        endFloorColor, brightColor,
                        brightCurve.Evaluate(distance -(int)(i / step))
                    );

                    floor.SetColor(point, targetColor);
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        foreach (Vector3Int point in map.wallPositions){
            wall.SetColor(point, normalColor);
        }
        foreach (Vector3Int point in map.floorPositions){
            floor.SetColor(point, endFloorColor);
        }
        CameraBgManager.instance.SetBackground(CameraBgManager.instance.defaultBg, 1);

    }


}
