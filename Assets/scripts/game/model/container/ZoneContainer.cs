using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry.bounds;

namespace game.model.container {
    public class ZoneContainer : LocalMapModelComponent {
        public Dictionary<Vector3Int, EcsEntity> zones = new();
        
        public ZoneContainer(LocalModel model) : base(model) {
            
        }

        public void createZone(IntBounds3 bounds, string type) {
            
        }

        public EcsEntity getZone(Vector3Int position) {
            return zones.ContainsKey(position) ? zones[position] : EcsEntity.Null;
        }
    }
}