using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.view.tilemaps;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using static game.view.util.TilemapLayersConstants;

namespace game.view.system {
public class RoomVisualSystem : IEcsRunSystem {
    public EcsFilter<RoomComponent>.Exclude<RoomVisualComponent> filter;
        // NESW
    private string[] spriteNames = new[] {
        "none", "W", "S", "SW", "E", "EW", "ES", "ESW", "N", "NW", "NS", "NSW", "NE", "NEW", "NES", "NESW"
    };
    private Vector3 spriteOffset;

    public void Run() {
        spriteOffset = new(0, 0, WALL_LAYER * GRID_STEP);
        foreach (var i in filter) {
            RoomComponent room = filter.Get1(i);
            EcsEntity entity = filter.GetEntity(i);
            List<GameObject> list = new();
            GameObject parent = new GameObject();
            parent.transform.parent = GameView.get().sceneElementsReferences.mapHolder;
            Vector3 roomPosition = ViewUtil.fromModelToScene(room.positions.First());
            Debug.Log(roomPosition);
            parent.transform.localPosition = ViewUtil.fromModelToScene(room.positions.First()) + spriteOffset;
            entity.Replace(new RoomVisualComponent { tiles = new(), parent = parent });
            foreach (var position in room.positions) {
                GameObject tile = PrefabLoader.create("RoomTile", parent.transform,
                    ViewUtil.fromModelToScene(position) - roomPosition);
                Sprite sprite = RoomTilesetHolder.get().getSprite(getTileName(position, room.positions));
                tile.GetComponent<SpriteRenderer>().sprite = sprite;
            }
            Debug.Log("room visual created");
        }
    }

    private string getTileName(Vector3Int position, List<Vector3Int> positions) {
        List<Vector3Int> offsets = positions.Select(pos => pos - position)
            .Where(offset => offset.x < 2 && offset.x > -2 && offset.y < 2 && offset.y > -2) // position near current
            .Where(offset => (offset.x == 0 || offset.y == 0) && offset.x != offset.y) // position orthogonal
            .ToList();
        int value = 15; // 4 bits: NESW
        foreach (var offset in offsets) {
            if (offset.x == 1) { 
                value &= 0b1011; // E
            } else if(offset.x == -1) {
                value &= 0b1110; // W
            } else if (offset.y == 1) {
                value &= 0b0111; // N
            } else if (offset.y == -1) {
                value &= 0b1101; // S
            }
        }
        return "roomTiles_" + spriteNames[value];
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