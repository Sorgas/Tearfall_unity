using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using Leopotam.Ecs;
using MoreLinq;
using util.lang.extension;

namespace game.model.component.task.action {
// Base class for building, constructing and crafting actions.
// Provides methods to check items for order and find new ones.
public abstract class GenericItemConsumingAction : EquipmentAction {
    protected readonly AbstractItemConsumingOrder order;

    // first place to look for new items. nullable
    protected readonly EcsEntity containerEntity;

    public GenericItemConsumingAction(ActionTarget target, AbstractItemConsumingOrder order) : this(target, order, EcsEntity.Null) { }

    public GenericItemConsumingAction(ActionTarget target, AbstractItemConsumingOrder order, EcsEntity containerEntity) : base(target) {
        this.order = order;
        this.containerEntity = containerEntity;
        debug = true;
    }

    // Checks if order is valid, then re-locks or unlocks all items.
    protected bool validateOrder() {
        bool valid = validateOrderItems();
        if (valid) {
            order.ingredients.Values.ForEach(ingredientOrder => lockEntities(ingredientOrder.items));
        } else {
            order.ingredients.Values.ForEach(clearIngredientOrder);
        }
        return valid;
    }

    // Checks if all ingredient orders are valid (have correct quantity, types and materials of selected items)
    // clears invalid ingredient orders and finds new items
    // returns true if items for all ingredients found
    private bool validateOrderItems() {
        List<IngredientOrder> invalidOrders = container.craftingUtil.findInvalidIngredientOrders(order, target.pos, containerEntity);
        int invalidOrdersCount = invalidOrders.Count;
        invalidOrders.ForEach(clearIngredientOrder);
        foreach (var ingredientOrder in invalidOrders) {
            List<EcsEntity> items = container.craftingUtil.findItemsForIngredient(ingredientOrder, order, target.pos);
            if (items == null) {
                logDebug($"Validating order: INVALID, invalid ingredient orders: {invalidOrdersCount}, can't find items for ingredient order {ingredientOrder.key}");
                return false;
            }
            ingredientOrder.items.AddRange(items);
        }
        logDebug($"Validating order: VALID, invalid ingredient orders: {invalidOrdersCount}");
        return true; // items found for all ingredients
    }

    private void clearIngredientOrder(IngredientOrder ingredientOrder) {
        unlockEntities(ingredientOrder.items);
        ingredientOrder.items.Clear();
    }
    
    // Checks that all items selected for order are stored in container, creates bringing actions for those which are not.
    protected  bool checkBringingItems() {
        ItemContainerComponent containerComponent = containerEntity.take<ItemContainerComponent>();
        bool actionCreated = false;
        order.allIngredientItems()
            .Where(item => !containerComponent.items.Contains(item))
            .ForEach(item => {
                addPreAction(new PutItemToContainerAction(containerEntity, item));
                actionCreated = true;
            });
        return actionCreated;
    }
}
}