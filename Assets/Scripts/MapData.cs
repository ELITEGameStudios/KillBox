using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapData : MonoBehaviour
{
    [SerializeField]
    private int index, debutRound, retireRound;

    [SerializeField]
    private GameObject root;

    [SerializeField]
    private Collider2D obstacles;

    [SerializeField]
    private Transform player_start, portal_start, weapons_start, upgrades_start, starter_start;

    [SerializeField]
    private Spawn2[] spawners;

    [Header("Tile data")]
    public Tilemap floorTiles;
    public Tilemap wallTiles;
    public Vector3Int[] wallPositions, floorPositions;

    public void Update(){
        
    }


    public void SetTileData(){
        BoundsInt wallBounds = wallTiles.cellBounds;
        BoundsInt floorBounds = floorTiles.cellBounds;

        List<Vector3Int> wallPositions = new();
        List<Vector3Int> floorPositions = new();
        
        foreach (Vector3Int point in wallBounds.allPositionsWithin){
            if(wallTiles.HasTile(point)){wallPositions.Add(point);}
        }

        foreach (Vector3Int point in floorBounds.allPositionsWithin){
            if(floorTiles.HasTile(point)){floorPositions.Add(point);}
        }

        this.wallPositions = wallPositions.ToArray();
        this.floorPositions = floorPositions.ToArray();

        AssignDefaultWallColors();
    }

    public void AssignDefaultWallColors(){
        foreach (Vector3Int point in wallPositions){
            wallTiles.SetColor(point, GridAnimationManager.instance.defaultWallColor);
        }
    }

    // Start is called before the first frame update
    void Awake(){
        root = gameObject;
    }

    public GameObject Root{get {return root;}}
    public Collider2D Obstacles{get {return obstacles;}}
    public Transform Player{get {return player_start;}}
    public Transform Portal{get {return portal_start;}}
    public Transform Weapons{get {return weapons_start;}}
    public Transform Upgrades{get {return upgrades_start;}}
    public Transform Starter{get {return starter_start;}}
    public int Index {get {return index;}}
    public int DebutRound {get {return debutRound;}}
    public int RetireRound {get {return retireRound;}}
    public Spawn2[] GetSpawners(){return spawners;}
    public Spawn2[] GetSpawners(int round){
        List<Spawn2> spawnlist = new();
        foreach (Spawn2 spawner in spawners){
            if(spawner.unlock_on_level <= round){
                spawnlist.Add(spawner);
            }
        }
        return spawnlist.ToArray();
    }
}
