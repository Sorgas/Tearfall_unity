using System.Collections.Generic;
using UnityEngine;
using util.lang;

namespace game.view.util {
    // registry and utility wrapper for prefabs. prefabs should be registered with their paths.
    // all prefabs in game should have unique names
    // TODO add recursive search for prefabs.
    public class PrefabLoader : Singleton<PrefabLoader> {
        private Dictionary<string, GameObject> prefabs = new();
        private Dictionary<string, string> paths = new(); // prefab filename to path

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
            GameObject prefab = Object.Instantiate(get(name), localPosition, Quaternion.identity, parent);
            prefab.transform.localPosition = localPosition;
            return prefab;
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
            paths.Add("Unit", "prefabs/Unit");
            paths.Add("Building", "prefabs/Building");
            paths.Add("Item", "prefabs/Item");
            paths.Add("Plant", "prefabs/Plant");
            
            // workbench
            paths.Add("craftingOrderLine", "prefabs/workbenchMenu/CraftingOrderLine");
            paths.Add("itemIconWithTooltip", "prefabs/workbenchMenu/ItemIconWithTooltip"); // TODO use for other inventory windows
            paths.Add("MaterialSelectionButton", "prefabs/workbenchMenu/MaterialSelectionButton");
            paths.Add("recipeLine", "prefabs/workbenchMenu/RecipeLine");
            paths.Add("itemButtonWithMaterialList", "prefabs/materialSelector/ItemButtonWithMaterialList");
            paths.Add("materialRow", "prefabs/materialSelector/MaterialRow");
            
            // stockpile
            
            paths.Add("StockpileCategoryRow", "prefabs/stockpileMenu/StockpileCategoryRow");
            paths.Add("WideButtonWithIcon", "prefabs/farmMenu/WideButtonWithIcon");
            // paths.Add("materialRow", "prefabs/materialSelector/MaterialRow");
        }
    }
}