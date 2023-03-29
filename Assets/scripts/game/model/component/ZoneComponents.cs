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

        public T take<T>() {
            throw new System.NotImplementedException();
        }
    }

    public struct ZoneTasksComponent {
        public bool paused; // new tasks not generated when paused.
        public int priority;
    }

    public struct StockpileOpenStoreTaskComponent { // exists when zone is waiting for performer
        public EcsEntity bringTask;
    }

    public struct StockpileOpenRemoveTaskComponent {
        public EcsEntity removeTask;
    }
    
    public struct ZoneVisualComponent {
        public List<Vector3Int> tiles;
    }

    public struct ZoneUpdatedComponent {
        public List<Vector3Int> tiles;
    }

    // present when zone is changed but changes not yet processed by visual system
    public struct ZoneVisualUpdatedComponent {
        public List<Vector3Int> tiles;
    }
    
    // TODO make zone deletion instant: ui event -> delete from containers, cancel tasks, delete visual tiles (add and use special un-pausable systems)
    public struct ZoneDeletedComponent { }

    public struct StockpileComponent {
        public MultiValueDictionary<string, int> map; // allowed itemTypes -> materials
        public string preset; // TODO if preset is set, map in this component is empty, and config stored in ZoneContainer in localmodel
        public bool hasFreeTile; // TODO use this field for optimising checks on hauling tasks
    }

    // stores tiles which should be targeted by tesks
    public struct ZoneTrackingComponent {
        public Dictionary<string, HashSet<EcsEntity>> tasks; // task type -> task (just stores tasks associated with zone)
        public Dictionary<string, HashSet<Vector3Int>> tiles; // task type -> tiles (tracks tiles to perform tasks to)
        public Dictionary<Vector3Int, EcsEntity> locked; // tile -> task (locks tiles to tasks to ensure only one task can be performed on a tile)
    }

    public struct FarmComponent {
        public string plant;
    }

    public struct FarmOpenHoeingTaskComponent {
        public EcsEntity hoeTask;
    }
    
    public struct FarmOpenPlantingTaskComponent {
        public EcsEntity plantTask;
    }
    
    public struct FarmOpenRemovingTaskComponent {
        public EcsEntity removeTask;
    }
}