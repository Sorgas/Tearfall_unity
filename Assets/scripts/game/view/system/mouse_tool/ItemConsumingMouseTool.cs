using game.model.component.task.order;
using generation.item;
using UnityEngine;

namespace game.view.system.mouse_tool {
// Superclass for tools that require selection of items (building)
    public abstract class ItemConsumingMouseTool : MouseTool {
        protected bool hasMaterials; // TODO use to check before applying tool and for sprite tint
        protected CraftingOrderGenerator generator = new();

        protected void fillSelector(AbstractItemConsumingOrder order) {
            // TODO select first item type ( wood log)
            hasMaterials = materialSelector.fillForOrder(order);
            materialSelector.open();
            if(!hasMaterials) Debug.LogWarning("materials for construction not found.");
        }
    }
}