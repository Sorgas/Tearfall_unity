using game.model.component;
using game.model.component.task;
using game.model.component.task.action.plant.farm;
using game.model.container.task;
using Leopotam.Ecs;
using types.unit;
using util.lang.extension;
using static types.ZoneTaskTypes;
using Action = game.model.component.task.action.Action;

namespace game.model.system.zone {

public class FarmTaskCreationSystem : LocalModelIntervalEcsSystem {
    public EcsFilter<FarmComponent>.Exclude<FarmOpenTaskComponent> workFilter;

    private readonly TaskGenerator generator = new();

    public FarmTaskCreationSystem() : base(100) { }

    protected override void runIntervalLogic(int updates) {
        foreach (int i in workFilter) {
            EcsEntity entity = workFilter.GetEntity(i);
            FarmComponent farm = workFilter.Get1(i);
            ZoneComponent zone = entity.take<ZoneComponent>();
            tryCreateWorkTask(entity, zone, farm);
        }
    }

    private void tryCreateWorkTask(EcsEntity entity, ZoneComponent zone, FarmComponent farm) {
        ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
        if (tracking.tilesToTask.Keys.Count > tracking.totalTasks.Count) {
            Action action = new FarmingAction(entity);
            int priority = entity.take<ZoneTasksComponent>().priority;
            EcsEntity task = createTask(action, priority, entity, FARM_WORK);
            entity.Replace(new FarmOpenTaskComponent { task = task });
            tracking.totalTasks.Add(task);
        }
    }

    private EcsEntity createTask(Action action, int priority, EcsEntity zone, string taskType) {
        EcsEntity task = generator.createTask(action, Jobs.FARMER, priority, model.createEntity(), model);
        task.Replace(new TaskZoneComponent { zone = zone, taskType = taskType });
        model.taskContainer.addOpenTask(task);
        return task;
    }
}
}