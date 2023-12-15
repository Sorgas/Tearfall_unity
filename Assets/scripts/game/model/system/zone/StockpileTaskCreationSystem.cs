using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.item;
using game.model.component.task;
using game.model.component.task.action;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.zone;
using game.model.container;
using game.model.container.task;
using game.model.util;
using Leopotam.Ecs;
using types;
using types.action;
using types.item.type;
using types.unit;
using UnityEngine;
using util.geometry;
using util.lang;
using util.lang.extension;

namespace game.model.system.zone {
    // TODO when item put on ground, add marker component to it. if put into stockpile, update free cells etc.
    // creates tasks for bringing and removing items to/from stockpiles
    public class StockpileTaskCreationSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<StockpileComponent>.Exclude<StockpileOpenStoreTaskComponent, TaskCreationTimeoutComponent> storeFilter;
        public EcsFilter<StockpileComponent>.Exclude<StockpileOpenRemoveTaskComponent, TaskCreationTimeoutComponent> removeFilter;
        private readonly TaskGenerator generator = new();

        public override void Run() {
            foreach (int i in storeFilter) {
                EcsEntity entity = storeFilter.GetEntity(i);
                StockpileComponent stockpile = storeFilter.Get1(i);
                tryCreateStoreTask(stockpile, entity);
            }
            foreach (int i in removeFilter) {
                EcsEntity entity = removeFilter.GetEntity(i);
                StockpileComponent stockpile = removeFilter.Get1(i);
                tryCreateRemoveTask(stockpile, entity);
            }
        }
        
        private void tryCreateStoreTask(StockpileComponent stockpile, EcsEntity entity) {
            Action action = tryCreateStoreAction(stockpile, entity.take<ZoneComponent>(), entity.take<ZoneTrackingComponent>(), entity);
            if (action == null) return; // TODO add timeout component when action not created
            int priority = entity.take<ZoneTasksComponent>().priority;
            EcsEntity task = createTask(action, priority, entity, ZoneTaskTypes.STORE_ITEM);
            entity.Replace(new StockpileOpenStoreTaskComponent { bringTask = task });
        }

        private void tryCreateRemoveTask(StockpileComponent stockpile, EcsEntity entity) {
            Action action = tryCreateRemoveAction(stockpile, entity.take<ZoneComponent>());
            if (action == null) return;
            int priority = entity.take<ZoneTasksComponent>().priority;
            EcsEntity task = createTask(action, priority, entity, ZoneTaskTypes.REMOVE_ITEM);
            entity.Replace(new StockpileOpenRemoveTaskComponent { removeTask = task });
        }
  
        private Action tryCreateStoreAction(StockpileComponent stockpile, ZoneComponent zone, ZoneTrackingComponent tracking, EcsEntity entity) {
            if (stockpile.map.Count <= 0) return null; // stockpile not configured
            if (ZoneUtils.countFreeStockpileTiles(zone, stockpile, tracking, model) <= 0) return null; // all tiles are locked or filled
            return new StoreItemToStockpileAction(entity);
        }

        private Action tryCreateRemoveAction(StockpileComponent stockpile, ZoneComponent zone) {
            EcsEntity item = findUndesiredItem(zone, stockpile);
            if (item == EcsEntity.Null) return null;
            Vector3Int targetPosition = findFreeCellOffZone(zone.tiles, item, new List<Vector3Int>(zone.tiles));
            if (targetPosition == Vector3Int.back) return null;
            return new PutItemToPositionAction(item, targetPosition);
        }

        private EcsEntity findUndesiredItem(ZoneComponent zone, StockpileComponent stockpile) {
            MultiValueDictionary<string, int> config = stockpile.map;
            List<EcsEntity> items = zone.tiles
                .SelectMany(tile => model.itemContainer.onMap.itemsOnMap.get(tile))
                .Where(item => {
                    ItemComponent component = item.take<ItemComponent>();
                    return !ZoneUtils.itemAllowedInStockpile(stockpile, component);
                })
                .ToList();
            return items.Count > 0 ? items[0] : EcsEntity.Null;
        }

        // TODO move to remove action
        // finds free tile or tile with not full stack of same items
        private Vector3Int findFreeCellOffZone(List<Vector3Int> tiles, EcsEntity item, List<Vector3Int> searchedTiles) {
            ItemComponent itemComponent = item.take<ItemComponent>();
            int zoneArea = model.localMap.passageMap.defaultHelper.area.get(tiles[0]);
            List<Vector3Int> passableNeighbours =
                tiles
                    .SelectMany(tile => PositionUtil.fourNeighbour.Select(pos => pos + tile))
                    .Distinct()
                    .Where(tile => !searchedTiles.Contains(tile))
                    .Where(tile => model.localMap.passageMap.defaultHelper.area.get(tile) == zoneArea).ToList();
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

        private EcsEntity createTask(Action action, int priority, EcsEntity zone, string taskType) {
            EcsEntity task = generator.createTask(action, Jobs.HAULER, priority, model.createEntity(), model);
            task.Replace(new TaskZoneComponent { zone = zone, taskType = taskType });
            zone.take<ZoneTrackingComponent>().totalTasks.Add(task);
            model.taskContainer.addOpenTask(task);
            return task;
        }
    }
}