﻿using System.Collections.Generic;
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
            paths.Add("JobIcon", "prefabs/jobsmenu/JobIcon");
            paths.Add("UnitJobRow", "prefabs/jobsmenu/UnitJobRow");
            paths.Add("JobPriorityButton", "prefabs/jobsmenu/JobPriorityButton");
            
            paths.Add("toolbarButton", "prefabs/toolbar/toolbarButton");
            paths.Add("materialButton", "prefabs/toolbar/materialButton");
            paths.Add("toolbarPanel", "prefabs/toolbar/toolbarPanel");
            paths.Add("designation", "prefabs/Designation");
            paths.Add("Unit", "prefabs/Unit");
            paths.Add("Building", "prefabs/Building");
            paths.Add("Item", "prefabs/Item");
            paths.Add("Plant", "prefabs/Plant");
            paths.Add("RoomTile", "prefabs/RoomTile");
            
            // workbench
            paths.Add("craftingOrderLine", "prefabs/workbenchMenu/CraftingOrderLine");
            paths.Add("itemButtonWithTooltip", "prefabs/ItemButtonWithTooltip"); // TODO use for other inventory windows
            paths.Add("MaterialSelectionButton", "prefabs/workbenchMenu/MaterialSelectionButton");
            paths.Add("CraftingOrderItemTypeLine", "prefabs/workbenchMenu/CraftingOrderItemTypeLine");
            paths.Add("CraftingOrderMaterialLine", "prefabs/workbenchMenu/CraftingOrderMaterialLine");
            paths.Add("CraftingOrderConfig", "prefabs/workbenchMenu/CraftingOrderConfig");
            paths.Add("IngredientOrderPanel", "prefabs/workbenchMenu/IngredientOrderPanel");
            paths.Add("recipeLine", "prefabs/workbenchMenu/RecipeLine");
            paths.Add("itemButtonWithMaterialList", "prefabs/materialSelector/ItemButtonWithMaterialList");
            paths.Add("materialRow", "prefabs/materialSelector/MaterialRow");
            
            // stockpile
            paths.Add("StockpileCategoryRow", "prefabs/stockpileMenu/StockpileCategoryRow");
            paths.Add("WideButtonWithIcon", "prefabs/farmMenu/WideButtonWithIcon");
            // paths.Add("materialRow", "prefabs/materialSelector/MaterialRow");
            
            // tooltips
            paths.Add("SelectionTooltip", "prefabs/ui/tooltips/SelectionTooltip");
            paths.Add("ItemTooltip", "prefabs/ui/tooltips/ItemTooltip");
            paths.Add("WeaponItemTooltip", "prefabs/ui/tooltips/WeaponItemTooltip");
            paths.Add("WearItemTooltip", "prefabs/ui/tooltips/WearItemTooltip");
            paths.Add("dummyTooltip", "prefabs/ui/tooltips/DummyTooltip");
            paths.Add("textTooltip", "prefabs/ui/tooltips/TextTooltip");
            paths.Add("unitDiseaseTooltip", "prefabs/ui/tooltips/UnitDiseaseTooltip");
            
            // unit menu
            paths.Add("UnitDiseaseRow", "prefabs/unitMenu/UnitDiseaseRow");
            
            // notifications
            paths.Add("Notification", "prefabs/notifications/Notification");
            
            paths.Add("GenericTextButton", "prefabs/ui/GenericTextButton");
        }
    }
}