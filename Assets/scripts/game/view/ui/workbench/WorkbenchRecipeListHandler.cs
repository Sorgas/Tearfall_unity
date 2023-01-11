using System.Collections.Generic;
using game.model.component.building;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.building;
using types.item.recipe;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui.workbench {
    public class WorkbenchRecipeListHandler : MonoBehaviour, ICloseable {
        public WorkbenchWindowHandler workbenchWindow;

        // fills recipes available in workbench
        public void fillFor(EcsEntity entity) {
            clear();
            string name = entity.take<BuildingComponent>().type.name;
            List<string> recipeNames = BuildingTypeMap.get().getRecipes(name);
            int recipeCount = 0;
            foreach(string recipeName in recipeNames) {
                Recipe recipe = RecipeMap.get().get(recipeName);
                if (recipe != null) {
                    addRecipeButton(recipe, recipeCount++);
                } else {
                    Debug.LogError("Recipe " + recipeName + " of " + name + " not found in RecipeMap.");
                }
            }
        }

        public void addRecipeButton(Recipe recipe, int index) {
            GameObject line = PrefabLoader.create("recipeLine", transform, new Vector3(0, index * -40, 0));
            line.GetComponentInChildren<TextMeshProUGUI>().text = recipe.title;
            line.GetComponent<Button>().onClick.AddListener(() => workbenchWindow.createOrder(recipe.name));
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