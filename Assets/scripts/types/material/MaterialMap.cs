using System.Collections.Generic;
using System.Linq;
using enums.material;
using UnityEngine;
using util.input;
using util.lang;

namespace types.material {
    public class MaterialMap : Singleton<MaterialMap> {
        public const int GENERIC_PLANT = 4;
        private Dictionary<string, Material_> map = new();
        private Dictionary<int, Material_> idMap = new();

        public MaterialMap() {
            loadFiles();
            createVariants();
        }

        private void loadFiles() {
            Debug.Log("loading materials");
            map.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/materials");
            foreach (TextAsset file in files) {
                int count = 0;
                RawMaterial[] materials = JsonArrayReader.readArray<RawMaterial>(file.text);
                if (materials == null) continue;
                foreach (RawMaterial raw in materials) {
                    Material_ material = new(raw);
                    saveMaterial(material);
                    count++;
                }
                Debug.Log("loaded " + count + " from " + file.name);
            }
        }

        public Material_ material(int id) {
            return idMap[id];
        }

        public Material_ material(string name) {
            return map[name];
        }

        public int id(string name) {
            return material(name).id;
        }

        public List<Material_> getByTag(string tag) {
            return map.Values
                .Select(material => material)
                .Where(material => material.tags.Contains(tag))
                .ToList();
        }

        private void createVariants() {
            Debug.Log("creating material variants");
            int count = 0;
            count += createVariantByTag("stone", "rock", 1000);
            count += createVariantByTag("wood", "log", 1000);
            // TODO metal bar
            Debug.Log("created " + count);
        }

        private int createVariantByTag(string tag, string itemTypeName, int idMod) {
            int count = 0;
            List<Material_> materials = map.Values.Where(material => material.tags.Contains(tag)).ToList();
            foreach (Material_ material in materials) {
                Material_ variant = new(material);
                if (variant.tileset == null) variant.tileset = variant.name;
                variant.id += idMod;
                variant.name = variateValue(variant.name, itemTypeName);
                variant.tileset = variateValue(variant.tileset, itemTypeName);
                saveMaterial(variant);
                count++;
            }
            return count;
        }

        // applies wording rule to value for variation
        public static string variateValue(string value, string itemTypeName) {
            return value + "_" + itemTypeName;
        }

        private void saveMaterial(Material_ material) {
            map.Add(material.name, material);
            idMap.Add(material.id, material);
        }

        public List<Material_> all => map.Values.ToList();
    }
}