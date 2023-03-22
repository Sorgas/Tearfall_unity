using game.model.component;
using game.model.component.task.action.plant;
using game.model.container;
using game.model.localmap;
using Leopotam.Ecs;
using types.plant;
using util.lang.extension;
using Action = game.model.component.task.action.Action;

namespace game.model.system.zone {
    public class FarmTaskCreationSystem : LocalModelEcsSystem { // TODO make interval
        public EcsFilter<FarmComponent>.Exclude<FarmOpenHoeingTaskComponent> hoeingFilter;
        public EcsFilter<FarmComponent>.Exclude<FarmOpenPlantingTaskComponent> plantingFilter;
        private readonly TaskGenerator generator = new();

        public FarmTaskCreationSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in hoeingFilter) {
                EcsEntity entity = hoeingFilter.GetEntity(i);
                FarmComponent farm = hoeingFilter.Get1(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                FarmTileTrackingComponent tracking = entity.take<FarmTileTrackingComponent>();
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
            FarmTileTrackingComponent tracking = entity.take<FarmTileTrackingComponent>();
            if (tracking.toHoe.Count > 0) { // has tiles to hoe
                Action action = new FarmHoeingAction(entity);
                EcsEntity task = createTask(action);
                entity.Replace(new FarmOpenHoeingTaskComponent { hoeTask = task });
            }
        }

        private void tryCreatePlantingTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
            if (PlantTypeMap.get().get(farm.plant) != null && entity.take<FarmTileTrackingComponent>().toPlant.Count > 0) {
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