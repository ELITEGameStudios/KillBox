using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FloorColorScript : MonoBehaviour
{
    [SerializeField]
    private Tilemap[] floor_tilemaps, wall_tilemaps, special_tilemaps, special_wall_tilemaps;

    [SerializeField]
    private Tile floor_tile;

    private int theme;

    [SerializeField]
    private Color[] default_colors, themed;

    [SerializeField]
    private GameObject[] equipped_display;

    [SerializeField]
    private Color base_color;
    
    [SerializeField]
    private Gradient master_gradient;

    [SerializeField]
    private BoundsInt bounds;

    [SerializeField]
    private bool wall_colored;

    [SerializeField]
    private Color current_color;

    List<Tilemap> GetActiveTiles(Tilemap[] tiles_list){
        List<Tilemap> valid = new List<Tilemap>();

        foreach (Tilemap tilemap in tiles_list)
        {
            if(tilemap.gameObject.activeInHierarchy){
                valid.Add(tilemap);
            }
        }

        return valid;
    }

    public void ChangeColor(Color floor, Color wallCol, bool change_wall = false)
    {
        floor_tile.color = base_color;

        current_color = floor;

        MapData map = GameManager.main.GetCurrentMap();

        foreach (Vector3Int point in map.wallPositions){
            map.floorTiles.SetColor(point, floor);
        }

        if (change_wall)
        {
            foreach (Vector3Int point in map.wallPositions){
                map.wallTiles.SetColor(point, wallCol);
            }
        }

        //for (int i = 0; i < special_tilemaps.Length; i++)
        //{
        //    special_tilemaps[i].color = default_colors[Random.Range(0, default_colors.Length)];
        //    special_tilemaps[i].RefreshAllTiles();
        //}
    }

    // Main default function to keep set the maps color corresponding to the level
    public void SetColorFromGradient(float round, bool resetTilemaps = true)
    {
        float loops = round / 40;
        round -= 40 * (int)loops;
        MapData map = GameManager.main.GetCurrentMap();
        Debug.Log(loops.ToString() + "sdgdsLOOOOOOOOOOOOOOOOOOOOOOOOOOPS");
        
        if(resetTilemaps){
            map.wallTiles.color = Color.white;
            floor_tile.color = Color.white;
        }
        
        foreach (Vector3Int point in map.wallPositions){
            map.floorTiles.SetColor(point, base_color);
        }
        current_color = master_gradient.Evaluate(round / 40);

        foreach (Vector3Int point in map.floorPositions){
            map.floorTiles.SetColor(point, current_color);
        }
    }
    
    public Color GetColorFromGradient(float round)
    {

        float loops = round / 40;
        round -= 40 * (int)loops;
        return master_gradient.Evaluate(round / 40);

    }

    public void ChangeActiveColor(Color floor, Color wall, bool change_wall = false)
    {
        MapData map = GameManager.main.GetCurrentMap();
        floor_tile.color = base_color;

        current_color = floor;

        List<Tilemap> active_floor_tiles = GetActiveTiles(floor_tilemaps);

        foreach (Vector3Int point in map.floorPositions){
            map.floorTiles.SetColor(point, current_color);
        }

        if (change_wall)
        {
            
            List<Tilemap> active_wall_tiles = GetActiveTiles(wall_tilemaps);

            foreach (Vector3Int point in map.wallPositions){
                map.wallTiles.SetColor(point, current_color);
            }
        }

        //for (int i = 0; i < special_tilemaps.Length; i++)
        //{
        //    special_tilemaps[i].color = default_colors[Random.Range(0, default_colors.Length)];
        //    special_tilemaps[i].RefreshAllTiles();
        //}
    }

    public void ChangeWall(Color wall)
    {

        MapData map = GameManager.main.GetCurrentMap();
        List<Tilemap> active_wall_tiles = GetActiveTiles(wall_tilemaps);

        foreach (Vector3Int point in map.wallPositions){
            map.wallTiles.SetColor(point, wall);
        }
    }

    public Color CurrentColor
    {
        get { return current_color; }
    }
}
