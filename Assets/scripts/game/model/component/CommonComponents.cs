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
    public struct MultiPositionComponent {
        public List<Vector3Int> positions;
    }
    
    public struct NameComponent {
        public string name;
    }

    // for unit and designation
    public struct TaskComponent {
        public EcsEntity task;
    }

    public struct TaskFinishedComponent {
        public TaskStatusEnum status;
        public string reason; // TODO no-materials, combat, 
    }

    // shows that entity is ready to be deleted
    public struct RemovedComponent { }

    public struct TaskCreationTimeoutComponent {
        public int value;
    }

    // indicates when entity is changed 
    public struct UiUpdateComponent { }

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
}