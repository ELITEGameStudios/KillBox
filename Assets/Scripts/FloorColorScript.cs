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

    public void ChangeColor(Color floor, Color wall, bool change_wall = false)
    {
        floor_tile.color = base_color;

        current_color = floor;

        for (int i = 0; i < floor_tilemaps.Length; i++)
        {
            floor_tilemaps[i].color = current_color;
            floor_tilemaps[i].RefreshAllTiles();
        }

        if (change_wall)
        {

            for (int i = 0; i < wall_tilemaps.Length; i++)
            {
                wall_tilemaps[i].color = current_color;
                wall_tilemaps[i].RefreshAllTiles();
            }
        }

        //for (int i = 0; i < special_tilemaps.Length; i++)
        //{
        //    special_tilemaps[i].color = default_colors[Random.Range(0, default_colors.Length)];
        //    special_tilemaps[i].RefreshAllTiles();
        //}
    }

    // Main default function to keep set the maps color corresponding to the level
    public void SetColorFromGradient(float round)
    {
        float loops = round / 40;
        round -= 40 * (int)loops;
        Debug.Log(loops.ToString() + "sdgdsLOOOOOOOOOOOOOOOOOOOOOOOOOOPS");

        floor_tile.color = base_color;

        current_color = master_gradient.Evaluate(round / 40);

        for (int i = 0; i < floor_tilemaps.Length; i++)
        {
            floor_tilemaps[i].color = current_color;
            floor_tilemaps[i].RefreshAllTiles();
        }
    }
    public void ChangeActiveColor(Color floor, Color wall, bool change_wall = false)
    {
        floor_tile.color = base_color;

        current_color = floor;

        List<Tilemap> active_floor_tiles = GetActiveTiles(floor_tilemaps);

        for (int i = 0; i < active_floor_tiles.Count; i++)
        {
            active_floor_tiles[i].color = current_color;
            active_floor_tiles[i].RefreshAllTiles();
        }

        if (change_wall)
        {
            
            List<Tilemap> active_wall_tiles = GetActiveTiles(wall_tilemaps);

            for (int i = 0; i < active_wall_tiles.Count; i++)
            {
                active_wall_tiles[i].color = current_color;
                active_wall_tiles[i].RefreshAllTiles();
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

        List<Tilemap> active_wall_tiles = GetActiveTiles(wall_tilemaps);

        for (int i = 0; i < active_wall_tiles.Count; i++)
        {
            active_wall_tiles[i].color = wall;
            active_wall_tiles[i].RefreshAllTiles();
        }
    }

    public Color CurrentColor
    {
        get { return current_color; }
    }
}
