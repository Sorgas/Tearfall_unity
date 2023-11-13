using System.Collections.Generic;
using System.Linq;
using Leopotam.Ecs;
using types.item.recipe;

// order for crafting items in workbenches. Order is 'completed' when performed targetQuantity number of times.
namespace game.model.component.task.order {
public class CraftingOrder : AbstractItemConsumingOrder {
    public Recipe recipe;
    public CraftingOrderStatus status;
    public bool repeated;
    public int targetQuantity;
    public int performedQuantity;

    public CraftingOrder(Recipe recipe) {
        name = recipe.title;
        this.recipe = recipe;
        status = CraftingOrderStatus.WAITING;
        targetQuantity = 1;
        performedQuantity = 0;
    }

    public CraftingOrder(CraftingOrder source) : base(source) {
        recipe = source.recipe;
        status = source.status;
        repeated = source.repeated;
        targetQuantity = source.targetQuantity;
        performedQuantity = source.performedQuantity;
    }
    
    public enum CraftingOrderStatus {
        PERFORMING, // A, order has task
        WAITING, // W, order has no task
        PAUSED, // P, order paused by player
        // PAUSED_PROBLEM // PP, order paused by failed task (similar to PAUSED)
    }
}
}