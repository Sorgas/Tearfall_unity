using System.Collections.Generic;
using System.Linq;

namespace types.material {
    // constructions should use different tilesets depending on materials and item types used to build them.
    // material variants are additional materials created on load.
    // they use same properties, except name and special tileset
    // e.g: gneiss - natural stone (in json),
    // gneiss rock wall - wall of round boulders (variant),
    // gneiss block wall - wall of square blocks (variant).
    public class MaterialVariantGenerator {
        private MaterialMap map;
        public Dictionary<string, VariationDescriptor> descriptors = new(); 
        
        public  MaterialVariantGenerator(MaterialMap map) {
            this.map = map;
        }

        // TODO add metal bar
        public void createVariants() {
            new List<VariationDescriptor>() {
                new("stone", "rock", 1000),
                new("stone", "block", 2000),
                new("wood", "log", 1000),
                new("wood", "plank", 2000)
            }.ForEach(descriptor => {
                descriptors.Add(descriptor.itemTypeName, descriptor);
                createVariant(descriptor);
            });
        }

        private void createVariant(VariationDescriptor descriptor) {
            List<Material_> materials = map.all
                .Where(material => material.isVariant == false)
                .Where(material => material.tags.Contains(descriptor.materialTag)).ToList();
            foreach (Material_ material in materials) {
                Material_ variant = new(material);
                if (variant.tileset == null) variant.tileset = variant.name;
                variant.id += descriptor.idMod;
                variant.name = variateValue(variant.name, descriptor.itemTypeName);
                variant.tileset = variateValue(variant.tileset, descriptor.itemTypeName);
                variant.isVariant = true;
                map.saveMaterial(variant);
            }
        }
        
        // applies wording rule to value for variation
        public static string variateValue(string value, string itemTypeName) => value + "_" + itemTypeName;
    }

public class VariationDescriptor {
    public string materialTag;
    public string itemTypeName;
    public int idMod;

    public VariationDescriptor(string materialTag, string itemTypeName, int idMod) {
        this.materialTag = materialTag;
        this.itemTypeName = itemTypeName;
        this.idMod = idMod;
    }
}
}