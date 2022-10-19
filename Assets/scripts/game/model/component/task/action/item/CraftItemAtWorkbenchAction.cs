using System.Collections.Generic;
using System.Linq;
using enums.action;
using game.model.component.building;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using util.lang.extension;
using static game.model.component.task.TaskComponents;

/**
* Action for crafting item by item order on workbench. Ingredient items will be brought to WB.
* WB should have {@link WorkbenchAspect} and {@link ItemContainerAspect}.
* Crafting speed is influenced by unit's performance (see {@link stonering.entity.unit.aspects.health.HealthAspect}), and skill.
* TODO add intermediate 'unfinished' item like in RW.
*
* @author Alexander on 06.01.2019.
*/
class CraftItemAtWorkbenchAction : ItemConsumingAction {
    private CraftingOrder itemOrder;
    private EcsEntity workbench;
    private string skill;

    // unit will stand near wb while performing task
    public CraftItemAtWorkbenchAction(CraftingOrder order, EcsEntity workbench) : base(order, new EntityActionTarget(workbench, ActionTargetTypeEnum.NEAR)) {
        this.itemOrder = order;
        this.workbench = workbench;
        // ItemContainerAspect containerAspect = workbench.get(ItemContainerAspect.class);
        // WorkbenchAspect workbenchAspect = workbench.get(WorkbenchAspect.class);
        // FuelConsumerAspect fuelAspect = workbench.get(FuelConsumerAspect.class);
        
        //TODO check ingredients and fuel availability before bringing something to workbench.
        //TODO add usage of items in nearby containers.
        startCondition = () => {

            if (!ingredientOrdersValid()) return ActionConditionStatusEnum.FAIL; // check/find items for order
            if (checkBringingItems()) return ActionConditionStatusEnum.NEW; // bring ingredient items
            return ActionConditionStatusEnum.OK;
        };

        // onStart = () -> {
        //     System.out.println("start action craft");
        //     maxProgress = itemOrder.recipe.workAmount * (1 + getMaterialWorkAmountMultiplier());
        //     float performanceBonus = Optional.ofNullable(task.performer.get(HealthAspect.class))
        //             .map(aspect -> aspect.stats.get("performance"))
        //             .orElse(0f);
        //     float skillBonus = Optional.ofNullable(SkillMap.getSkill(this.skill))
        //             .map(skill -> Optional.ofNullable(task.performer.get(JobSkillAspect.class))
        //                     .map(aspect -> aspect.skills.get(this.skill).level())
        //                     .map(level -> level * skill.speed)
        //                     .orElse(0f)).orElse(0f);
        //     //TODO add WB tier bonus
        //     speed = 1 + performanceBonus + skillBonus;
        // };

        // // Creates item, consumes ingredients. Product item is put to Workbench.
        // onFinish = () -> {
        //     System.out.println("finish action craft");
        //     Item product = new ItemGenerator().generateItemByOrder(itemOrder);
        //     // spend components
        //     List<Item> items = itemOrder.allIngredients().stream()
        //             .map(ingredientOrder -> ingredientOrder.items)
        //             .flatMap(Collection::stream)
        //             .collect(Collectors.toList());
        //     itemContainer.removeItems(items);
        //     storeProduct(product);
        // };
    }

    // checks that item is in WB
    private bool checkBringingItems() {
        BuildingItemContainerComponent component = workbench.take<BuildingItemContainerComponent>();
        List<EcsEntity> notInWbItems = order.allIngredientItems().Where(item => !component.items.Contains(item)).ToList();
        notInWbItems.ForEach(item => task.take<TaskActionsComponent>().addFirstPreAction(new PutItemToContainerAction(workbench, item))); // create action
        return notInWbItems.Count != 0;
    }

    private void storeProduct(EcsEntity item) {
        //TODO put product into WB's bound container
        workbench.takeRef<BuildingItemContainerComponent>().items.Add(item);
        container.stored.addItemToContainer(item, workbench);
    }

    public string toString() {
        return "Crafting action: " + itemOrder.name;
    }
}