using System.Collections.Generic;
using game.model.component;
using game.model.component.task.action;
using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

namespace game.model.system.unit {
    // finds and assigns appropriate tasks to units
    public class UnitTaskAssignmentSystem : IEcsRunSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent> filter; // units without tasks

        public void Run() {
            foreach (int i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                EcsEntity? task = getTaskFromContainer(unit); 
                // TODO add needs
                if (task == null) task = createIdleTask(unit);
                assignTask(unit, task.Value);
            }
        }

        // gets any task for unit
        private EcsEntity? getTaskFromContainer(EcsEntity unit) {
            if (unit.Has<UnitJobsComponent>()) {
                UnitJobsComponent jobs = unit.Get<UnitJobsComponent>();
                Debug.Log("enabled jobs: " + jobs.enabledJobs.Count);
                
                foreach (var enabledJob in jobs.enabledJobs) { // TODO add jobs priorities
                    EcsEntity? task = GameModel.get().taskContainer.getTask(enabledJob, unit.pos());
                    if (task.HasValue) return task;
                }
            } else {
                Debug.LogError("unit without jobs component attempts to get jobs from container.");
            }
            return null; // TODO get from task container
        }

        private EcsEntity createIdleTask(EcsEntity unit) {
            // Debug.Log("creating idle task for unit " + unit);
            Vector3Int current = unit.pos();
            Vector3Int position = GameModel.localMap.util.getRandomPosition(current, 10, 4);
            return GameModel.get().taskContainer.createTask(new MoveAction(position));
        }

        // bind unit and task entities
        private void assignTask(EcsEntity unit, EcsEntity task) {
            unit.Replace(new TaskComponent { task = task });
            task.Replace(new TaskPerformerComponent { performer = unit });
        }
    }
}