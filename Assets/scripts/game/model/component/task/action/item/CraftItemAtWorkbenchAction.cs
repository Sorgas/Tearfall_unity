using System.Collections.Generic;
using System.Linq;
using game.model.component.building;
using game.model.component.item;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using generation.item;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder;

namespace game.model.component.task.action.item {
    /**
* Action for crafting item by item order on workbench. Ingredient items will be brought to WB.
* WB should have {@link WorkbenchAspect} and {@link ItemContainerAspect}.
* Crafting speed is influenced by unit's performance (see {@link stonering.entity.unit.aspects.health.HealthAspect}), and skill.
* TODO add intermediate 'unfinished' item like in RW.
*
* @author Alexander on 06.01.2019.
*/
    class CraftItemAtWorkbenchAction : ItemCraftingAction {
        private EcsEntity workbench;
        private string skill;

        // unit will stand near wb while performing task
        public CraftItemAtWorkbenchAction(CraftingOrder order, EcsEntity workbench) : base(order, new WorkbenchActionTarget(workbench)) {
            this.workbench = workbench;
            this.name = "crafting " + order.name + " action";
        
            //TODO check ingredients and fuel availability before bringing something to workbench.
            //TODO add usage of items in nearby containers.
            startCondition = () => {
                if (!ingredientOrdersValid()) return ActionConditionStatusEnum.FAIL; // check/find items for order
                order.ingredients.ForEach(ingredientOrder => lockEntities(ingredientOrder.items));
                if (checkBringingItems()) return ActionConditionStatusEnum.NEW; // bring ingredient items
                return ActionConditionStatusEnum.OK;
            };

            onStart = () => {
                log("start");
                // maxProgress = itemOrder.recipe.workAmount * (1 + getMaterialWorkAmountMultiplier());
                // float performanceBonus = Optional.ofNullable(task.performer.get(HealthAspect.class))
                //         .map(aspect -> aspect.stats.get("performance"l))
                //         .orElse(0f);
                // float skillBonus = Optional.ofNullable(SkillMap.getSkill(this.skill))
                //         .map(skill -> Optional.ofNullable(task.performer.get(JobSkillAspect.class))
                //                 .map(aspect -> aspect.skills.get(this.skill).level())
                //                 .map(level -> level * skill.speed)
                //                 .orElse(0f)).orElse(0f);
                // //TODO add WB tier bonus
                // speed = 1 + performanceBonus + skillBonus;
            };

            // Creates item, consumes ingredients. Product item is put to Workbench.
            onFinish = () => {
                EcsEntity result = new ItemGenerator().generateItem(order.recipe.newType, selectMaterialForItem(), model.createEntity());
                ref BuildingItemContainerComponent containerComponent = ref workbench.takeRef<BuildingItemContainerComponent>();
                foreach(EcsEntity item in order.allIngredientItems()) {
                    container.stored.removeItemFromContainer(item);
                    containerComponent.items.Remove(item);
                    item.Destroy();
                }
                storeProduct(result);
            };
        }

        // checks that item is in WB
        private bool checkBringingItems() {
            BuildingItemContainerComponent component = workbench.take<BuildingItemContainerComponent>();
            List<EcsEntity> notInWbItems = order.allIngredientItems().Where(item => !component.items.Contains(item)).ToList();
            notInWbItems.ForEach(item => addPreAction(new PutItemToContainerAction(workbench, item))); // create action
            return notInWbItems.Count != 0;
        }

        private void storeProduct(EcsEntity item) {
            //TODO put product into WB's bound container
            log("putting item " + item.name() + " to " + workbench.name());
            workbench.takeRef<BuildingItemContainerComponent>().items.Add(item);
            container.stored.addItemToContainer(item, workbench);
        }

        private int selectMaterialForItem() {
            IngredientOrder ingredientOrder = order.ingredients.Count == 1 
                ? order.ingredients[0] 
                : order.ingredients.Where(ingredientOrder => ingredientOrder.key == "main").First();
            EcsEntity firstItemOfMainIngredient = ingredientOrder.items[0];
            return firstItemOfMainIngredient.take<ItemComponent>().material;
        }   
    }
}