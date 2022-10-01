using System;
using System.Collections.Generic;
using System.Linq;
using enums.item;
using UnityEngine;

public class IngredientProcessor {
    public Ingredient parseIngredient(string ingredientString) {
        if (!validateIngredient(ingredientString)) return null;
        string[] args = ingredientString.Split(":");
        List<string> itemTypes = new(args[1].Split("/"));
        ItemTagEnum tag = ItemTagEnum.BREWABLE.get(args[2]);
        return new Ingredient(args[0], itemTypes, tag, int.Parse(args[3]));
    }

    public bool validateIngredient(string ingredientString) {
        List<string> args = new(ingredientString.Split(":"));
        if (args.Count < 4) {
            Debug.LogError("Ingredient has empty or missing args.");
            return false;
        }
        if (args.Where(s => s == null || s.Length == 0).Count() > 0) {
            Debug.LogError("Ingredient has empty argument.");
            return false;
        }
        if (!"any".Equals(args[2])) {
            try {
                ItemTagEnum.BREWABLE.get(args[2]);
            } catch (ArgumentException) {
                Debug.LogError("Ingredient " + ingredientString + " tag " + args[2] + " is invalid");
                return false;
            }
        }
        return true;
    }
}