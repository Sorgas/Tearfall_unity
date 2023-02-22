using game.model.component;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.target;
using game.model.component.task.action.zone;
using game.model.container;
using game.model.localmap;
using game.model.util;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.zone {
    // creates open task for zone without open task
    public class ZoneTaskCreationSystem : LocalModelEcsSystem {
        public EcsFilter<StockpileComponent>.Exclude<ZoneOpenTaskComponent, TaskCreationTimeoutComponent> stockpileFilter;
        private readonly TaskGenerator generator = new();

        public ZoneTaskCreationSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in stockpileFilter) {
                EcsEntity entity = stockpileFilter.GetEntity(i);
                StockpileComponent stockpile = stockpileFilter.Get1(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                ZoneTasksComponent tasks = entity.take<ZoneTasksComponent>();
                EcsEntity task = createTaskForStockpile(stockpile, zone, tasks, entity);
                if (task != EcsEntity.Null) {
                    Debug.Log("stockpile task created");
                    task.Replace(new TaskComponents.TaskZoneComponent { zone = entity });
                    entity.Replace(new ZoneOpenTaskComponent { task = task });
                    model.taskContainer.addOpenTask(task);
                } else {
                    entity.Replace(new TaskCreationTimeoutComponent { value = 500 });
                    // add idle component
                }
            }
        }

        private EcsEntity createTaskForStockpile(StockpileComponent stockpile, ZoneComponent zone, ZoneTasksComponent tasks, EcsEntity entity) {
            if (stockpile.map.Count > 0) {
                int freeCells = ZoneUtils.countFreeStockpileCells(zone, stockpile, model);
                if (tasks.bringTasks.Count < freeCells) {
                    Action action = new HaulItemToStockpileAction(new StockpileActionTarget(entity));
                    EcsEntity task = generator.createTask(action, TaskPriorityEnum.JOB, model.createEntity(), model);
                    return task;
                } else {
                    EcsEntity item = findUndesiredItem();
                    if (item != EcsEntity.Null) {
                        // create task to remove item from zone
                    }
                }
            }
            return EcsEntity.Null;
        }

        private EcsEntity findUndesiredItem() {
            return EcsEntity.Null;
        }
    }
}