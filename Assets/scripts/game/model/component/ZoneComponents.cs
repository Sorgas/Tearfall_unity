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

    // present when zone is changed but changes not yet processed by visual system
    public struct ZoneUpdatedComponent {
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
}