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

    // public struct ZoneUpdateComponent {
    //     public HashSet<Vector3Int> tiles;
    //
    //     public void add(Vector3Int tile) {
    //         if (tiles == null) tiles = new();
    //         tiles.Add(tile);
    //     }
    //
    //     public void add(ICollection<Vector3Int> values) {
    //         if (tiles == null) tiles = new();
    //         foreach (Vector3Int tile in values) {
    //             tiles.Add(tile);
    //         }
    //     }
    // }

    // present when zone is changed but changes not yet processed by visual system
    public struct ZoneVisualUpdateComponent {
        public List<Vector3Int> tiles;

        public void add(Vector3Int tile) {
            if (tiles == null) tiles = new();
            tiles.Add(tile);
        }
    }
    
    public struct StockpileComponent {
        public MultiValueDictionary<string, int> map; // allowed itemTypes -> materials
        public string preset; // TODO if preset is set, map in this component is empty, and config stored in ZoneContainer in localmodel
        public bool hasFreeTile; // TODO use this field for optimising checks on hauling tasks
    }

    // stores tiles which should be targeted by tesks
    public struct ZoneTrackingComponent {
        // TODO deprecated, use tilesToTask and totalTasks
        public Dictionary<Vector3Int, EcsEntity> locked; // tile -> task (locks tiles to tasks to ensure only one task can be performed on a tile)
        public Dictionary<Vector3Int, string> tilesToTask; // tile -> task type to perform
        public HashSet<EcsEntity> totalTasks;

        // removes tile from all task types
        public void removeTile(Vector3Int tile) {
            tilesToTask.Remove(tile);
        }
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
    
    public struct FarmOpenHarvestTaskComponent {
        public EcsEntity harvestTask;
    }

    public struct FarmOpenTaskComponent {
        public EcsEntity task;
    }
}