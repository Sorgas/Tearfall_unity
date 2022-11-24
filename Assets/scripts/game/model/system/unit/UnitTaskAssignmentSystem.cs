using enums.action;
using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit
{
    // finds and assigns appropriate tasks to units
    public class UnitTaskAssignmentSystem : LocalModelEcsSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter; // units without tasks
        private UnitNeedActionCreator needActionCreator = new();

        public UnitTaskAssignmentSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                EcsEntity task = tryCreateTask(unit);
                if (!task.IsNull()) assignTask(ref unit, task);
            }
        }

        private EcsEntity tryCreateTask(EcsEntity unit) {
            EcsEntity jobTask = getTaskFromContainer(unit);
            EcsEntity needTask = needActionCreator.selectAndCreateAction(model, unit);
            EcsEntity task = priority(jobTask) > priority(needTask) ? jobTask : needTask;
            // if (task.IsNull()) task = createIdleTask(unit);
            return task;
        }

        // TODO add jobs priorities
        private EcsEntity getTaskFromContainer(EcsEntity unit) {
            UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
            return model.taskContainer.findTask(jobs.enabledJobs, unit.pos());
        }

        private EcsEntity createIdleTask(EcsEntity unit) {
            Vector3Int current = unit.pos();
            // Debug.Log("creating idle task for unit in position " + current);
            Vector3Int? position = model.localMap.util.getRandomPosition(current, 10, 4);
            return position.HasValue
                ? model.taskContainer.generator.createTask(new MoveAction(position.Value), model.createEntity(), model)
                : EcsEntity.Null;
        }

        // bind unit and task entities
        private void assignTask(ref EcsEntity unit, EcsEntity task) {
            Debug.Log("[UnitTaskAssignmentSystem] assigning task " + task.name() + " to " + unit.name());
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
            model.taskContainer.claimTask(task, unit);
        }

        private TaskPriorityEnum priority(EcsEntity task) {
            return task.IsNull() ? TaskPriorityEnum.NONE : task.take<TaskPriorityComponent>().priority;
        }
    }
}