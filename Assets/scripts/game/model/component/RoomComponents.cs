using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace game.model.component {

public struct RoomComponent {
    public string type;
    public EcsEntity building;
    public List<Vector3Int> positions;
}

public struct RoomVisualComponent {
    public GameObject parent;
    public Tile tileInstance;
    public List<GameObject> tiles;
}
}