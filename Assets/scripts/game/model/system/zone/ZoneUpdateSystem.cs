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
using EcsEntity = Leopotam.Ecs.EcsEntity;

namespace game.model.system.zone {
    // recounts tile tracked state for updated zones.
    // TODO add update zone component when plant or item is put on farm, 
    public class ZoneUpdateSystem : LocalModelUnscalableEcsSystem {
        public EcsFilter<ZoneUpdateComponent> filter;
        private readonly List<int> allowedFarmMaterials;
        private bool debug = true;
        
        public ZoneUpdateSystem() {
            allowedFarmMaterials = MaterialMap.get().getByTag("soil").Select(material => material.id).ToList();
        }

        public override void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                ZoneUpdateComponent updateComponent = filter.Get1(i);
                updateZone(entity, updateComponent);
                if (zone.type == ZoneTypeEnum.FARM) updateFarm(entity, zone, updateComponent);
                if (zone.type == ZoneTypeEnum.STOCKPILE) updateStockpile(entity, updateComponent);
                log("tiles updated " + updateComponent.tiles.Count);
                entity.Del<ZoneUpdateComponent>();
                entity.Del<TaskCreationTimeoutComponent>(); // check task creation for updated zone
            }
        }

        // removes tile from zone, if tile is not floor
        private void updateZone(EcsEntity zone, ZoneUpdateComponent update) {
            foreach (Vector3Int tile in update.tiles) {
                if (model.localMap.blockType.get(tile) != BlockTypes.FLOOR.CODE) {
                    model.zoneContainer.removeTileFromZone(tile);
                }
            }
        }

        // iterates tiles, put tile to appropriate tracking list
        private void updateFarm(EcsEntity entity, ZoneComponent zone, ZoneUpdateComponent update) {
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            foreach (Vector3Int tile in update.tiles) {
                if (!zone.tiles.Contains(tile)) continue; // tile was removed
                if (!allowedFarmMaterials.Contains(model.localMap.blockType.getMaterial(tile))) {
                    model.zoneContainer.removeTileFromZone(tile);
                    continue; // tile deleted, no more checks
                }
                tracking.removeTile(tile);
                string taskType = getTaskTypeForFarm(entity, tracking, tile);
                Debug.Log(taskType);
                if (taskType != null) tracking.tiles[taskType].Add(tile);
            }
        }
        
        private void updateStockpile(EcsEntity entity, ZoneUpdateComponent update) {
            StockpileComponent stockpile = entity.take<StockpileComponent>();
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
            foreach (Vector3Int tile in update.tiles) {
                tracking.removeTile(tile);
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
            log(plant.ToString());
            if (plant == EcsEntity.Null) {
                if (model.farmContainer.isFarm(tile)) return ZoneTaskTypes.PLANT; // already hoed and no plant
                return ZoneTaskTypes.HOE; // not hoed
            }
            if (farm.plant != plant.take<PlantComponent>().type.name) return ZoneTaskTypes.REMOVE_PLANT; // undesired plant
            if (plant.Has<PlantHarvestableComponent>()) return ZoneTaskTypes.HARVEST; // plant ready for harvest
            return null; // desired plant
        }

        private void log(string message) {
            if(debug) Debug.Log("[ZoneUpdateSystem] " + message);
        }
    }
}