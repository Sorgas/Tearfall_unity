using System;
using System.Collections.Generic;
using Assets.scripts.util.lang;
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
                Materials materials = JsonUtility.FromJson<Materials>(file.text);
                Debug.Log(materials);
                Debug.Log(materials.materials);


                foreach (RawMaterial raw in materials.materials) {
                    Material material = new Material(id++, raw);
                    map.Add(material.name, material);
                    Debug.Log("material loaded:" + material.name);
                }
            }
        }

        public override string ToString() {
            return base.ToString();
        }

        [Serializable]
        public class Materials {
            public RawMaterial[] materials;
        }
    }
}