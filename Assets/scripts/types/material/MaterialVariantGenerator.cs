using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace types.material {
    // constructions should use different tilesets depending on materials and item types used to build them.
    // material variants are additional materials created on load.
    // they use same properties, except name and special tileset
    // e.g: gneiss - natural stone (in json),
    // gneiss rock wall - wall of round boulders (variant),
    // gneiss block wall - wall of square blocks (variant). 
    public class MaterialVariantGenerator {
        private MaterialMap map;

        public MaterialVariantGenerator(MaterialMap map) {
            this.map = map;
        }

        public void createVariants() {
            Debug.Log("creating material variants");
            int count = 0;
            count += createVariantByTag("stone", "rock", 1000);
            count += createVariantByTag("stone", "block", 2000);
            count += createVariantByTag("wood", "log", 1000);
            count += createVariantByTag("wood", "plank", 2000);
            // TODO metal bar
            Debug.Log("created " + count);
        }

        private int createVariantByTag(string tag, string itemTypeName, int idMod) {
            int count = 0;
            List<Material_> materials = map.all.Where(material => material.tags.Contains(tag)).ToList();
            foreach (Material_ material in materials) {
                Material_ variant = new(material);
                if (variant.tileset == null) variant.tileset = variant.name;
                variant.id += idMod;
                variant.name = variateValue(variant.name, itemTypeName);
                variant.tileset = variateValue(variant.tileset, itemTypeName);
                map.saveMaterial(variant);
                count++;
            }
            return count;
        }
        
        // applies wording rule to value for variation
        public static string variateValue(string value, string itemTypeName) => value + "_" + itemTypeName;
    }
}