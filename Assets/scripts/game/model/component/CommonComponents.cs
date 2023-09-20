using System.Collections.Generic;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;

namespace game.model.component {
    public struct PositionComponent {
        public Vector3Int position;
    }

    // for large objects
    // public struct MultiPositionComponent {
    //     public List<Vector3Int> positions;
    // }
    
    public struct NameComponent {
        public string name;
    }

    // for unit and designation
    public struct TaskComponent {
        public EcsEntity task;
    }

    // shows that entity is ready to be deleted
    public struct RemovedComponent { }

    public struct TaskCreationTimeoutComponent {
        public int value;
    }

    // units can own items and buildings.
    public struct OwnedComponent {
        public EcsEntity owner;
    }

    // tasks can lock other entities so other tasks should not select them
    public struct LockedComponent {
        public EcsEntity task;
    }

    // shows how well item or building is crafted. 
    public struct QualityComponent {
        public QualityEnum quality;
    }

    // event component
    public struct TaskAssignedComponent { }

    // see EcsEntityExtension
    public struct NullComponent { }

    public struct TileUpdateComponent {
        public HashSet<Vector3Int> tiles;
    }
    
    public struct TileVisualUpdateComponent {
        public HashSet<Vector3Int> tiles;
    }

// entity with this can contain items. Content should be updated only via StoredItemsManager
public struct ItemContainerComponent {
    public List<EcsEntity> items;
    public bool updated; // to track content changes from ui

    public void addItem(EcsEntity item) {
        items.Add(item);
        updated = true;
    }

    public void removeItem(EcsEntity item) {
        items.Remove(item);
        updated = true;
    }
}
}