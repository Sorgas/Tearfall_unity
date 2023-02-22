using System.Collections.Generic;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang;

namespace game.model.component {
    public struct ZoneComponent {
        public List<Vector3Int> tiles;
        public ZoneTypeEnum type;
        public int number; // used for name generation
    }

    public struct ZoneTasksComponent {
        public bool paused; // new tasks not generated when paused.
        public HashSet<EcsEntity> bringTasks;
        public HashSet<EcsEntity> removeTasks;
        public int priority;
    }

    public struct ZoneOpenTaskComponent { // exists when zone is waiting for performer
        public EcsEntity task;
    }
    
    public struct ZoneVisualComponent {
        public List<Vector3Int> tiles;
    }

    // present when zone is changed but changes not yet processed by visual system
    public struct ZoneUpdatedComponent {
        public List<Vector3Int> tiles;
    }

    public struct ZoneDeletedComponent { }

    public struct StockpileComponent {
        public MultiValueDictionary<string, int> map; // allowed itemTypes -> materials
        public string preset; // TODO if preset is set, map in this component is empty, and config stored in ZoneContainer in localmodel
        public bool hasFreeTile; // TODO use this field for optimising checks on hauling tasks
    }
}