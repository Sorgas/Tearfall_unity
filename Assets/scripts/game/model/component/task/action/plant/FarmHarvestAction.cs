using game.model.component.plant;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.plant {
    // looks for harvestable plants on a farm and creates multiple sub actions.
    public class FarmHarvestAction : Action {
        private EcsEntity farm;

        public FarmHarvestAction(EcsEntity farm) : base(new ZoneActionTarget(farm, ActionTargetTypeEnum.ANY)) {
            this.farm = farm;
            name = "harvest farm " + farm.name();
            startCondition = () => {
                EcsEntity plant = findHarvestablePlant();
                if (plant == EcsEntity.Null) {
                    return ActionConditionStatusEnum.OK;
                }
                lockZoneTile(farm, plant.pos()); // unlocked in plant harvest action
                return addPreAction(new PlantHarvestAction(plant));
            };
        }

        private EcsEntity findHarvestablePlant() {
            ZoneComponent zone = farm.take<ZoneComponent>();
            for (var i = 0; i < zone.tiles.Count; i++) {
                EcsEntity plant = model.plantContainer.getPlant(zone.tiles[i]);
                if (plant != EcsEntity.Null && plant.Has<PlantHarvestableComponent>()) return plant;
            }
            return EcsEntity.Null;
        }
    }
}