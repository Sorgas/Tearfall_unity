using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.container {
// stores rooms
public class RoomContainer : LocalModelContainer {
    private readonly Dictionary<Vector3Int, EcsEntity> rooms = new();
    
    public RoomContainer(LocalModel model) : base(model) { }

    public void createRoom() {
        
    }
}
}