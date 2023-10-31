using System;
using System.Collections.Generic;
using System.Linq;
using types.item.type;
using types.material;
using UnityEngine;

namespace types.item.recipe {
// Parses strings from recipe definition into Ingredient objects.
// Format is: "itemPart:itemType1,itemType2/typeTag1,typeTag2:material1,material2/materialTag1,materialTag2:quantity"
// If tags not present, items will not be filtered by tags. Tags filters combined with AND.
// Word 'any' can be used to allow any item type or material.
public class IngredientProcessor {
    public Ingredient parseIngredient(string ingredientString) {
        Debug.Log(ingredientString);
        if (!validateIngredient(ingredientString)) return null;
        string[] args = ingredientString.Split(":");
        // types
        string[] typeArgs = args[1].Split('/');
        List<string> itemTypes = new(typeArgs[0].Equals("any")
            ? ItemTypeMap.getAll().Select(type => type.name)
            : typeArgs[0].Split(',')
        );
        if (typeArgs.Length == 2) { // filter by tags
            string[] typeTags = typeArgs[1].Split(',');
            itemTypes = itemTypes.Select(type => ItemTypeMap.get().getType(type))
                .Where(type => type.tags.IsSupersetOf(typeTags))
                .Select(type => type.name).ToList();
        }
        // materials
        string[] materialArgs = args[2].Split('/');
        List<int> materials = new(materialArgs[0].Equals("any")
            ? MaterialMap.get().all.Select(material => material.id)
            : materialArgs[0].Split(',').Select(name => MaterialMap.get().material(name).id)
        );
        if (materialArgs.Length == 2) { // filter by tags
            string[] materialTags = materialArgs[1].Split(',');
            materials = materials.Select(material => MaterialMap.get().material(material))
                .Where(material => material.tags. IsSupersetOf(materialTags))
                .Select(material => material.id).ToList();
        }
        return new Ingredient(args[0], itemTypes, materials, int.Parse(args[3]));
    }

    // true if ok
    public bool validateIngredient(string ingredientString) {
        List<string> args = new(ingredientString.Split(":"));
        if (args.Count < 4) {
            Debug.LogError($"Ingredient {ingredientString} has empty or missing arguments.");
            return false;
        }
        if (args.Count(String.IsNullOrEmpty) > 0) {
            Debug.LogError($"Ingredient {ingredientString} has empty argument.");
            return false;
        }
        string[] itemTypeArgs = args[1].Split('/');
        if (itemTypeArgs[0] != "any") {
            foreach (string type in itemTypeArgs[0].Split(',')) {
                if (!ItemTypeMap.get().hasType(type)) {
                    Debug.LogError($"Ingredient {ingredientString} has invalid item type name {type}.");
                    return false;
                }
            }
        }
        if (itemTypeArgs.Length == 2 && !validateTags(itemTypeArgs[1], ingredientString)) return false;
        string[] materialArgs = args[1].Split('/');
        // material names
        if (materialArgs[0] != "any") {
            foreach (string type in materialArgs[0].Split(',')) {
                if (!ItemTypeMap.get().hasType(type)) {
                    Debug.LogError($"Ingredient {ingredientString} has invalid item type name {type}.");
                    return false;
                }
            }
        }
        // material tags
        if (materialArgs.Length == 2 && !validateTags(materialArgs[1], ingredientString)) return false;
        return true;
    }

    private bool validateTags(string tagString, string ingredientString) {
        foreach (string tag in tagString.Split(',')) {
            if (!ItemTags.tags.Keys.Contains(tag)) {
                Debug.LogError($"Ingredient {ingredientString} has invalid tag {tag}.");
                return false;
            }
        }
        return true;
    }
}
}