﻿using game.model.component;
using game.model.component.task.action.plant;
using game.model.container;
using Leopotam.Ecs;
using types.plant;
using util.lang.extension;
using static types.ZoneTaskTypes;
using Action = game.model.component.task.action.Action;

namespace game.model.system.zone {
    public class FarmTaskCreationSystem : LocalModelIntervalEcsSystem { // TODO make interval
        public EcsFilter<FarmComponent>.Exclude<FarmOpenHoeingTaskComponent> hoeingFilter;
        public EcsFilter<FarmComponent>.Exclude<FarmOpenPlantingTaskComponent> plantingFilter;
        private readonly TaskGenerator generator = new();

        public FarmTaskCreationSystem() : base(100) { }

        protected override void runIntervalLogic(int updates) {
            foreach (int i in hoeingFilter) {
                EcsEntity entity = hoeingFilter.GetEntity(i);
                FarmComponent farm = hoeingFilter.Get1(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                tryCreateHoeingTask(zone, farm, entity);
            }
            foreach (int i in plantingFilter) {
                EcsEntity entity = plantingFilter.GetEntity(i);
                FarmComponent farm = plantingFilter.Get1(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                tryCreatePlantingTask(entity, zone, farm);
            }
        }

        private void tryCreateHoeingTask(ZoneComponent zone, FarmComponent farm, EcsEntity entity) {
            if (farm.plant == null) return;
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            if (tracking.tiles[HOE].Count > 0) { // has tiles to hoe
                Action action = new FarmHoeingAction(entity);
                EcsEntity task = createTask(action);
                entity.Replace(new FarmOpenHoeingTaskComponent { hoeTask = task });
            }
        }

        private void tryCreatePlantingTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
            if (PlantTypeMap.get().get(farm.plant) != null && entity.take<ZoneTrackingComponent>().tiles[PLANT].Count > 0) {
                Action action = new PlantingAction(entity);
                EcsEntity task = createTask(action);
                entity.Replace(new FarmOpenPlantingTaskComponent { plantTask = task });
            }
        }

        // TODO add farmer job
        private EcsEntity createTask(Action action) {
            EcsEntity task = generator.createTask(action, model.createEntity(), model);
            model.taskContainer.addOpenTask(task);
            return task;
        }
    }
}