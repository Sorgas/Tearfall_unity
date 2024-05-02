using game.model.component.plant;
using game.model.component.task.action.target;
using game.model.component.unit;
using generation.item;
using Leopotam.Ecs;
using types.action;
using types.unit.skill;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action.plant {
    // harvests product items from harvestable plant.
    // locks and unlocks zone tile if plant is within zone.
    // drops items to ground
    public class PlantHarvestAction : Action {
        private EcsEntity plant;
        private Vector3Int plantPosition;
        private EcsEntity zone;
        
        public PlantHarvestAction(EcsEntity plant) : base(new PlantActionTarget(plant)) {
            this.plant = plant;
            maxProgress = 100;
            plantPosition = plant.pos();
            name = "harvest plant " + plantPosition;
            usedSkill = model.zoneContainer.getZone(plantPosition) != EcsEntity.Null
                    ? UnitSkills.FARMING.name
                    : UnitSkills.FORAGING.name;
            
            assignmentCondition = (unit) => ActionCheckingEnum.OK;
            
            startCondition = () => {
                zone = model.zoneContainer.getZone(plantPosition);
                if (!plant.IsAlive()) return ActionCheckingEnum.FAIL;
                if (!plant.Has<PlantHarvestableComponent>()) return ActionCheckingEnum.FAIL;
                if (zone != EcsEntity.Null) lockZoneTile(zone, plantPosition);
                return ActionCheckingEnum.OK;
            };
            
            onStart = () => {
                speed = getSpeed();
            };

            // TODO add chance to fail based on skill and plant rarity. Change exp amount based on plant rarity
            onFinish = () => {
                zone = model.zoneContainer.getZone(plantPosition);
                plant.Del<PlantHarvestableComponent>();
                plant.Replace(new PlantHarvestedComponent());
                if (zone != EcsEntity.Null) unlockZoneTile(zone, plantPosition);
                createItems();
                giveExp(5);
            };
        }

        // items created here, because quantity depends on performer's skill
        private void createItems() {
            ItemGenerator generator = new();
            for (int i = 0; i < plant.take<PlantComponent>().type.product.productCount; i++) {
                EcsEntity item = generator.generatePlantProduct(plant, model.createEntity());
                model.itemContainer.onMap.putItemToMap(item, plantPosition);
            }
        }
    }
}