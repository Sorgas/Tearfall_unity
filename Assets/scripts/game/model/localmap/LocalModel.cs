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
        
        // containers for referencing and CRUD on entities
        public readonly UnitContainer unitContainer = new();
        public readonly DesignationContainer designationContainer;
        public readonly TaskContainer taskContainer;
        public readonly ItemContainer itemContainer;
        public readonly PlantContainer plantContainer;
        public readonly BuildingContainer buildingContainer;
        public readonly ZoneContainer zoneContainer;
        public readonly FarmContainer farmContainer;
        public readonly TileUpdateUtil updateUtil;

        private readonly List<ModelUpdateEvent> modelUpdateEventQueue = new();

        public LocalModel() {
            Debug.Log("creating EcsWorld");
            designationContainer = new(this);
            taskContainer = new(this);
            itemContainer = new(this);
            plantContainer = new(this);
            buildingContainer = new(this);
            zoneContainer = new(this);
            farmContainer = new(this);
            updateUtil = new(this);
        }

        public void update(int ticks) {
            scalableSystems?.Run();
            for (var i = 0; i < ticks; i++) {
                unscalableSystems?.Run();
            }
        }

        public void init() {
            Debug.Log("initializing local model");
            new LocalModelSystemsInitializer().init(this);
            localMap.init();
            Debug.Log("local model initialized");
        }

        //TODO move ecs world to global game model. (as units should travel between regions)
        public EcsEntity createEntity() {
            EcsEntity entity = ecsWorld.NewEntity();
            // Debug.Log("created entity: " + entity.GetInternalId());
            return entity;
        }

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

        public string getDebugInfo() => taskContainer.getDebugIngo();
    }
} 