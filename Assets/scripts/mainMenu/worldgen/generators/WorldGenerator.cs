using System.Collections;
using System.Collections.Generic;
using mainMenu.WorldGen;
using UnityEngine;
using UnityEngine.Tilemaps;

// Gets generation parameters from ui, launches generation, renders world to tilemap
public abstract class WorldGenerator
{
    public Tilemap tilemap;
    public TileBase tile;

    public abstract void generate(WorldGenConfig config, WorldGenContainer container);
}
