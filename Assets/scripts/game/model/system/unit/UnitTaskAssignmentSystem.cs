using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.unit {
    /**
     * Find and assigns tasks for units without tasks.
     * 
     * When searching task for unit:
     * For unit's jobs in priority descending order:
     *      Tasks of these jobs and with available targets are taken from container.
     *      For task priorities in descending order:
     *          Select tasks of current priority
     *          For tasks by ascending of distance to target
     *              Check task conditions with creating sub actions
     *              If success, assign task to unit
     *              If fail, add timeout component to task, return to container.
     */
    public class UnitTaskAssignmentSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<UnitComponent>.Exclude<TaskComponent, TaskFinishedComponent> filter; // units without tasks
        private readonly UnitNeedActionCreator needActionCreator = new();

        public override void Run() {
            foreach (int i in filter) {
                ref EcsEntity unit = ref filter.GetEntity(i);
                EcsEntity task = tryCreateTask(unit);
                if (task != EcsEntity.Null) assignTask(ref unit, task);
            }
        }

        private EcsEntity tryCreateTask(EcsEntity unit) {
            if (unit.Has<UnitNextTaskComponent>()) return createNextTask(unit);
            // TODO compare max priority of needs and tasks before lookup in container

            EcsEntity jobTask = getTaskFromContainer(unit);
            EcsEntity needTask = needActionCreator.selectAndCreateAction(model, unit);
            EcsEntity task = priority(jobTask) > priority(needTask) ? jobTask : needTask;
            // if (task.IsNull()) task = createIdleTask(unit);
            return task;
        }

        private EcsEntity createNextTask(EcsEntity unit) {
            return model.taskContainer.generator
                .createTask(unit.take<UnitNextTaskComponent>().action, model.createEntity(), model);
        }

        private EcsEntity getTaskFromContainer(EcsEntity unit) {
            // get tasks in prioritized order 
            // in loop check tasks by creating sub-actions
            // if ok, assign task
            // if failed, add timeout to task and move to delayed tasks map
            
            

            UnitJobsComponent jobs = unit.take<UnitJobsComponent>();
            for (int i = TaskPriorities.range.min; i <= TaskPriorities.range.max; i++) {
                List<string> jobsList = jobs.getByPriority(i);
                return model.taskContainer.findTask(jobsList, unit.pos());

            }
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
            Debug.Log("[UnitTaskAssignmentSystem] assigning task [" + task.name() + "] to " + unit.name());
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
            task.Replace(new TaskAssignedComponent());
            model.taskContainer.claimTask(task, unit);
        }

        private TaskPriorities priority(EcsEntity task) {
            return task.IsNull() ? TaskPriorities.NONE : task.take<TaskActionsComponent>().priority;
        }
    }
}