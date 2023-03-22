using System.Collections.Generic;
using game.model.component;
using game.model.component.plant;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.zone {
    // recounts tile tracked state for updated zones.
    // TODO add update zone component when plant is item is put on farm, 
    public class ZoneUpdateSystem : LocalModelEcsSystem {
        public EcsFilter<ZoneUpdatedComponent> filter;

        public ZoneUpdateSystem(LocalModel model) : base(model) { }

        public override void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ZoneComponent zone = entity.take<ZoneComponent>();
                if (zone.type == ZoneTypeEnum.FARM) {
                    updateFarm(entity, zone, filter.Get1(i));
                }
                // TODO same for stockpiles
                entity.Del<ZoneUpdatedComponent>();
            }
        }

        // iterates tiles, put tile to appropriate tracking list
        private void updateFarm(EcsEntity entity, ZoneComponent zone, ZoneUpdatedComponent updated) {
            FarmTileTrackingComponent tracking = entity.take<FarmTileTrackingComponent>();
            foreach (Vector3Int tile in updated.tiles) {
                tracking.toHoe.Remove(tile);
                tracking.toPlant.Remove(tile);
                tracking.toRemove.Remove(tile);
                getListForTile(entity, tracking, tile)?.Add(tile);
            }
            Debug.Log("farm updated");
        }

        // finds list to put tile
        private List<Vector3Int> getListForTile(EcsEntity entity, FarmTileTrackingComponent tracking, Vector3Int tile) {
            FarmComponent farm = entity.take<FarmComponent>();
            byte blockType = model.localMap.blockType.get(tile);
            if (blockType != BlockTypes.FARM.CODE) {
                return tracking.toHoe;
            }
            EcsEntity plant = model.plantContainer.getPlant(tile);
            if (plant == EcsEntity.Null && blockType == BlockTypes.FARM.CODE) {
                return tracking.toPlant;
            }
            string name = plant.take<PlantComponent>().type.name;
            if (!farm.config.Contains(name)) {
                return tracking.toRemove;
            }
            return null;
        } 
    }
}