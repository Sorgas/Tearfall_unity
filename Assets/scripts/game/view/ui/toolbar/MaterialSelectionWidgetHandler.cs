using System.Collections.Generic;
using game.model;
using game.view.system.mouse_tool;
using game.view.ui.material_selector;
using game.view.util;
using Leopotam.Ecs;
using types.building;
using UnityEngine;
using UnityEngine.UI;
using util.lang;

namespace game.view.ui.toolbar {
    // shows buttons for selecting material for construction or building.
    // TODO keep selected material between usages
    // TODO add multi-ingredient buildings
    public class MaterialSelectionWidgetHandler : MonoBehaviour {
        private List<MaterialButtonHandler> buttons = new();
        private Dictionary<string, KeyValuePair<string, int>> selectedMaterials = new();
        private string currentBuilding;
        
        public bool fill(string buildingName, BuildingVariant[] variants) {
            clear();
            // variant -> materialId -> items
            Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> items =
                GameModel.get().currentLocalModel.itemContainer.util.findForBuildingVariants(variants);
            MaterialButtonHandler buttonToSelect = null;
            for (int i = 0; i < variants.Length; i++) {
                GameObject button = 
                    PrefabLoader.create("itemButtonWithMaterialList", gameObject.transform, new Vector3(i * 100, 0, 0));
                MaterialButtonHandler handler = button.GetComponentInChildren<MaterialButtonHandler>();
                handler.init(variants[i], items[variants[i]], this);
                if (buttonToSelect == null && handler.hasEnoughItems()) buttonToSelect = handler;
                buttons.Add(handler);
            }
            if(buttonToSelect != null) buttonToSelect.selectAny();
            return buttonToSelect != null;
        }
        
        public void clear() {
            foreach (MaterialButtonHandler button in buttons) Destroy(button.gameObject);
            buttons.Clear();
        }

        public void selectFirst() {
            if (buttons.Count > 0) buttons[0].selectAny();
        }

        // selects itemType/material combination to mouse tool
        public void select(string itemType, int materialId) {
            MouseToolManager.get().setItem(itemType, materialId);
            buttons.ForEach(button => button.updateSelected(itemType, materialId));
        }

        public void close() => gameObject.SetActive(false);

        public void open() => gameObject.SetActive(true);
    }
}