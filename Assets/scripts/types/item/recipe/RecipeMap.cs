using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using util.lang;

namespace Assets.scripts.types.item.recipe {
    public class RecipeMap : Singleton<RecipeMap> {
        private Dictionary<string, Recipe> map = new();
        private string logMessage;
        private RecipeProcessor processor = new();

        public RecipeMap() {
            loadFiles();
        }

        public Recipe get(string recipeName) => map.ContainsKey(recipeName) ? map[recipeName] : null;

        private void loadFiles() {
            Debug.Log("loading recipes");
            map.Clear();
            TextAsset file = Resources.Load<TextAsset>("data/recipes/recipe.json");
            List<RawRecipe> raws = JsonConvert.DeserializeObject<List<RawRecipe>>(file.text);
            foreach (var raw in raws) {
                Recipe recipe = processor.processRawRecipe(raw);
                if(recipe != null) map.Add(recipe.name, recipe);
            }
            Debug.Log("loaded " + raws.Count + " from " + file.name);
        }

        private void log(string message) {
            logMessage += message + "\n";
        }
    }
}