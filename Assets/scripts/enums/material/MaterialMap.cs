using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using util.input;
using util.lang;

namespace enums.material {
    public class MaterialMap : Singleton<MaterialMap> {
        private Dictionary<string, Material_> map = new Dictionary<string, Material_>();
        private Dictionary<int, Material_> idMap = new Dictionary<int, Material_>();

        public MaterialMap() {
            loadFiles();
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
                    Material_ material = new Material_(raw);
                    map.Add(material.name, material);
                    idMap.Add(material.id, material);
                    count++;
                }
                Debug.Log("loaded " + count + " from " + file.name);
            }
        }

        public Material_ material(int id) {
            // Debug.Log(id + " " + idMap.Count + " " + map.Count);
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

        public List<Material_> all => map.Values.ToList();
    }
}