using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.lang.extension;

namespace types.item.recipe {
    public class IngredientProcessor {
        // format is: "itemPart:itemType1/itemType2:materialTag:quantity" 
        public Ingredient parseIngredient(string ingredientString) {
            if (!validateIngredient(ingredientString)) return null;
            string[] args = ingredientString.Split(":");
            List<string> itemTypes = new(args[1].Split("/"));
            return new Ingredient(args[0], itemTypes, args[2], int.Parse(args[3]));
        }

        // true if ok
        public bool validateIngredient(string ingredientString) {
            List<string> args = new(ingredientString.Split(":"));
            if (args.Count < 4) {
                Debug.LogError("Ingredient has empty or missing args.");
                return false;
            }
            if (args.Count(String.IsNullOrEmpty) > 0) {
                Debug.LogError("Ingredient has empty argument.");
                return false;
            }
            if (!"any".Equals(args[2]) && !ItemTags.tags.Keys.Contains(args[2])) {
                Debug.LogError("Ingredient " + ingredientString + " tag " + args[2] + " is invalid");
                return false;
            }
            return true;
        }
    }
}