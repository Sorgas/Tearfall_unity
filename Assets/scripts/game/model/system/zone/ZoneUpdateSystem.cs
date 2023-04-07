using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.plant;
using game.model.localmap;
using game.model.util;
using Leopotam.Ecs;
using types;
using types.material;
using UnityEngine;
using util.lang.extension;
using EcsEntity = Leopotam.Ecs.EcsEntity;

namespace game.model.system.zone {
    // recounts tile tracked state for updated zones.
    // TODO add update zone component when plant or item is put on farm, 
    public class ZoneUpdateSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<ZoneUpdatedComponent> filter;
        private readonly List<int> allowedFarmMaterials;

        public ZoneUpdateSystem() {
            allowedFarmMaterials = MaterialMap.get().getByTag("soil").Select(material => material.id).ToList();
        }

        public override void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                ZoneUpdatedComponent updatedComponent = filter.Get1(i);
                updateZone(entity, updatedComponent);
                if (zone.type == ZoneTypeEnum.FARM) updateFarm(entity, zone, updatedComponent);
                if (zone.type == ZoneTypeEnum.STOCKPILE) updateStockpile(entity, updatedComponent);
                entity.Del<ZoneUpdatedComponent>();
                entity.Del<TaskCreationTimeoutComponent>(); // check task creation for updated zone
            }
        }

        // removes tile from zone, if tile is not floor
        private void updateZone(EcsEntity zone, ZoneUpdatedComponent updated) {
            foreach (Vector3Int tile in updated.tiles) {
                if (model.localMap.blockType.get(tile) != BlockTypes.FLOOR.CODE) {
                    model.zoneContainer.removeTileFromZone(tile);
                }
            }
        }

        // iterates tiles, put tile to appropriate tracking list
        private void updateFarm(EcsEntity entity, ZoneComponent zone, ZoneUpdatedComponent updated) {
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            foreach (Vector3Int tile in updated.tiles) {
                if (!allowedFarmMaterials.Contains(model.localMap.blockType.getMaterial(tile))) {
                    model.zoneContainer.removeTileFromZone(tile);
                    continue; // tile deleted, no more checks
                }
                foreach (string taskType in ZoneTaskTypes.FARM_TASKS) {
                    tracking.tiles[taskType].Remove(tile);
                }
                string taskType1 = getTaskTypeForFarm(entity, tracking, tile);
                if (taskType1 != null) tracking.tiles[taskType1].Add(tile);
            }
            Debug.Log("farm updated");
        }
        
        private void updateStockpile(EcsEntity entity, ZoneUpdatedComponent updated) {
            StockpileComponent stockpile = entity.take<StockpileComponent>();
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            foreach (Vector3Int tile in updated.tiles) {
                foreach (string taskType in ZoneTaskTypes.STOCKPILE_TASKS) {
                    tracking.tiles[taskType].Remove(tile);
                }
                List<EcsEntity> items = model.itemContainer.onMap.itemsOnMap.get(tile);
                if (items.Count == 0) {
                    tracking.tiles[ZoneTaskTypes.STORE_ITEM].Add(tile);
                } else if (ZoneUtils.allItemsAllowedInStockpile(stockpile, items)) {
                    // check stack size
                } else {
                    tracking.tiles[ZoneTaskTypes.REMOVE_ITEM].Add(tile);
                }
            }
        }

        // resolves taskType for farm tile. clean tiles hoed first, then planted. If undesired plant present, it is removed
        // tile is guaranteed to be soil floor
        private string getTaskTypeForFarm(EcsEntity entity, ZoneTrackingComponent tracking, Vector3Int tile) {
            FarmComponent farm = entity.take<FarmComponent>();
            // byte blockType = model.localMap.blockType.get(tile);
            EcsEntity plant = model.plantContainer.getPlant(tile);
            if (plant == EcsEntity.Null) {
                if (model.farmContainer.isFarm(tile)) return ZoneTaskTypes.PLANT; // already hoed and no plant
                return ZoneTaskTypes.HOE; // not hoed
            }
            if (farm.plant != plant.take<PlantComponent>().type.name) return ZoneTaskTypes.REMOVE_PLANT; // undesired plant
            return null; // desired plant
        }
    }
}