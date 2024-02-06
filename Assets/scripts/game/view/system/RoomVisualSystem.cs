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
            Vector3 roomPosition = ViewUtil.fromModelToScene(room.positions.First());
            Debug.Log(roomPosition);
            parent.transform.localPosition = ViewUtil.fromModelToScene(room.positions.First());
            entity.Replace(new RoomVisualComponent{tiles = new(), parent = parent});
            foreach (var position in room.positions) {
                PrefabLoader.create("RoomTile", parent.transform, 
                    ViewUtil.fromModelToScene(position) - roomPosition);
            }
            Debug.Log("room visual created");
        }
    }

    private void createRoomVisualComponent(EcsEntity entity, RoomComponent room) {
        // List<GameObject> list = new();
        // GameObject parent = new GameObject();
        // parent.transform.parent = GameView.get().sceneElementsReferences.mapHolder;
        // Vector3 roomPosition = ViewUtil.fromModelToScene(room.positions.First());
        // Debug.Log(roomPosition);
        // parent.transform.localPosition = ViewUtil.fromModelToScene(room.positions.First());
        // entity.Replace(new RoomVisualComponent{tiles = new(), parent = parent, tileInstance = });
        // foreach (var position in room.positions) {
        //     PrefabLoader.create("RoomTile", parent.transform, 
        //         ViewUtil.fromModelToScene(position) - roomPosition);
        //     GameView.get().tileUpdater.setRoomTile(, position);
        // }
        //
        // Debug.Log("room visual created");
    }
}
}