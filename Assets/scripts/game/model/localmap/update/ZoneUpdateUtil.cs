using System.Collections.Generic;
using System.Linq;
using game.model.component;
using game.model.component.plant;
using game.model.util;
using Leopotam.Ecs;
using types;
using types.material;
using UnityEngine;
using util.lang.extension;
using static types.ZoneTaskTypes;

namespace game.model.localmap.update {
    public class ZoneUpdateUtil {
        private LocalModel model;
        private readonly List<int> allowedFarmMaterials;

        public ZoneUpdateUtil(LocalModel model) {
            this.model = model;
            allowedFarmMaterials = MaterialMap.get().getByTag("soil").Select(material => material.id).ToList();
        }

        public void updateZone(Vector3Int tile) {
            EcsEntity entity = model.zoneContainer.getZone(tile);
            if (entity == EcsEntity.Null) return;
            ZoneComponent zone = entity.take<ZoneComponent>();
            checkZone(entity, tile);
            if (zone.type == ZoneTypeEnum.FARM) updateFarm(entity, zone, tile);
            if (zone.type == ZoneTypeEnum.STOCKPILE) updateStockpile(entity, tile);
            // entity.Del<ZoneUpdateComponent>();
            entity.Del<TaskCreationTimeoutComponent>(); // check task creation for updated zone
        }

        // removes tile from zone, if tile is not floor
        private void checkZone(EcsEntity zone, Vector3Int tile) {
            if (model.localMap.blockType.get(tile) != BlockTypes.FLOOR.CODE) {
                model.zoneContainer.removeTileFromZone(tile);
            }
        }

        // iterates tiles, put tile to appropriate tracking list
        private void updateFarm(EcsEntity entity, ZoneComponent zone, Vector3Int tile) {
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            if (!zone.tiles.Contains(tile)) return; // tile was removed
            if (!allowedFarmMaterials.Contains(model.localMap.blockType.getMaterial(tile))) {
                model.zoneContainer.removeTileFromZone(tile);
                return; // tile deleted, no more checks
            }
            tracking.removeTile(tile);
            string taskType = getTaskTypeForFarm(entity, tracking, tile);
            if (taskType != null) {
                tracking.tilesToTask.Add(tile, taskType);
            }
        }

        private void updateStockpile(EcsEntity entity, Vector3Int tile) {
            StockpileComponent stockpile = entity.take<StockpileComponent>();
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            tracking.removeTile(tile);
            List<EcsEntity> items = model.itemContainer.onMap.itemsOnMap.get(tile);
            if (items.Count == 0) {
                tracking.tilesToTask.Add(tile, STORE_ITEM);
            } else if (ZoneUtils.allItemsAllowedInStockpile(stockpile, items)) {
                // TODO check stack size
            } else {
                tracking.tilesToTask.Add(tile, REMOVE_ITEM);
            }
        }

        // resolves taskType for farm tile. clean tiles hoed first, then planted. If undesired plant present, it is removed
        // tile is guaranteed to be soil floor
        private string getTaskTypeForFarm(EcsEntity entity, ZoneTrackingComponent tracking, Vector3Int tile) {
            FarmComponent farm = entity.take<FarmComponent>();
            EcsEntity plant = model.plantContainer.getPlant(tile);
            if (plant != EcsEntity.Null && plant.take<PlantComponent>().type.name != farm.plant)
                return REMOVE_PLANT; // undesired plant
            if (!model.farmContainer.isFarm(tile)) return HOE; // not hoed
            if (plant == EcsEntity.Null && farm.plant != null) return PLANT; // already hoed and no plant
            if (plant != EcsEntity.Null && plant.Has<PlantHarvestableComponent>()) return HARVEST; // plant ready for harvest
            return null; // desired plant
        }
    }
}