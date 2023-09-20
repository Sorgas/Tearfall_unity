using System.Collections.Generic;

namespace types.item.recipe {
    public class RawRecipe {
        public string name;
        public string title;
        public string itemName;
        public string newMaterial;
        public string iconName;
        public string description;

        public List<string> ingredients = new();
        public string newTag;
        public string removeTag;
        public bool uniqueIngredients;
        public float workAmount; // increases crafting time
        public string job;
        public string skill;
    }
}