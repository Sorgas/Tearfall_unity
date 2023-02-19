using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.zone;
using game.model.container;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.zone {
    // creates open task for zone without open task
    public class ZoneTaskCreationSystem : LocalModelEcsSystem {
        public EcsFilter<StockpileComponent>.Exclude<ZoneOpenTaskComponent, TaskCreationTimeoutComponent> stockpileFilter;

        public ZoneTaskCreationSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in stockpileFilter) {
                EcsEntity entity = stockpileFilter.GetEntity(i);
                StockpileComponent stockpile = stockpileFilter.Get1(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                ZoneTasksComponent tasks = entity.take<ZoneTasksComponent>();
            }
        }

        private EcsEntity createTask(StockpileComponent stockpile, ZoneComponent zone, ZoneTasksComponent tasks) {
            if (stockpile.map.Count > 0) {
                Vector3Int freeCell = getFreeCell(stockpile, zone, tasks);
                if (freeCell.z >= 0) {
                    // Action action = new HaulItemToStockpileAction(new HaulItemToStockpileAction())
                    // TaskGenerator generator = new();
                    // generator.createTask(, TaskPriorityEnum.JOB, model.createEntity(), model);
                }
            }
            return EcsEntity.Null;
        }

        // find tile without items or tasks
        private Vector3Int getFreeCell(StockpileComponent stockpile, ZoneComponent zone, ZoneTasksComponent tasks) {
            List<Vector3Int> tiles = zone.tiles
                .Where(tile => !stockpile.items.ContainsKey(tile))
                .Where(tile => !tasks.tasks.ContainsKey(tile))
                .ToList();
            return tiles.Count == 0 ? Vector3Int.back : tiles[0];
        } 
    }
}