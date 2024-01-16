using System.Collections.Generic;
using System.Linq;
using game.model;
using game.model.component.task.order;
using game.view.util;
using TMPro;
using types.item.recipe;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.workbench {
// panel for one ingredient of crafting order. Shows options for all variants of IngredientOrder.
public class CraftingOrderConfigPanelIngredientHandler : MonoBehaviour {
    public TextMeshProUGUI titleText;
    public RectTransform scrollContent;

    private readonly Dictionary<string, CraftingOrderConfigLine> typeRows = new();
    private readonly Dictionary<string, Dictionary<int, CraftingOrderConfigLine>> materialRows = new();
    private IngredientOrder order;

    public void fillFor(string title, IngredientOrder order) {
        Debug.Log("filling ingredient panel for " + order.ingredients[0].key);
        this.order = order;
        titleText.text = title;

        foreach (GameObject child in scrollContent) {
            Destroy(child);
        }
        int counter = 0;
        foreach (Ingredient orderIngredient in order.ingredients) {
            foreach (string type in orderIngredient.itemTypes) {
                GameObject typeGo = PrefabLoader.create("CraftingOrderItemTypeLine", scrollContent, new Vector3(0, -30 * counter, 0));
                CraftingOrderConfigLine typeRow = typeGo.GetComponent<CraftingOrderConfigLine>();
                typeRows.Add(type, typeRow);
                materialRows.Add(type, new());
                typeGo.GetComponent<Button>().onClick.AddListener(() => toggleType(type));
                Dictionary<int, int> quantities = GameModel.get().currentLocalModel.itemContainer.availableItemsManager.findQuantitiesByType(type);
                bool typeSelected = order.selected.ContainsKey(type)
                                    && orderIngredient.materials.TrueForAll(mat => order.selected[type].Contains(mat));
                int totalQuantity = quantities.Values.Aggregate(0, (q1, q2) => q1 + q2);
                typeRow.initType(type, totalQuantity, typeSelected);
                counter++;
                foreach (int material in orderIngredient.materials) {
                    GameObject materialGo = PrefabLoader.create("CraftingOrderMaterialLine", scrollContent, new Vector3(12, -30 * counter, 0));
                    CraftingOrderConfigLine materialRow = materialGo.GetComponent<CraftingOrderConfigLine>();
                    materialRows[type].Add(material, materialRow);
                    bool materialSelected = order.selected.ContainsKey(type)
                                            && order.selected[type].Contains(material);
                    materialRow.GetComponent<Button>().onClick.AddListener(() => toggleMaterial(type, material));
                    int quantity = quantities.ContainsKey(material) ? quantities[material] : 0;
                    materialRow.initMaterial(type, material, quantity, materialSelected);
                    counter++;
                }
            }
        }
        scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, counter * 30);
    }

    private void toggleType(string type) {
        Debug.Log("toggling type");
        bool selected = typeSelected(type);
        typeRows[type].setSelected(!selected);
        if (selected) { // deselect all materials
            foreach (CraftingOrderConfigLine craftingOrderConfigLine in materialRows[type].Values) {
                craftingOrderConfigLine.setSelected(false);
            }
            order.selected.Remove(type);
        } else { // select all materials
            foreach (CraftingOrderConfigLine craftingOrderConfigLine in materialRows[type].Values) {
                craftingOrderConfigLine.setSelected(true);
                order.selected.add(type, craftingOrderConfigLine.material);
            }
        }
    }

    private void toggleMaterial(string type, int material) {
        Debug.Log("toggling material");
        bool materialSelected = order.selected.contains(type, material);
        if (materialSelected) { // deselect material and type
            materialRows[type][material].setSelected(false);
            order.selected.remove(type, material);
            typeRows[type].setSelected(false);
        } else { // select material and check type selection
            materialRows[type][material].setSelected(true);
            order.selected.add(type, material);
            if (typeSelected(type)) {
                typeRows[type].setSelected(true);
            }
        }
    }

    private bool typeSelected(string type) {
        return order.selected.ContainsKey(type) && selectIngredient(type).materials.TrueForAll(mat => order.selected[type].Contains(mat));
    }

    private bool noMaterialOfTypeSelected(string type) {
        return !order.selected.ContainsKey(type);
    }

    private Ingredient selectIngredient(string type) {
        return order.ingredients.First(ingredient => ingredient.itemTypes.Contains(type));
    }
}
}