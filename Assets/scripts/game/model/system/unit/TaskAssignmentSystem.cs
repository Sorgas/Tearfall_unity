using System.Collections.Generic;
using Assets.scripts.game.model;
using Leopotam.Ecs;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.model.system.unit {
    public class TaskAssignmentSystem : IEcsRunSystem {
        EcsFilter<UnitComponent>.Exclude<TaskComponent> filter;
        // EcsFilter<TaskComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity unit = filter.GetEntity(i);
                // TODO get task from container
                TaskComponent? task = getTaskForUnit();
                // TODO add needs
                if (task == null) task = createIdleTask(unit);
                if (task != null) unit.Replace<TaskComponent>((TaskComponent)task);
            }
        }

        // gets any task for unit
        private TaskComponent? getTaskForUnit() {
            return null; // TODO get from task container
        }

        private TaskComponent? createIdleTask(EcsEntity unit) {
            Vector3Int current = unit.Get<MovementComponent>().position;
            Vector3Int position = GameModel.get().localMap.util.getRandomPosition(current, 10, 4);
            if (position != null) {
                TaskComponent task = new TaskComponent() { preActions = new List<_Action>() };
                task.initialAction = new MoveAction(position);
                return task;
            }
            return null;
        }
    }
}