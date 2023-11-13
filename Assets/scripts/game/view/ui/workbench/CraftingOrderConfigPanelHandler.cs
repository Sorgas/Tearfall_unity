using System.Collections.Generic;
using game.model.component.task.order;
using game.view.util;
using UnityEngine;

namespace game.view.ui.workbench {
// Creates columns of ingredient configuration for each ingredient order
public class CraftingOrderConfigPanelHandler : MonoBehaviour {
    private List<CraftingOrderConfigPanelIngredientHandler> columns = new();

    public void fillFor(AbstractItemConsumingOrder order) {
        Debug.Log("filling crafting order config");
        int maxHeight = 0;
        foreach (var handler in columns) {
            Destroy(handler.gameObject);
        }
        columns.Clear();
        foreach (IngredientOrder ingredientOrder in order.ingredients.Values) {
            GameObject go = PrefabLoader.create("IngredientOrderPanel", gameObject.transform, new Vector3(columns.Count * 200, 0, 0));
            CraftingOrderConfigPanelIngredientHandler handler = go.GetComponent<CraftingOrderConfigPanelIngredientHandler>();
            columns.Add(handler);
            handler.fillFor(ingredientOrder.ingredients[0].key, ingredientOrder);
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(columns.Count * 200, 200);
    }
}
}