using System.Collections.Generic;
using game.model.component.item;
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
        public int priority;
    }

    public struct StockpileOpenBringTaskComponent { // exists when zone is waiting for performer
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

    public struct ZoneDeletedComponent { }

    public struct StockpileComponent {
        public MultiValueDictionary<string, int> map; // allowed itemTypes -> materials
        public string preset; // TODO if preset is set, map in this component is empty, and config stored in ZoneContainer in localmodel
        public bool hasFreeTile; // TODO use this field for optimising checks on hauling tasks
        
        public bool itemAllowed(ItemComponent item) {
            return map.ContainsKey(item.type) && map[item.type].Contains(item.material);
        }
    }

    public struct StockpileTasksComponent {
        public HashSet<EcsEntity> bringTasks;
        public HashSet<EcsEntity> removeTasks;
    }

    public struct FarmComponent {
        public List<string> config; // stores plants allowed on farm
        public string plant;
    }

    // stores tiles which should be targeted by tesks
    public struct FarmTileTrackingComponent {
        public List<Vector3Int> toHoe; // soil floor tiles
        public List<Vector3Int> toPlant; // tiles without desired plant
        public List<Vector3Int> toRemove; // tiles with undesired plant
    }

    // stores created tasks (all states)
    public struct FarmTaskTrackingComponent {
        public List<EcsEntity> hoe;
        public Dictionary<Vector3Int, EcsEntity> plant;
        public Dictionary<Vector3Int, EcsEntity> remove; // TODO implement with designations
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