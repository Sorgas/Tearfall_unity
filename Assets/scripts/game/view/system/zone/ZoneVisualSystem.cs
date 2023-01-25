using game.model.component;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.zone {
    public class ZoneVisualSystem : IEcsRunSystem {
        public EcsFilter<ZoneVisualUpdatedComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ZoneVisualUpdatedComponent updatedComponent = filter.Get1(i);
                updateTiles(entity, updatedComponent);
            }
        }

        private void updateTiles(EcsEntity entity, ZoneVisualUpdatedComponent updatedComponent) {
            foreach (Vector3Int tile in updatedComponent.tiles) {
                GameView.get().tileUpdater.updateTile(tile, false);
            }
            entity.Del<ZoneVisualUpdatedComponent>();
        }
    }
}