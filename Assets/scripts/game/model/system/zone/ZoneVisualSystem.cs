using System.Collections.Generic;
using game.model.component;
using game.view;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.system.zone {
    public class ZoneVisualSystem : IEcsRunSystem {
        public EcsFilter<ZoneVisualUpdatedComponent> filter;
        
        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
            }
        }

        // private void create(EcsEntity entity, ZoneComponent zone) {
        //     List<Vector3Int> visualTiles = new();
        //     foreach (Vector3Int position in zone.tiles) {
        //         GameView.get().tileUpdater.updateTile(position, false);
        //     }
        //     entity.Replace(new ZoneVisualComponent { tiles =  });
        // }
        //
        // private void createTiles(ZoneComponent zone, ZoneVisualUpdatedComponent updatedComponent) {
        //     List<Vector3Int> toAdd = zone.tiles.
        // }
    }
}