using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component {

public struct RoomComponent {
    public string type;
    public EcsEntity building;
    public List<Vector3Int> positions;
}
}