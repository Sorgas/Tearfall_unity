using enums.action;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component {
    public struct PositionComponent {
        public Vector3Int position;
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
    }

    // shows that entity is ready to be deleted
    public struct DeletionComponent {
        
    }
}