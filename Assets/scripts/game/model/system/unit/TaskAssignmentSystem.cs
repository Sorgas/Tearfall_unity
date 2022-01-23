using System.Collections.Generic;
using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // finds and assigns appropriate tasks to units
    public class TaskAssignmentSystem : IEcsRunSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent> filter; // units without tasks

        public void Run() {
            foreach (int i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                Debug.Log("assigning task " + unit);
                EcsEntity? task = getTaskFromContainer(); 
                // TODO add needs
                if (task == null) task = createIdleTask(unit);
                assignTask(unit, task.Value);
            }
        }

        // gets any task for unit
        private EcsEntity? getTaskFromContainer() {
            // GameModel.get().taskContainer.openTasks;
            return null; // TODO get from task container
        }

        private EcsEntity createIdleTask(EcsEntity unit) {
            Debug.Log("creating idle task for unit " + unit);
            Vector3Int current = unit.Get<MovementComponent>().position;
            Vector3Int position = GameModel.localMap.util.getRandomPosition(current, 10, 4);
            EcsEntity task = GameModel.get().createEntity();
            task.Replace(new TaskActionsComponent { initialAction = new MoveAction(position), preActions = new List<Action>() });
            return task;
        }

        // bind unit and task entities
        private void assignTask(EcsEntity unit, EcsEntity task) {
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
        }
    }
}