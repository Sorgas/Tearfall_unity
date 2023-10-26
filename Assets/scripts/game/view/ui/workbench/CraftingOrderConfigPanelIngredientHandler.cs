using System.Collections.Generic;
using System.Linq;
using game.model.component.task.order;
using game.view.util;
using TMPro;
using types.item.recipe;
using UnityEngine;
using UnityEngine.UI;
using static game.view.ui.UiColorsEnum;

namespace game.view.ui.workbench {
// shows 
public class CraftingOrderConfigPanelIngredientHandler : MonoBehaviour {
    public TextMeshProUGUI titleText;
    public RectTransform scrollContent;

    private readonly Dictionary<string, GameObject> typeRows = new();
    private readonly Dictionary<string, Dictionary<int, GameObject>> materialRows = new();
    private CraftingOrder.IngredientOrder order;
    
    public void fillFor(string title, CraftingOrder.IngredientOrder order) {
        Debug.Log("filling ingredient panel for " + order.ingredients[0].key);
        this.order = order;
        titleText.text = title;
        
        foreach (GameObject child in scrollContent) {
            Destroy(child);
        }
        int counter = 0;
        foreach (Ingredient orderIngredient in order.ingredients) {
            foreach (string type in orderIngredient.itemTypes) {
                GameObject typeRow = PrefabLoader.create("CraftingOrderItemTypeLine", scrollContent, new Vector3(0, -30 * counter, 0));
                typeRows.Add(type, typeRow);
                materialRows.Add(type, new());
                typeRow.GetComponent<Button>().onClick.AddListener(() => toggleType(type));
                bool typeSelected = orderIngredient.materials.TrueForAll(mat => order.selected[type].Contains(mat));
                typeRow.GetComponent<Image>().color = typeSelected ? backgroundHighlight : background;
                counter++;
                foreach (int material in orderIngredient.materials) {
                    GameObject materialRow = PrefabLoader.create("CraftingOrderMaterialLine", scrollContent, new Vector3(0, -30 * counter, 0));
                    materialRows[type].Add(material, materialRow);
                    materialRow.GetComponent<Button>().onClick.AddListener(() => toggleMaterial(type, material));
                    materialRow.GetComponent<Image>().color = order.selected.contains(type, material) ? backgroundHighlight : background;
                    counter++;
                }
            }
        }
        scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, counter * 30);
    }

    private void toggleType(string type) {
        Ingredient ingredient = selectIngredient(type);
        bool typeSelected = ingredient.materials.TrueForAll(mat => order.selected[type].Contains(mat));
        if (typeSelected) {
            // deselect all materials
        } else {
            foreach (var material in materialRows[type].Keys) {
                
            }
            // select all materials
        }
    }

    private void toggleMaterial(string type, int material) { }

    private Ingredient selectIngredient(string type) {
        return order.ingredients.First(ingredient => ingredient.itemTypes.Contains(type));
    }
}
}