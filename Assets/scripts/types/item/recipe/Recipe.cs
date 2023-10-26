using System.Collections.Generic;
using util.lang;

namespace types.item.recipe {
// Recipe describes crafting operation of turning items other items. 
// Crafting takes some ingredient items described in Ingredient. Crafting produces result item described in recipe.
// TODO combine with blueprints for buildigns
public class Recipe {
    public string name; // recipe(id)
    // display
    public string title; // displayed name
    public string iconName; // if itemName is empty, icon is used in workbenches
    public string description; // recipe description.

    // crafting
    // item parts, mapped to ingredient variants for this part. 'consumed' and 'main' keywords are allowed for parts
    // ingredients for same part are 'variants'. Two variants for same part should not allow same item types.
    public MultiValueDictionary<string, Ingredient> ingredients = new(); 
    public string newType; // type of crafted item
    public string newMaterial; // material of crafted item.
    public string newTag; // this tag will be added to product
    public string removeTag; // this tag will be removed from main ingredient item
    public bool uniqueIngredients; // same itemtype, material and origin cannot be used for ingredients


    public float workAmount; // increases crafting time
    public string job;
    public string skill; // if set, crafting gets bonus and gives experience in that skill

    public Recipe(RawRecipe raw) {
        name = raw.name;
        title = raw.title;
        newType = raw.itemName;
        newMaterial = raw.newMaterial;
        iconName = raw.iconName;
        description = raw.description;
        newTag = raw.newTag;
        removeTag = raw.removeTag;
        uniqueIngredients = raw.uniqueIngredients;
        workAmount = raw.workAmount != 0 ? raw.workAmount : 1f;
        job = raw.job;
        skill = raw.skill;
    }
}
}