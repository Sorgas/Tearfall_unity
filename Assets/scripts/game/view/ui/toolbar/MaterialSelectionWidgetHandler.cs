using System.Collections.Generic;
using game.model;
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
        
        public bool fill(BuildingVariant[] variants) {
            clear();
            // variant -> materialId -> items
            Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> items =
                GameModel.get().currentLocalModel.itemContainer.util.findForBuildingVariants(variants);
            bool itemsSelected = false;
            for (int i = 0; i < variants.Length; i++) {
                GameObject button = 
                    PrefabLoader.create("itemButtonWithMaterialList", gameObject.transform, new Vector3(i * 100, 0, 0));
                MaterialButtonHandler handler = button.GetComponentInChildren<MaterialButtonHandler>();
                handler.init(variants[i], items[variants[i]], this);
                if (!itemsSelected && handler.hasEnoughItems()) {
                    handler.selectAny();
                    itemsSelected = true;
                }
            }
            return itemsSelected;
        }
        
        public void clear() {
            foreach (MaterialButtonHandler button in buttons) Destroy(button.gameObject);
            buttons.Clear();
        }

        public void selectFirst() {
            if (buttons.Count <= 0) return;
            buttons[0].GetComponentInChildren<Button>().onClick.Invoke(); // select first material by default
        }

        public void updateSelected(string itemType, int materialId) {
            buttons.ForEach(button => button.updateSelected(itemType, materialId));
        }

        public void close() => gameObject.SetActive(false);

        public void open() => gameObject.SetActive(true);
    }
}