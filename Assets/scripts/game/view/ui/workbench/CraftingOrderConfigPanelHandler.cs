using System.Collections.Generic;
using game.model.component.task.order;
using game.view.util;
using UnityEngine;
using static game.model.component.task.order.CraftingOrder;

namespace game.view.ui.workbench {
// Creates columns of ingredient configuration for each ingredient order
public class CraftingOrderConfigPanelHandler : MonoBehaviour {
    // ingredientOrder -> itemType -> material/any
    // private Dictionary<IngredientOrder, Dictionary<string, Dictionary<int, GameObject>>> buttons = new();
    public CraftingOrderLineHandler parent;
    private const int ButtonWidth = 200;
    private const int ButtonHeight = 40;
    public int buttonPadding = 5;
    private List<CraftingOrderConfigPanelIngredientHandler> columns = new();

    public void fillFor(CraftingOrder order) {
        Debug.Log("filling crafting order config");
        int maxHeight = 0;
        foreach (var handler in columns) {
            Destroy(handler.gameObject);
        }
        columns.Clear();
        foreach (IngredientOrder ingredientOrder in order.ingredients) {
            GameObject go = PrefabLoader.create("IngredientOrderPanel", gameObject.transform, new Vector3(columns.Count * 200, 0, 0));
            CraftingOrderConfigPanelIngredientHandler handler = go.GetComponent<CraftingOrderConfigPanelIngredientHandler>();
            columns.Add(handler);
            handler.fillFor(ingredientOrder.ingredients[0].key, ingredientOrder);
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(columns.Count * 200, 200);
    }

    // private void createButton(int x, int y, IngredientOrder ingredientOrder, ItemType itemType, int materialId) {
    //     GameObject button = PrefabLoader.create("MaterialSelectionButton", gameObject.transform,
    //         new Vector3(
    //             buttonPadding + x * (ButtonWidth + buttonPadding),
    //             buttonPadding + y * (ButtonHeight + buttonPadding), 0));
    //     Image iconImage = button.GetComponentsInChildren<Image>()[1];
    //     iconImage.sprite = ItemTypeMap.get().getSprite(itemType.name); // item sprite
    //     TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
    //     if (materialId != -1) {
    //         Material_ material = MaterialMap.get().material(materialId);
    //         iconImage.color = material.color; // item color
    //         text.text = material.name + " " + itemType.title;
    //     } else {
    //         text.text = "Any " + itemType.title;
    //         iconImage.color = Color.white;
    //     }
    //     button.GetComponentInChildren<Button>().onClick.AddListener(() => {
    //         toggleMaterialForIngredientOrder(ingredientOrder, itemType.name, materialId);
    //         updateButtonsColor(ingredientOrder, itemType.name);
    //     });
    //     buttons[ingredientOrder][itemType.name].Add(materialId, button);
    // }
    //
    // private void toggleMaterialForIngredientOrder(IngredientOrder ingredientOrder, string itemType, int materialId) {
    //     int numberOfMaterials = buttons[ingredientOrder][itemType].Count - 1;
    //     if (materialId == -1) { // any material button
    //         if (ingredientOrder.materials.Count < numberOfMaterials) {
    //             buttons[ingredientOrder][itemType].Keys
    //                 .Where(material => material != -1)
    //                 .ForEach(material => ingredientOrder.materials.Add(material));
    //         } else {
    //             ingredientOrder.materials.Clear();
    //         }
    //     } else { // single material button
    //         if (ingredientOrder.materials.Contains(materialId)) {
    //             ingredientOrder.materials.Remove(materialId);
    //         } else {
    //             ingredientOrder.materials.Add(materialId);
    //         }
    //     }
    // }
    //
    // private void updateButtonsColor(IngredientOrder ingredientOrder, string itemType) {
    //     foreach (KeyValuePair<int, GameObject> pair in buttons[ingredientOrder][itemType]) {
    //         Image buttonImage = pair.Value.GetComponentInChildren<Image>();
    //         bool buttonEnabled = pair.Key != -1
    //             ? ingredientOrder.materials.Contains(pair.Key)
    //             : ingredientOrder.materials.Count == buttons[ingredientOrder][itemType].Count - 1;
    //         buttonImage.color = buttonEnabled ? BUTTON_CHOSEN : BUTTON_NORMAL;
    //     }
    // }
    //
    // private void destroyButtons() {
    //     foreach (var ingredientButtons in buttons.Values) {
    //         foreach (var itemTypeButtons in ingredientButtons.Values) {
    //             foreach (GameObject button in itemTypeButtons.Values) {
    //                 Destroy(button);
    //             }
    //         }
    //     }
    //     buttons.Clear();
    // }
}
}