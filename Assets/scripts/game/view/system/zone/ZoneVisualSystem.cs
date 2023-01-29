using game.model.component;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.zone {
    public class ZoneVisualSystem : IEcsRunSystem {
        public EcsFilter<ZoneUpdatedComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ZoneUpdatedComponent updatedComponent = filter.Get1(i);
                updateTiles(entity, updatedComponent);
            }
        }

        private void updateTiles(EcsEntity entity, ZoneUpdatedComponent updatedComponent) {
            foreach (Vector3Int tile in updatedComponent.tiles) {
                GameView.get().tileUpdater.updateTile(tile, false);
            }
            entity.Del<ZoneUpdatedComponent>();
        }
    }
}