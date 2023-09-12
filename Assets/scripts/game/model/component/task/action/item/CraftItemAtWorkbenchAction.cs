using System.Collections.Generic;
using System.Linq;
using game.model.component.building;
using game.model.component.item;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using generation.item;
using Leopotam.Ecs;
using MoreLinq;
using types.material;
using UnityEngine;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action.item {
/**
* Action for crafting item by item order on workbench. Ingredient items will be brought to WB.
* WB should have WorkbenchComponent and ItemContainerComponent.
* TODO Crafting speed is influenced by unit's performance (see {@link stonering.entity.unit.aspects.health.HealthAspect}), and skill.
* TODO add intermediate 'unfinished' item like in RW.
*
* @author Alexander on 06.01.2019.
*/
class CraftItemAtWorkbenchAction : EquipmentAction {
    private readonly CraftingOrder order;
    private readonly EcsEntity workbench;
    private string skill;

    // unit will stand near wb while performing task
    public CraftItemAtWorkbenchAction(CraftingOrder order, EcsEntity workbench) : base(new WorkbenchActionTarget(workbench)) {
        this.order = order;
        this.workbench = workbench;
        name = "crafting " + order.name + " action";

        assignmentCondition = (unit) => OK; // order was checked before task creation
        
        //TODO check ingredients and fuel availability before bringing something to workbench.
        startCondition = () => {
            if (!validateOrder()) return FAIL; // check/find items for order
            order.ingredients.ForEach(ingredientOrder => lockEntities(ingredientOrder.items));
            if (checkBringingItems()) return NEW; // bring ingredient items
            return OK;
        };

        onStart = () => {
            maxProgress = getWorkAmount();
            speed = getSpeed();
        };

        // Creates item, consumes ingredients. Product item is put to Workbench.
        onFinish = () => {
            EcsEntity result = new ItemGenerator().generateItem(order.recipe.newType, selectMaterialForItem(), model.createEntity());
            foreach (EcsEntity item in order.allIngredientItems()) {
                container.stored.removeItemFromContainer(item);
                item.Destroy();
            }
            storeProduct(result);
        };
    }
    
    private bool validateOrder() {
        bool valid = validateOrderItems();
        if (valid) {
            order.allIngredientItems().ForEach(lockEntity);
        } else {
            order.ingredients.ForEach(ingOrder => {
                unlockEntities(ingOrder.items);
                ingOrder.items.Clear();
            });
        }
        return valid;
    }
    
    // checks if all ingredient orders are valid (have correct quantity, types and materials of selected items)
    // clears invalid ingredient orders and unlocks items
    // finds new items for cleared ingredient orders
    // returns true if items for all ingredients found
    private bool validateOrderItems() {
        List<CraftingOrder.IngredientOrder> invalidOrders = container.craftingUtil.findInvalidIngredientOrders(order, target.pos);
        foreach (var ingredientOrder in invalidOrders) {
            ingredientOrder.items.ForEach(unlockEntity);
            ingredientOrder.items.Clear();
        }
        foreach (var ingredientOrder in invalidOrders) {
            List<EcsEntity> items = container.craftingUtil.findItemsForIngredient(ingredientOrder, order, target.pos);
            if (items == null || items.Count != ingredientOrder.ingredient.quantity) return false;
            ingredientOrder.items.AddRange(items);
        }
        return true; // items found for all ingredients
    }

    // if some items not stored in WB, creates bringing sub-actions and returns true.
    private bool checkBringingItems() {
        ItemContainerComponent containerComponent = workbench.take<ItemContainerComponent>();
        bool actionCreated = false;
        order.allIngredientItems()
            .Where(item => !containerComponent.items.Contains(item))
            .ForEach(item => {
                addPreAction(new PutItemToContainerAction(workbench, item));
                actionCreated = true;
            });
        return actionCreated;
    }

    private void storeProduct(EcsEntity item) {
        //TODO put product into WB's bound container
        log("putting item " + item.name() + " to " + workbench.name());
        container.stored.addItemToContainer(item, workbench);
    }

    // in recipe definition, first ingredient will give material for result item
    private int selectMaterialForItem() {
        EcsEntity firstItemOfMainIngredient = order.ingredients[0].items[0];
        return firstItemOfMainIngredient.take<ItemComponent>().material;
    }

    private float getWorkAmount() {
        int totalModifier = 0;
        foreach (var ingredient in order.ingredients) {
            foreach (var item in ingredient.items) {
                totalModifier += MaterialMap.get().material(item.take<ItemComponent>().material).workAmountModifier;
            }
        }
        return order.recipe.workAmount * (1 + totalModifier / 100f);
    }

    private float getSpeed() {
        // TODO use performer skill level, use workbench bonuses(lighting, tool rack), use performer health condition(work slowly when tired). 
        // float performanceBonus = Optional.ofNullable(task.performer.get(HealthAspect.class))
        //         .map(aspect -> aspect.stats.get("performance"l))
        //         .orElse(0f);
        // float skillBonus = Optional.ofNullable(SkillMap.getSkill(this.skill))
        //         .map(skill -> Optional.ofNullable(task.performer.get(JobSkillAspect.class))
        //                 .map(aspect -> aspect.skills.get(this.skill).level())
        //                 .map(level -> level * skill.speed)
        //                 .orElse(0f)).orElse(0f);
        // speed = 1 + performanceBonus + skillBonus;
        return 1f;
    }
}
}