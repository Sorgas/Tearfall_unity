using game.model.component.plant;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.plant {
    public class FarmHarvestAction : Action {
        private EcsEntity farm;

        public FarmHarvestAction(EcsEntity farm) : base(new ZoneActionTarget(farm, ActionTargetTypeEnum.ANY)) {
            this.farm = farm;
            startCondition = () => {
                EcsEntity plant = findHarvestablePlant();
                if (plant == EcsEntity.Null) {
                    return ActionConditionStatusEnum.OK;
                }
                lockZoneTile(farm, plant.pos());
                return addPreAction(new PlantHarvestAction(plant));
            };
        }

        private EcsEntity findHarvestablePlant() {
            ZoneComponent zone = farm.take<ZoneComponent>();
            for (var i = 0; i < zone.tiles.Count; i++) {
                Vector3Int tile = zone.tiles[i];
                EcsEntity plant = model.plantContainer.getPlant(tile);
                if (plant != EcsEntity.Null) {
                    if (plant.Has<PlantHarvestableComponent>()) {
                        return plant;
                    }
                }
            }
            return EcsEntity.Null;
        }
    }
}