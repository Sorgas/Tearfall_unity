using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.model.container {
// stores rooms
public class RoomContainer : LocalModelContainer {
    private readonly Dictionary<Vector3Int, EcsEntity> rooms = new();

    public RoomContainer(LocalModel model) : base(model) { }

    public void createRoom() { }

    // when building is created, it can create a room of corresponding type
    public void buildingCreated(EcsEntity building) {
        BuildingComponent buildingComponent = building.take<BuildingComponent>();
        if (rooms.ContainsKey(building.pos())) {
            // TODO 
        } else {
            if (RoomTypes.map.ContainsKey(buildingComponent.type.name)) {
                List<RoomType> types = RoomTypes.map[buildingComponent.type.name];
            }
        }
    }

    // when building destroyed, room can be deleted, or its type could change
    public void buildingDestroyed(EcsEntity building) { }

    // when passage areas become merged, rooms in these areas should be merged too.
    // expands one room, while shrinking others
    // given 'positions' considered interconnected
    public void roomsMerged(IEnumerable<Vector3Int> positions) {
        // // collect rooms on positions
        // HashSet<EcsEntity> set = positions
        //     .Where(rooms.ContainsKey)
        //     .Select(pos => rooms[pos]).ToHashSet();
        // if (set.Count == 0) return;
        //
        // EcsEntity room = set.First();
        // RoomComponent roomToExpand = room.take<RoomComponent>().building.take<RoomComponent>();
        // BuildingComponent building = roomToExpand.building.take<BuildingComponent>();
        //
        // Vector3Int startPosition = building.type.access != null 
        //     ? building.type.getAccessByPositionAndOrientation(roomToExpand.building.pos(), building.orientation)
        //     : roomToExpand.building.pos();
        // List<Vector3Int> newPositions = model.localMap.passageMap.roomHelper.floodFill(startPosition);
        // // set new positions as main room
        // foreach (var pos in newPositions) {
        //     addTileToRoom(pos, room);
        // }
    }

    // when passage area is split, room in this area should be reduced or split into two as well.
    public void roomsSplit(IEnumerable<Vector3Int> positions) { }

    // checks if room type is still valid 
    private void revalidateRoom(EcsEntity room) {
        HashSet<EcsEntity> buildings = room.take<RoomComponent>().positions
            .Select(pos => model.buildingContainer.getBuilding(pos))
            .Where(entity => !entity.IsNull())
            .ToHashSet();
        
    }
    
    private void addTileToRoom(Vector3Int tile, EcsEntity room) {
        removeTileFromRoom(tile);
        rooms[tile] = room;
        room.take<RoomComponent>().positions.Add(tile);
    }
    
    // removes a tile from its current room. Can delete whole room
    private void removeTileFromRoom(Vector3Int tile) {
        if (!rooms.ContainsKey(tile)) return;
        EcsEntity room = rooms[tile];
        RoomComponent roomComponent = room.take<RoomComponent>();
        roomComponent.positions.Remove(tile);
        rooms.Remove(tile);
        // if all tiles of room deleted, or tile with main building deleted
        if (roomComponent.positions.Count == 0 || roomComponent.building.take<BuildingComponent>().bounds.isIn(tile)) {
            removeRoom(room);
        }
    }
    
    private void removeRoom(EcsEntity room) {
        RoomComponent roomComponent = room.take<RoomComponent>();
        foreach (var oldRoomTile in roomComponent.positions) {
            rooms.Remove(oldRoomTile);
        }
        room.Destroy();
        // TODO notify owner, tasks, etc.
    }
}
}