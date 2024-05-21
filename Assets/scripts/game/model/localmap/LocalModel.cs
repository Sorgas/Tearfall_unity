using System.Collections.Generic;
using game.model.container;
using game.model.container.item;
using game.model.container.task;
using game.model.localmap.update;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.localmap { // contains LocalMap and ECS world for its entities
    public class LocalModel {
        public LocalMap localMap;
        
        public readonly EcsWorld ecsWorld = new();
        public EcsSystems unscalableSystems;
        public EcsSystems scalableSystems; // for scalable and interval systems
        public EcsSystems timeIndependentSystems; // for scalable and interval systems
        
        // containers for referencing and CRUD on entities
        public readonly UnitContainer unitContainer = new();
        public readonly DesignationContainer designationContainer;
        public readonly TaskContainer taskContainer;
        public readonly ItemContainer itemContainer;
        public readonly PlantContainer plantContainer;
        public readonly BuildingContainer buildingContainer;
        public readonly ZoneContainer zoneContainer;
        public readonly FarmContainer farmContainer;
        public readonly RoomContainer roomContainer;
        public readonly TileUpdateUtil updateUtil;
        
        private readonly List<ModelUpdateEvent> modelUpdateEventQueue = new();
        private readonly bool debug = false;
        
        public LocalModel() {
            log("creating EcsWorld");
            designationContainer = new(this);
            taskContainer = new(this);
            itemContainer = new(this);
            plantContainer = new(this);
            buildingContainer = new(this);
            zoneContainer = new(this);
            farmContainer = new(this);
            roomContainer = new(this);
            updateUtil = new(this);
        }

        public void update(int ticks) {
            if (ticks != 0) {
                scalableSystems?.Run();
            }
            for (var i = 0; i < ticks; i++) {
                unscalableSystems?.Run();
            }
            timeIndependentSystems?.Run();
        }

        public void init() {
            log("initializing local model");
            new LocalModelSystemsInitializer().init(this);
            localMap.init();
            log("local model initialized");
        }

        //TODO move ecs world to global game model. (as units should travel between regions)
        public EcsEntity createEntity() => ecsWorld.NewEntity();

        // to receive update events from UI. 
        public void addUpdateEvent(ModelUpdateEvent newEvent) {
            lock (modelUpdateEventQueue) {
                modelUpdateEventQueue.Add(newEvent);
            }
        }

        public List<ModelUpdateEvent> getUpdateEvents() {
            lock (modelUpdateEventQueue) {
                List<ModelUpdateEvent> result = new(modelUpdateEventQueue);
                modelUpdateEventQueue.Clear();
                return result;
            }
        }
        
        private void log(string message) {
            if(debug) Debug.Log($"[LocalModel]: {message}");
        }
    }
} 