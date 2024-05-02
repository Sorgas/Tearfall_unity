using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.task.order;
using generation.item;
using Leopotam.Ecs;
using MoreLinq;
using types.material;
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
class CraftItemAtWorkbenchAction : GenericItemConsumingAction {
    private readonly CraftingOrder order;
    private readonly EcsEntity workbench;
    private string skill;

    // unit will stand near wb while performing task
    public CraftItemAtWorkbenchAction(CraftingOrder order, EcsEntity workbench) : base(new WorkbenchActionTarget(workbench), order, workbench) {
        this.order = order;
        this.workbench = workbench;
        name = "crafting " + order.name + " action";
        usedSkill = order.recipe.skill;
        
        assignmentCondition = (unit) => OK; // order was checked before task creation

        startCondition = () => {
            if (!validateOrder()) return FAIL; // check/find items for order
            order.ingredients.Values.ForEach(ingredientOrder => lockEntities(ingredientOrder.items));
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
            giveExp(order.recipe.exp);
        };
    }

    private void storeProduct(EcsEntity item) {
        //TODO put product into WB's bound container
        log("putting item " + item.name() + " to " + workbench.name());
        container.stored.addItemToContainer(item, workbench);
    }

    // in recipe definition, first ingredient will give material for result item
    private int selectMaterialForItem() {
        EcsEntity firstItemOfMainIngredient = order.ingredients["main"].items[0];
        return firstItemOfMainIngredient.take<ItemComponent>().material;
    }

    private float getWorkAmount() {
        int totalModifier = 0;
        foreach (var ingredient in order.ingredients.Values) {
            foreach (var item in ingredient.items) {
                totalModifier += MaterialMap.get().material(item.take<ItemComponent>().material).workAmountModifier;
            }
        }
        return order.recipe.workAmount * (1 + totalModifier / 100f);
    }

    // TODO use workbench bonuses for workspeed (lighting, tool rack),
}
}