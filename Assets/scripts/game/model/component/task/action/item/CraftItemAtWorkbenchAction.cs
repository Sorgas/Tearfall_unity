using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using generation.item;
using Leopotam.Ecs;
using types.action;
using types.material;
using util.lang.extension;

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
    public CraftItemAtWorkbenchAction(CraftingOrder order, EcsEntity workbench) : base(order,
        new WorkbenchActionTarget(workbench)) {
        this.workbench = workbench;
        name = "crafting " + order.name + " action";

        //TODO check ingredients and fuel availability before bringing something to workbench.
        //TODO add usage of items in nearby containers.
        startCondition = () => {
            if (!checkOrderItems()) return ActionConditionStatusEnum.FAIL; // check/find items for order
            order.ingredients.ForEach(ingredientOrder => lockEntities(ingredientOrder.items));
            if (checkBringingItems()) return ActionConditionStatusEnum.NEW; // bring ingredient items
            return ActionConditionStatusEnum.OK;
        };

        onStart = () => {
            log("start");
            maxProgress = getWorkAmount();
            speed = getSpeed();

        };

        // Creates item, consumes ingredients. Product item is put to Workbench.
        onFinish = () => {
            EcsEntity result = new ItemGenerator().generateItem(order.recipe.newType, selectMaterialForItem(), model.createEntity());
            destroyIngredients();
            storeProduct(result);
        };
    }

    // checks that item is in WB
    private bool checkBringingItems() {
        ItemContainerComponent component = workbench.take<ItemContainerComponent>();
        List<EcsEntity> notInWbItems =
            order.allIngredientItems().Where(item => !component.items.Contains(item)).ToList();
        notInWbItems.ForEach(item => addPreAction(new PutItemToContainerAction(workbench, item))); // create action
        return notInWbItems.Count != 0;
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

    private void destroyIngredients() {
        foreach (EcsEntity item in order.allIngredientItems()) {
            container.stored.removeItemFromContainer(item);
            item.Destroy();
        }
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