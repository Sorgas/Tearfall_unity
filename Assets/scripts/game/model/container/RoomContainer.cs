using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang;
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
        // collect rooms on positions
        HashSet<EcsEntity> set = positions
            .Where(rooms.ContainsKey)
            .Select(pos => rooms[pos]).ToHashSet();
        if (set.Count == 0) return;
        EcsEntity room = set.First();
        RoomComponent roomComponent = room.take<RoomComponent>().building.take<RoomComponent>();
        BuildingComponent buildingComponent = roomComponent.building.take<BuildingComponent>();
        Vector3Int startPosition = buildingComponent.type.access != null 
            ? buildingComponent.type.getAccessByPositionAndOrientation(roomComponent.building.pos(), buildingComponent.orientation)
            : roomComponent.building.pos();
        List<Vector3Int> newPositions = model.localMap.passageMap.roomHelper.floodFill(startPosition);
        // set new positions as main room
        foreach (var pos in newPositions) {
            addTileToRoom(pos, room);
        }
    }

    // when passage area is split, room in this area should be reduced or split into two as well.
    public void roomsSplit(List<Vector3Int> positions) {
        log($"splitting rooms in {positions.Select(pos => pos.ToString()).Aggregate((s1, s2) => s1 + s2)}" );
        // collect rooms on positions
        HashSet<EcsEntity> set = positions
            .Where(rooms.ContainsKey)
            .Select(pos => rooms[pos]).ToHashSet();
        if (set.Count == 0) return;
        // refill rooms
        foreach (var room in set) {
            RoomComponent roomComponent = room.take<RoomComponent>().building.take<RoomComponent>();
            BuildingComponent buildingComponent = roomComponent.building.take<BuildingComponent>();
            Vector3Int startPosition = buildingComponent.type.access != null
                ? buildingComponent.type.getAccessByPositionAndOrientation(roomComponent.building.pos(), buildingComponent.orientation)
                : roomComponent.building.pos();
            List<Vector3Int> newPositions = model.localMap.passageMap.roomHelper.floodFill(startPosition);
            foreach (var position in roomComponent.positions) { // remove all not accessible from building
                if (!newPositions.Contains(position)) {
                    removeTileFromRoom(position);
                }
            }
            // set new positions as main room
            foreach (var pos in newPositions) {
                addTileToRoom(pos, room);
            }
        }
        List<Vector3Int> positionsWithoutRooms = positions
            .Where(pos => !rooms.ContainsKey(pos))
            .ToList();
        foreach (var positionsWithoutRoom in positionsWithoutRooms) {
            // TODO createRoom
        }
    }

    // creates new room flood-filling from given position and sets its type.
    public void createRoomFromPosition(Vector3Int position) {
        List<Vector3Int> newPositions = model.localMap.passageMap.roomHelper.floodFill(position);
        HashSet<EcsEntity> buildings = newPositions
            .Select(pos => model.buildingContainer.getBuilding(pos))
            .Where(building => !building.IsNull())
            .ToHashSet();
        MultiValueDictionary<string, EcsEntity> typesOfBuildings = new();
        foreach (var building in buildings) {
            typesOfBuildings.add(building.take<BuildingComponent>().type.name, building);
        }
        RoomType roomType = selectRoomTypeByBuildings(typesOfBuildings);
        if (roomType != null) {
            EcsEntity room = model.createEntity();
            room.Replace(new RoomComponent { building = typesOfBuildings[roomType.buildingType][0], positions = newPositions, type = roomType.name });
            foreach (var pos in newPositions) {
                addTileToRoom(pos, room);
            }
        }
    }
    
    // checks if room type is still valid 
    private void revalidateRoom(EcsEntity room) {
        HashSet<EcsEntity> buildings = room.take<RoomComponent>().positions
            .Select(pos => model.buildingContainer.getBuilding(pos))
            .Where(entity => !entity.IsNull())
            .ToHashSet();
    }
    
    private void addTileToRoom(Vector3Int tile, EcsEntity room) {
        if (rooms.ContainsKey(tile)) {
            if (rooms[tile] == room) return; // already in required room
            removeTileFromRoom(tile); // remove from current room
        }
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

    // TODO add other room types
    private RoomType selectRoomTypeByBuildings(MultiValueDictionary<string, EcsEntity> buildings) {
        if (buildings.ContainsKey("bed")) {
            if (buildings["bed"].Count > 1) {
                return RoomTypes.dormitory;
            } else {
                return RoomTypes.bedroom;
            }
        }
        return null;
    }
    
    private void log(string message) {
        Debug.Log($"[RoomContainer] {message}");
    }
}
}