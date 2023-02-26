using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.action.zone;
using game.model.container;
using game.model.localmap;
using game.model.util;
using Leopotam.Ecs;
using types;
using types.action;
using types.item.type;
using UnityEditor;
using UnityEngine;
using util.geometry;
using util.lang;
using util.lang.extension;

namespace game.model.system.zone {
    // creates tasks for bringing and removing items to/from stockpiles
    // TODO when task is assigned, add marker component to it. if task of stockpile, move it to assigned tasks of zone
    // TODO when item put on ground, add marker component to it. if put into stockpile, update free cells etc.
    public class StockpileTaskCreationSystem : LocalModelEcsSystem {
        public EcsFilter<StockpileComponent>.Exclude<StockpileOpenBringTaskComponent, TaskCreationTimeoutComponent> bringFilter;
        public EcsFilter<StockpileComponent>.Exclude<StockpileOpenRemoveTaskComponent, TaskCreationTimeoutComponent> removeFilter;
        private readonly TaskGenerator generator = new(); 

        public StockpileTaskCreationSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in bringFilter) {
                EcsEntity entity = bringFilter.GetEntity(i);
                StockpileComponent stockpile = bringFilter.Get1(i);
                tryCreateBringTask(stockpile, entity);
            }
            foreach (int i in removeFilter) {
                EcsEntity entity = removeFilter.GetEntity(i);
                StockpileComponent stockpile = removeFilter.Get1(i);
                tryCreateRemoveTask(stockpile, entity);
            }
        }

        private void tryCreateBringTask(StockpileComponent stockpile, EcsEntity entity) {
            Action action = createBringAction(stockpile, entity.take<ZoneComponent>(), entity.take<StockpileTasksComponent>(), entity);
            if (action == null) return;
            EcsEntity task = createTask(action, entity);
            entity.Replace(new StockpileOpenBringTaskComponent { bringTask = task });
        }

        private void tryCreateRemoveTask(StockpileComponent stockpile, EcsEntity entity) {
            Action action = createRemoveAction(stockpile, entity.take<ZoneComponent>(), entity.take<StockpileTasksComponent>(), entity);
            if (action == null) return;
            EcsEntity task = createTask(action, entity);
            entity.Replace(new StockpileOpenRemoveTaskComponent { removeTask = task });
        }
        
        private Action createBringAction(StockpileComponent stockpile, ZoneComponent zone, StockpileTasksComponent tasks, EcsEntity entity) {
            if (stockpile.map.Count <= 0) return null;
            int freeCells = ZoneUtils.countFreeStockpileCells(zone, stockpile, model);
            if (tasks.bringTasks.Count < freeCells) {
                return new HaulItemToStockpileAction(new StockpileActionTarget(entity));
            }
            return null;
        }

        private Action createRemoveAction(StockpileComponent stockpile, ZoneComponent zone, StockpileTasksComponent tasks, EcsEntity entity) {
            EcsEntity item = findUndesiredItem(zone, stockpile);
            if (item == EcsEntity.Null) return null;
            Vector3Int targetPosition = findFreeCellOffZone(zone.tiles, item, new List<Vector3Int>(zone.tiles));
            if (targetPosition != Vector3Int.back) {
                return new PutItemToPositionAction(item, targetPosition);
            }
            return null;
        }

        private EcsEntity findUndesiredItem(ZoneComponent zone, StockpileComponent stockpile) {
            MultiValueDictionary<string, int> config = stockpile.map;
            List<EcsEntity> items = zone.tiles
                .SelectMany(tile => model.itemContainer.onMap.itemsOnMap.get(tile))
                .Where(item => {
                    ItemComponent component = item.take<ItemComponent>();
                    return !stockpile.itemAllowed(component);
                })
                .ToList();
            return items.Count > 0 ? items[0] : EcsEntity.Null;
        }

        // finds free tile or tile with not full stack of same items
        private Vector3Int findFreeCellOffZone(List<Vector3Int> tiles, EcsEntity item, List<Vector3Int> searchedTiles) {
            ItemComponent itemComponent = item.take<ItemComponent>();
            int zoneArea = model.localMap.passageMap.area.get(tiles[0]);
            List<Vector3Int> passableNeighbours =
                tiles
                    .SelectMany(tile => PositionUtil.fourNeighbour.Select(pos => pos + tile))
                    .Distinct()
                    .Where(tile => !searchedTiles.Contains(tile))
                    .Where(tile => model.localMap.passageMap.area.get(tile) == zoneArea).ToList();
            Vector3Int freeTile = passableNeighbours
                // .Where(tile => model.localMap.blockType.get(tile) == BlockTypes.FLOOR.CODE)
                .firstOrDefault(tile => tileCanAcceptItem(tile, itemComponent), Vector3Int.back);
            if (freeTile != Vector3Int.back) return freeTile;
            searchedTiles.AddRange(passableNeighbours);
            return findFreeCellOffZone(passableNeighbours, item, searchedTiles);
        }

        private bool tileCanAcceptItem(Vector3Int tile, ItemComponent item) {
            if (!model.itemContainer.onMap.itemsOnMap.ContainsKey(tile)) return true;
            List<EcsEntity> items = model.itemContainer.onMap.itemsOnMap[tile];
            ItemType type = ItemTypeMap.getItemType(item.type);
            if (items.Count >= type.stackSize) return false;
            ItemComponent item2 = items[0].take<ItemComponent>();
            if (item2.type == item.type && item2.material == item.material) {
                return items.Count < type.stackSize;
            }
            return false;
        }

        private EcsEntity createTask(Action action, EcsEntity zone) {
            EcsEntity task = generator.createTask(action, TaskPriorityEnum.JOB, model.createEntity(), model);
            task.Replace(new TaskZoneComponent { zone = zone });
            model.taskContainer.addOpenTask(task);
            return task;
        }
    }
}