using game.model.component;
using game.model.component.task;
using game.model.component.task.action.plant;
using game.model.container;
using game.model.container.task;
using Leopotam.Ecs;
using util.lang.extension;
using static types.ZoneTaskTypes;
using Action = game.model.component.task.action.Action;

namespace game.model.system.zone {
    public class FarmTaskCreationSystemOld : LocalModelIntervalEcsSystem {
        // public EcsFilter<FarmComponent>.Exclude<FarmOpenHoeingTaskComponent> hoeingFilter;
        // public EcsFilter<FarmComponent>.Exclude<FarmOpenPlantingTaskComponent> plantingFilter;
        // public EcsFilter<FarmComponent>.Exclude<FarmOpenRemovingTaskComponent> removeFilter;
        // public EcsFilter<FarmComponent>.Exclude<FarmOpenHarvestTaskComponent> harvestFilter;
        public EcsFilter<FarmComponent>.Exclude<FarmOpenTaskComponent> workFilter;

        private readonly TaskGenerator generator = new();

        public FarmTaskCreationSystemOld() : base(100) { }

        protected override void runIntervalLogic(int updates) {
            // foreach (int i in hoeingFilter) {
            //     EcsEntity entity = hoeingFilter.GetEntity(i);
            //     FarmComponent farm = hoeingFilter.Get1(i);
            //     ZoneComponent zone = entity.take<ZoneComponent>();
            //     tryCreateHoeingTask(zone, farm, entity);
            // }
            // foreach (int i in plantingFilter) {
            //     EcsEntity entity = plantingFilter.GetEntity(i);
            //     FarmComponent farm = plantingFilter.Get1(i);
            //     ZoneComponent zone = entity.take<ZoneComponent>();
            //     tryCreatePlantingTask(entity, zone, farm);
            // }
            // foreach (int i in removeFilter) {
            //     EcsEntity entity = removeFilter.GetEntity(i);
            //     FarmComponent farm = removeFilter.Get1(i);
            //     ZoneComponent zone = entity.take<ZoneComponent>();
            //     tryCreateRemovingTask(entity, zone, farm);
            // }
            // foreach (int i in harvestFilter) {
            //     EcsEntity entity = removeFilter.GetEntity(i);
            //     FarmComponent farm = removeFilter.Get1(i);
            //     ZoneComponent zone = entity.take<ZoneComponent>();
            //     tryCreateHarvestTask(entity, zone, farm);
            // }
            // foreach (int i in workFilter) {
            //     EcsEntity entity = workFilter.GetEntity(i);
            //     FarmComponent farm = workFilter.Get1(i);
            //     ZoneComponent zone = entity.take<ZoneComponent>();
            //     tryCreateWorkTask(entity, zone, farm);
            // }
        }

        // private void tryCreateHoeingTask(ZoneComponent zone, FarmComponent farm, EcsEntity entity) {
        //     ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
        //     if (hasEnoughTiles(tracking, HOE)) { // there more tiles than tasks
        //         Action action = new FarmHoeingAction(entity);
        //         EcsEntity task = createTask(action, entity, HOE);
        //         entity.Replace(new FarmOpenHoeingTaskComponent { hoeTask = task });
        //         tracking.tasks[HOE].Add(task);
        //     }
        // }
        //
        // private void tryCreatePlantingTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
        //     ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
        //     if (farm.plant != null && hasEnoughTiles(tracking, PLANT)) {
        //         Action action = new PlantingAction(entity);
        //         EcsEntity task = createTask(action, entity, PLANT);
        //         entity.Replace(new FarmOpenPlantingTaskComponent { plantTask = task });
        //         // Debug.Log("FarmTaskCreationSystem] plant task created for " + entity.name());
        //         tracking.tasks[PLANT].Add(task);
        //     }
        // }
        //
        // private void tryCreateRemovingTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
        //
        // }
        //
        // private void tryCreateHarvestTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
        //     ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
        //     if (hasEnoughTiles(tracking, HARVEST)) {
        //         Action action = new FarmHarvestAction(entity);
        //         EcsEntity task = createTask(action, entity, HARVEST);
        //         entity.Replace(new FarmOpenHarvestTaskComponent { harvestTask = task });
        //         tracking.tasks[HARVEST].Add(task);
        //     }
        // }
        //
        // private void tryCreateWorkTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
        //     ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
        //     if (tracking.tilesToTask.Keys.Count > tracking.totalTasks.Count) {
        //         Action action = new FarmingAction(entity);
        //         EcsEntity task = createTask(action, entity, FARM_WORK);
        //         entity.Replace(new FarmOpenTaskComponent { task = task });
        //         tracking.tasks[FARM_WORK].Add(task);
        //         tracking.totalTasks.Add(task);
        //     }
        // }
        //
        // private bool hasEnoughTiles(ZoneTrackingComponent tracking, string taskType) {
        //     return tracking.tiles[taskType].Count > tracking.tasks[taskType].Count;
        // }

        // TODO add farmer job
        // private EcsEntity createTask(Action action, EcsEntity zone, string taskType) {
        //     EcsEntity task = generator.createTask(action, model.createEntity(), model);
        //     task.Replace(new TaskZoneComponent { zone = zone, taskType = taskType });
        //     model.taskContainer.addOpenTask(task);
        //     return task;
        // }
    }
}