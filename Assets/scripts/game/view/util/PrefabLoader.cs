using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.util {
    public class PrefabLoader : Singleton<PrefabLoader> {
        private Dictionary<string, GameObject> prefabs = new();
        private Dictionary<string, string> paths = new();

        public static GameObject get(string name) {
            return get().getPrefab(name);
        }

        public static GameObject create(string name) {
            return Object.Instantiate(get(name));
        }
        
        public static GameObject create(string name, Transform parent) {
            return Object.Instantiate(get(name), parent);
        }
        
        public static GameObject create(string name, Transform parent, Vector3 localPosition) {
            return Object.Instantiate(get(name), localPosition, Quaternion.identity, parent);
        }
        
        private GameObject getPrefab(string name) {
            if (!prefabs.ContainsKey(name)) {
                if (!paths.ContainsKey(name)) {
                    Debug.LogError("Prefab with name " + name + " not registered in PrefabLoader");
                    return null;
                }
                prefabs.Add(name, Resources.Load<GameObject>(paths[name]));
            }
            return prefabs[name];
        }

        protected override void init() {
            paths.Add("toolbarButton", "prefabs/toolbar/toolbarButton");
            paths.Add("materialButton", "prefabs/toolbar/materialButton");
            paths.Add("toolbarPanel", "prefabs/toolbar/toolbarPanel");
            paths.Add("designation", "prefabs/Designation");
        }
    }
}