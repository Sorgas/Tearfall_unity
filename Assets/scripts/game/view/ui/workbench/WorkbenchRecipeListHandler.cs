using System.Collections.Generic;
using game.model.component.building;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.building;
using types.item.recipe;
using types.item.type;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.workbench {
    // creates orders in workbench when list item is clicked
    public class WorkbenchRecipeListHandler : MonoBehaviour, ICloseable {
        public WorkbenchWindowHandler workbenchWindow;

        // fills recipes available in workbench
        public void fillFor(EcsEntity entity) {
            clear();
            string buildingTypeName = entity.take<BuildingComponent>().type.name;
            List<string> recipeNames = BuildingTypeMap.get().getRecipes(buildingTypeName);
            int recipeCount = 0;
            foreach(string recipeName in recipeNames) {
                Recipe recipe = RecipeMap.get().get(recipeName);
                if (recipe != null) {
                    addRecipeButton(recipe, recipeCount++);
                } else {
                    Debug.LogError("Recipe " + recipeName + " of " + buildingTypeName + " not found in RecipeMap.");
                }
            }
        }

        public void addRecipeButton(Recipe recipe, int index) {
            GameObject line = PrefabLoader.create("recipeLine", transform, new Vector3(5, (index * -35) - 5, 0));
            line.GetComponentInChildren<TextMeshProUGUI>().text = recipe.title;
            line.GetComponent<Button>().onClick.AddListener(() => workbenchWindow.createOrder(recipe.name));
            Sprite sprite;
            if (recipe.newType == null) {
                sprite = IconLoader.get(recipe.iconName);
            } else {
                sprite = ItemTypeMap.get().getSprite(recipe.newType);
            }
            line.GetComponentsInChildren<Image>()[1].sprite = sprite;
        }

        public void clear() {
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }
        }

        public void close() {
            gameObject.SetActive(false);
        }

        public void open() {
            gameObject.SetActive(true);
        }

        public bool accept(KeyCode key) {
            throw new System.NotImplementedException();
        }
    }
}