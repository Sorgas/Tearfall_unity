using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system {
public class RoomVisualSystem : IEcsRunSystem {
    public EcsFilter<RoomComponent>.Exclude<RoomVisualComponent> filter;
    
    public void Run() {
        foreach (var i in filter) {
            RoomComponent room = filter.Get1(i);
            EcsEntity entity = filter.GetEntity(i);
            List<GameObject> list = new();
            GameObject parent = new GameObject();
            parent.transform.parent = GameView.get().sceneElementsReferences.mapHolder;
            parent.transform.localPosition = ViewUtil.fromModelToScene(room.positions.First());
            entity.Replace(new RoomVisualComponent{tiles = new(), parent = parent});
            foreach (var position in room.positions) {
                PrefabLoader.create("RoomTile", parent.transform, 
                    parent.transform.InverseTransformPoint(ViewUtil.fromModelToScene(position)));
            }
        }
    }
}
}