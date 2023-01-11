using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using types.unit;
using static game.model.component.task.TaskComponents;

namespace game.model.container {
    // creates task entities
    public class TaskGenerator {
        public EcsEntity createTask(Action initialAction, EcsEntity entity, LocalModel model) => createTask(initialAction, TaskPriorityEnum.JOB, entity, model);
        
        public EcsEntity createTask(Action initialAction, TaskPriorityEnum priority, EcsEntity entity, LocalModel model) {
            entity.Replace(new TaskActionsComponent { initialAction = initialAction, preActions = new List<Action>(), model = model });
            entity.Replace(new TaskPriorityComponent { priority = priority });
            entity.Replace(new TaskLockedItemsComponent { lockedItems = new() });
            entity.Replace(new NameComponent { name = initialAction.name });
            entity.Replace(new TaskJobComponent{job = Jobs.NONE.name});
            initialAction.task = entity;
            return entity;
        } 
    }
}  