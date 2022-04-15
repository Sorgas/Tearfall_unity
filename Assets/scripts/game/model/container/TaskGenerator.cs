using System.Collections.Generic;
using enums.action;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using Leopotam.Ecs;

namespace game.model.container {
    // creates task entities
    public class TaskGenerator {
        public EcsEntity createTask(Action initialAction) => createTask(initialAction, TaskPriorityEnum.JOB);
        
        public EcsEntity createTask(Action initialAction, TaskPriorityEnum priority) {
            EcsEntity task = GameModel.get().createEntity();
            task.Replace(new TaskComponents.TaskActionsComponent { initialAction = initialAction, preActions = new List<Action>() });
            task.Replace(new TaskComponents.TaskPriorityComponent { priority = priority });
            task.Replace(new NameComponent { name = initialAction.name });
            initialAction.task = task;
            return task;
        }
    }
}