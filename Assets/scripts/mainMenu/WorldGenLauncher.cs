using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

// Gets generation parameters from ui, launches generation, renders world to tilemap
public class WorldGenLauncher
{
    public Tilemap tilemap;
    public TileBase tile;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("gen start");

        Vector3Int vector = new Vector3Int(0, 0, 0);
        for (int i = 0; i < 300; i++) {
            vector.Set(i, i, 0);
            tilemap.SetTile(vector, tile);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
