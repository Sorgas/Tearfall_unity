using System.Collections.Generic;
using Assets.scripts.util.lang;
using Tearfall_unity.Assets.scripts.util.input;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.enums.material {
    public class MaterialMap : Singleton<MaterialMap> {
        private Dictionary<string, Material> map = new Dictionary<string, Material>();

        public static void load() {
            get().loadFiles();
        }

        private void loadFiles() {
            map.Clear();
            int id = 0;
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/materials");
            foreach (TextAsset file in files) {
                RawMaterial[] materials = JsonArrayReader.readArray<RawMaterial>(file.text);
                if (materials == null) continue;
                foreach (RawMaterial raw in materials) {
                    Material material = new Material(id++, raw);
                    map.Add(material.name, material);
                    Debug.Log("material loaded:" + material.name);
                }
            }
        }
    }
}