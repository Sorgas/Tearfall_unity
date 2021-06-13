using System.Linq;
using System.Collections.Generic;
using Assets.scripts.util.lang;
using Tearfall_unity.Assets.scripts.util.input;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.enums.material {
    public class MaterialMap : Singleton<MaterialMap> {
        private Dictionary<string, Material> map = new Dictionary<string, Material>();
        private Dictionary<int, Material> idMap = new Dictionary<int, Material>();

        public MaterialMap() {
            loadFiles();
        }

        private void loadFiles() {
            Debug.Log("loading materials");
            map.Clear();
            int id = 0;
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/materials");
            foreach (TextAsset file in files) {
                int count = 0;
                RawMaterial[] materials = JsonArrayReader.readArray<RawMaterial>(file.text);
                if (materials == null) continue;
                foreach (RawMaterial raw in materials) {
                    Material material = new Material(id++, raw);
                    map.Add(material.name, material);
                    idMap.Add(material.id, material);
                    count++;
                }
                Debug.Log("loaded " + count + " from " + file.name);
            }
        }

        public Material material(int id) {
            return idMap[id];
        }

        public Material material(string name) {
            return map[name];
        }

        public int id(string name) {
            return material(name).id;
        }

        public List<Material> getByTag(string tag) {
            return map.Values
                .Select(material => material)
                .Where(material => material.tags.Contains(tag))
                .ToList();
        }

        public List<Material> all => map.Values.ToList();
    }
}