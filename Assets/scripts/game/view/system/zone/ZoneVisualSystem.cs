using game.model.component;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.zone {
    public class ZoneVisualSystem : IEcsRunSystem {
        public EcsFilter<ZoneVisualUpdateComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ZoneVisualUpdateComponent updateComponent = filter.Get1(i);
                updateTiles(entity, updateComponent);
            }
        }

        private void updateTiles(EcsEntity entity, ZoneVisualUpdateComponent updateComponent) {
            foreach (Vector3Int tile in updateComponent.tiles) {
                GameView.get().tileUpdater.updateTile(tile, false);
            }
            entity.Del<ZoneVisualUpdateComponent>();
        }
    }
}