using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.util {
    public class PrefabLoader : Singleton<PrefabLoader> {
        private Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();
        private Dictionary<string, string> paths = new Dictionary<string, string>();

        public static GameObject get(string name) {
            return get().getPrefab(name);
        }

        public static GameObject create(string name) {
            return GameObject.Instantiate(get(name));
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
            paths.Add("toolbarPanel", "prefabs/toolbar/toolbarPanel");
            paths.Add("designation", "prefabs/Designation");
        }
    }
}