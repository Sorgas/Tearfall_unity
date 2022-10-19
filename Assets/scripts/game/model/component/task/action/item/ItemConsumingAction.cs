using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using game.model.component.task.action;
using game.model.component.task.action.target;
using Leopotam.Ecs;
using util.lang.extension;
using static CraftingOrder;
/**
* Superclass for {@link CraftItemAction} and {@link BuildingAction}.
* Provides methods for searching for and locking ingredient items.
* During items selection uses {@link ItemConsumingAction#getPositionForItems} to check availability.
*
* @author Alexander on 05.05.2020
*/
class ItemConsumingAction : ItemAction {
    public CraftingOrder order;

    protected ItemConsumingAction(CraftingOrder order, ActionTarget target) : base(target) {
        this.order = order;
    }

    /**
     * Checks items of all ingredients. If items became invalid, clears them.
     * Then, finds items for all cleared ingredients.
     *
     * @return true, if all ingredients have valid items.
     */
    protected bool ingredientOrdersValid() {
        List<IngredientOrder> ingredients = order.ingredients;
        if(ingredients.Count == 0) return false;
        
        List<IngredientOrder> invalidIngredients = ingredients.Where(ingredient => !isIngredientOrderValid(ingredient)).ToList(); 
        foreach(IngredientOrder ingredient in invalidIngredients) {
            clearIngredientItems(ingredient);
        }
        foreach(IngredientOrder ingredient in invalidIngredients) {
            if (!findItemsForIngredient(ingredient)) { // search for items for ingredient
                return false; 
            }
        }
        return true;
    }

    /**
     * Checks that ingredient has correct quantity of items and all items are accessible and not spoiled, destroyed or locked.
     */
    private bool isIngredientOrderValid(IngredientOrder ingredientOrder) {
        return ingredientOrder.items.Count == order.recipe.ingredients[ingredientOrder.key].quantity
                && ingredientOrder.items.All(itemEntity => model.itemContainer.itemAccessible(itemEntity, performer.pos()));
    }

    // selects items for ingredientOrder
    private bool findItemsForIngredient(IngredientOrder ingredientOrder) {
        List<EcsEntity> items = model.itemContainer.util.findForCraftingOrderIngredient(ingredientOrder, order, performer.pos());
        if(items.Count == order.recipe.ingredients[ingredientOrder.key].quantity) {
            ingredientOrder.items.AddRange(items);
            return true;
        }        
        return false;
    }

    protected void consumeItems() {
        foreach (IngredientOrder ingredientOrder in order.ingredients) {
            foreach (EcsEntity item in ingredientOrder.items) {
                // container.removeItem();
            }
        }
   }

    public void clearOrderItems() {
        foreach (IngredientOrder ingredientOrder in order.ingredients) {
            clearIngredientItems(ingredientOrder);
        }
    }

    public void clearIngredientItems(IngredientOrder ingredientOrder) {
        setItemsLocked(ingredientOrder, false);
        ingredientOrder.items.Clear();
    }

    public void setItemsLocked(IngredientOrder ingredientOrder, bool value) {
        // TODO reference to task?
        if(value) {
            ingredientOrder.items.ForEach(item => item.Replace<ItemLockedComponent>(new ItemLockedComponent()));
        } else {
            ingredientOrder.items.ForEach(item => item.Del<ItemLockedComponent>());
        }
    }
}