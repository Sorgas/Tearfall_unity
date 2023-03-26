using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system {
    public class TileUpdatingSystem : LocalModelEcsSystem {
        public EcsFilter<TileUpdateComponent> filter;

        public TileUpdatingSystem(LocalModel model) : base(model) { }
        
        public override void Run() {
            foreach (int i in filter) {
                foreach (Vector3Int tile in filter.Get1(i).tiles) {
                    updateTile(tile);
                }
            }
        }
        
        private void updateTile(Vector3Int tile) {
            updateZone(model.zoneContainer.getZone(tile), tile);
            // TODO update other entities like this. e.g.: destroy plants and buildings on dig
        }

        private void updateZone(EcsEntity zone, Vector3Int tile) {
            if (zone == EcsEntity.Null) return;
            ref ZoneUpdatedComponent updatedComponent = ref zone.Get<ZoneUpdatedComponent>();
            if (updatedComponent.tiles == null) updatedComponent.tiles = new List<Vector3Int>();
            updatedComponent.tiles.Add(tile);
        }
    }
}