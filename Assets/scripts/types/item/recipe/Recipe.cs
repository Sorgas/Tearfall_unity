using System.Collections.Generic;

namespace types.item.recipe {
    public class Recipe {
        public string name;                               // recipe(id)
        public string title;                              // displayed name
        public string iconName;                           // if itemName is empty, icon is used in workbenches
        public string description;                        // recipe description.
        
        public string newType;                            // type of crafted item
        public string newMaterial;                        // material of crafted item.
        public string newTag;                        // this tag will be added to product
        public string removeTag;                     // this tag will be removed from main ingredient item

        public Dictionary<string, Ingredient> ingredients = new(); // all ingredients, mapped to parts, 'consumed' or 'main'

        public float workAmount;                          // increases crafting time
        public string job;                                // if null, no job requirement applied
        public string skill;                              // if set, crafting gets bonus and gives experience in that skill

        public Recipe(RawRecipe raw) {
            name = raw.name;
            title = raw.title;
            newType = raw.itemName;
            newMaterial = raw.newMaterial;
            iconName = raw.iconName;
            description = raw.description;
            newTag = raw.newTag;
            removeTag = raw.removeTag;
            workAmount = raw.workAmount != 0 ? raw.workAmount : 1f;
            job = raw.job;
            skill = raw.skill;
        }
    }
}