using System.Collections.Generic;
using System.Linq;
using game.model.component.item;
using game.model.component.task.action.target;
using game.model.component.task.order;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder;

// action for crafting with CraftingOrder. Finds and locks ingredient items, generates result item.
namespace game.model.component.task.action.item {
    public abstract class ItemCraftingAction : EquipmentAction {
        public CraftingOrder order;

        protected ItemCraftingAction(CraftingOrder order, ActionTarget target) : base(target) {
            this.order = order;
        }

        // Checks items of all ingredients. If items became invalid, clears them.
        // Then finds items for all cleared ingredients.
        // returns true, if all ingredients have valid items.
        protected bool ingredientOrdersValid() {
            List<IngredientOrder> invalidIngredients = order.ingredients.Where(ingredient => !isIngredientOrderValid(ingredient)).ToList();
            log("invalid ingredients count: " + invalidIngredients.Count);
            invalidIngredients.ForEach(ingredient => clearIngredientItems(ingredient));
            foreach (IngredientOrder ingredient in invalidIngredients) {
                // search for items for ingredient
                if (!findItemsForIngredient(ingredient)) return false;
            }
            return true;
        }

        // Checks that ingredient has correct quantity of items
        //  AND all items have correct itemType and material
        //  AND all items are accessible from performer area
        //  AND not spoiled or destroyed (todo).
        private bool isIngredientOrderValid(IngredientOrder ingredientOrder) {
            return ingredientOrder.items.Count == ingredientOrder.ingredient.quantity
                   && ingredientOrder.items
                       .Select(item => item.take<ItemComponent>())
                       .All(item => ingredientOrder.materials.Contains(item.material) && ingredientOrder.itemTypes.Contains(item.type))
                   && ingredientOrder.items
                       .All(itemEntity => model.itemContainer.itemAccessibleFromPosition(itemEntity, performer.pos()));
        }

        // selects items for ingredientOrder
        private bool findItemsForIngredient(IngredientOrder ingredientOrder) {
            IList<EcsEntity> items = model.itemContainer.craftingUtil.findForIngredientOrder(ingredientOrder, order, performer.pos());
            if (items.Count == ingredientOrder.ingredient.quantity) {
                ingredientOrder.items.AddRange(items);
                return true;
            }
            log(" cant find items for ingredient " + ingredientOrder.ingredient.key);
            return false;
        }

        public void clearOrderItems() => order.ingredients.ForEach(ingOrder => clearIngredientItems(ingOrder));
        
        public void clearIngredientItems(IngredientOrder ingredientOrder) {
            unlockEntities(ingredientOrder.items);
            ingredientOrder.items.Clear();
        }

        private new void log(string message) {
            Debug.Log("[ItemConsumingAction, " + name + "]: " + message);
        }
    }
}