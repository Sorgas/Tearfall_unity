using game.model.component.task.action.target;
using game.model.component.task.order;
using UnityEngine;
using util.lang.extension;

// action for crafting with CraftingOrder. Finds and locks ingredient items, generates result item.
namespace game.model.component.task.action.item {
public abstract class ItemCraftingAction : EquipmentAction {
    protected readonly CraftingOrder order;

    protected ItemCraftingAction(CraftingOrder order, ActionTarget target) : base(target) {
        this.order = order;
    }

    protected bool checkOrderItems() {
        // TODO use workbench access position
        bool found = container.craftingUtil.findItemsForOrder(order, performer.pos(), unlockEntity);
        if (found) { // re-lock all items in order to lock newly added items
            order.allIngredientItems().ForEach(lockEntity);
        } else { // unlock all items of failed order
            clearOrderItems();
        }
        return found;
    }

    private void clearOrderItems() {
        order.ingredients.ForEach(ingOrder => {
            unlockEntities(ingOrder.items);
            ingOrder.items.Clear();
        });
    }

    private new void log(string message) {
        Debug.Log("[ItemConsumingAction, " + name + "]: " + message);
    }
}
}