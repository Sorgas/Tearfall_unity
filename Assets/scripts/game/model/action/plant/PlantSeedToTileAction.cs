using game.model.component;
using game.model.component.task.action;
using game.model.component.task.action.target;
using generation.plant;
using Leopotam.Ecs;
using types.action;
using UnityEngine;
using util.item;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.action.plant {
    // action for planting single tile. created from FarmPlantingAction. 
    // looks for seed item, grabs whole stack of seeds, plants seed.
    // Remaining seeds will be planted from another actions.
    public class PlantSeedToTileAction : EquipmentAction {
        private SeedItemSelector seedSelector;
        private EcsEntity zone;
        private string plantName;
        
        public PlantSeedToTileAction(Vector3Int tile, EcsEntity zone) : base(new PositionActionTarget(tile, ActionTargetTypeEnum.ANY)) {
            name = "plant farm tile";
            this.zone = zone;
            seedSelector = new SeedItemSelector(zone.take<FarmComponent>().plant);
            plantName = zone.take<FarmComponent>().plant;
            maxProgress = 100;
            startCondition = () => {

                // if (seedSelector.checkItem(equipment().hauledItem)) return OK;
                // EcsEntity seedItem = model.itemContainer.util.findFreeReachableItemBySelector(seedSelector, performer.pos());
                // if (seedItem != EcsEntity.Null) {
                //     return addPreAction(new ObtainItemAction(seedItem));
                // }
                // return FAIL;
                lockZoneTile(zone, tile);
                return OK;
            };

            onFinish = () => {
                createPlant();
                unlockZoneTile(zone, tile);
                // Debug.Log("action " + name + " finished " + tile);
            };
        }

        private void createPlant() {
            EcsEntity plant = new PlantGenerator().generate(plantName, model.createEntity());
            model.plantContainer.addPlant(plant, target.pos);
        }
    }
}