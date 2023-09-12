using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using game.view.util;
using MoreLinq;
using TMPro;
using types.item.recipe;
using types.item.type;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;
using static game.model.component.task.order.CraftingOrder;
using static game.view.ui.UiColorsEnum;

namespace game.view.ui.workbench {
// Shows table of buttons. Each item type of each ingredient is a column. Allowed materials of ingredient give rows.
// When [anyMaterial] button is pressed, all buttons for this item type are enabled. 
public class CraftingOrderConfigPanelHandler : MonoBehaviour {
    // ingredientOrder -> itemType -> material/any
    private Dictionary<IngredientOrder, Dictionary<string, Dictionary<int, GameObject>>> buttons = new();
    public CraftingOrderLineHandler parent;
    private const int ButtonWidth = 200;
    private const int ButtonHeight = 40;
    public int buttonPadding = 5;

    public void fillFor(CraftingOrder order) {
        int columnIndex = 0;
        int maxHeight = 0;
        destroyButtons();
        foreach (IngredientOrder ingredientOrder in order.ingredients) {
            buttons.Add(ingredientOrder, new Dictionary<string, Dictionary<int, GameObject>>());
            Ingredient ingredient = ingredientOrder.ingredient;
            foreach (string itemTypeName in ingredientOrder.itemTypes) {
                buttons[ingredientOrder].Add(itemTypeName, new Dictionary<int, GameObject>());
                ItemType itemType = ItemTypeMap.getItemType(itemTypeName);
                createButton(columnIndex, 0, ingredientOrder, itemType, -1);
                for (var index = 0; index < ingredient.allowedMaterials.Count; index++) {
                    createButton(columnIndex, index + 1, ingredientOrder, itemType, ingredient.allowedMaterials[index]);
                }
                columnIndex++;
                updateButtonsColor(ingredientOrder, itemTypeName);
            }
            if (maxHeight < ingredient.allowedMaterials.Count + 1) maxHeight = ingredient.allowedMaterials.Count + 1;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector2(
            ButtonWidth * columnIndex + buttonPadding * (columnIndex + 1), 
            ButtonHeight * maxHeight + buttonPadding * (maxHeight + 1));
    }

    private void createButton(int x, int y, IngredientOrder ingredientOrder, ItemType itemType, int materialId) {
        GameObject button = PrefabLoader.create("MaterialSelectionButton", gameObject.transform,
            new Vector3(
                buttonPadding + x * (ButtonWidth + buttonPadding),
                buttonPadding + y * (ButtonHeight + buttonPadding), 0));
        Image iconImage = button.GetComponentsInChildren<Image>()[1];
        iconImage.sprite = ItemTypeMap.get().getSprite(itemType.name); // item sprite
        TextMeshProUGUI text = button.GetComponentInChildren<TextMeshProUGUI>();
        if (materialId != -1) {
            Material_ material = MaterialMap.get().material(materialId);
            iconImage.color = material.color; // item color
            text.text = material.name + " " + itemType.title;
        } else {
            text.text = "Any " + itemType.title;
            iconImage.color = Color.white;
        }
        button.GetComponentInChildren<Button>().onClick.AddListener(() => {
            toggleMaterialForIngredientOrder(ingredientOrder, itemType.name, materialId);
            updateButtonsColor(ingredientOrder, itemType.name);
        });
        buttons[ingredientOrder][itemType.name].Add(materialId, button);
    }

    private void toggleMaterialForIngredientOrder(IngredientOrder ingredientOrder, string itemType, int materialId) {
        int numberOfMaterials = buttons[ingredientOrder][itemType].Count - 1;
        if (materialId == -1) { // any material button
            if (ingredientOrder.materials.Count < numberOfMaterials) {
                buttons[ingredientOrder][itemType].Keys
                    .Where(material => material != -1)
                    .ForEach(material => ingredientOrder.materials.Add(material));
            } else {
                ingredientOrder.materials.Clear();
            }
        } else { // single material button
            if (ingredientOrder.materials.Contains(materialId)) {
                ingredientOrder.materials.Remove(materialId);
            } else {
                ingredientOrder.materials.Add(materialId);
            }
        }
    }

    private void updateButtonsColor(IngredientOrder ingredientOrder, string itemType) {
        foreach (KeyValuePair<int, GameObject> pair in buttons[ingredientOrder][itemType]) {
            Image buttonImage = pair.Value.GetComponentInChildren<Image>();
            bool buttonEnabled = pair.Key != -1
                ? ingredientOrder.materials.Contains(pair.Key)
                : ingredientOrder.materials.Count == buttons[ingredientOrder][itemType].Count - 1;
            buttonImage.color = buttonEnabled ? BUTTON_CHOSEN : BUTTON_NORMAL;
        }
    }

    private void destroyButtons() {
        foreach (var ingredientButtons in buttons.Values) {
            foreach (var itemTypeButtons in ingredientButtons.Values) {
                foreach (GameObject button in itemTypeButtons.Values) {
                    Destroy(button);
                }
            }
        }
        buttons.Clear();
    }
}
}