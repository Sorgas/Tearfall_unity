using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using util.lang;

namespace types.item.recipe {
public class RecipeMap : Singleton<RecipeMap> {
    private Dictionary<string, Recipe> map = new();
    private string logMessage;
    private bool debug = false;
    private RecipeProcessor processor = new();

    public RecipeMap() {
        loadFiles();
    }

    public Recipe get(string recipeName) => map.ContainsKey(recipeName) ? map[recipeName] : null;

    private void loadFiles() {
        log("loading recipes");
        map.Clear();
        TextAsset[] files = Resources.LoadAll<TextAsset>("data/recipes/");
        foreach (var file in files) {
            List<RawRecipe> raws = JsonConvert.DeserializeObject<List<RawRecipe>>(file.text);
            foreach (var raw in raws) {
                Recipe recipe = processor.processRawRecipe(raw);
                if (recipe != null) {
                    if (map.ContainsKey(recipe.name)) {
                        Debug.LogWarning("recipe " + recipe.name + " defined in multiple files!");
                    } else {
                        map.Add(recipe.name, recipe);
                    }
                }
            }
            log("loaded " + raws.Count + " from " + file.name);
        }
        if(debug) Debug.Log(logMessage);
    }

    private void log(string message) {
        if(debug) logMessage += message + "\n";
    }
}
}