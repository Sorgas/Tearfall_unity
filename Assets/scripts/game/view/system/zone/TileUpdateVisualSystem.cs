using game.model.component;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.zone {
    public class TileUpdateVisualSystem : IEcsRunSystem {
        public EcsFilter<TileVisualUpdateComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                TileVisualUpdateComponent component = filter.Get1(i);
                foreach (Vector3Int tile in filter.Get1(i).tiles) {
                    updateTile(tile);
                }
                component.tiles.Clear();
            }
        }

        private void updateTile(Vector3Int tile) {
            GameView.get().tileUpdater.updateTile(tile, false);
        }
    }
}