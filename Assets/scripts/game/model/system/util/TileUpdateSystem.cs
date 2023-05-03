using System.Collections.Generic;
using game.model.component;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.util {
    // changing entities in model containers can update tile (see LocalModelUpdateContainer).
    // This system consumes tile updates and recreates updates for entities on updated tiles
    public class TileUpdateSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<TileUpdateComponent> filter;
        private bool debug = true;
        
        public override void Run() {
            foreach (int i in filter) {
                foreach (Vector3Int tile in filter.Get1(i).tiles) {
                    updateTile(tile);
                    log(tile.ToString());
                }
            }
        }
        
        private void updateTile(Vector3Int tile) {
            updateZone(model.zoneContainer.getZone(tile), tile);
            // TODO update other entities like this. e.g.: destroy plants and buildings on dig
        }

        private void updateZone(EcsEntity zone, Vector3Int tile) {
            if (zone == EcsEntity.Null) return;
            zone.Get<ZoneUpdateComponent>().add(tile);
        }

        private void log(string message) {
            if(debug) Debug.Log("[TileUpdateSystem] " + message);
        }
    }
}