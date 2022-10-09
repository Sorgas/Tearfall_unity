using System.Collections.Generic;
using enums.item.type;
using enums.material;
using game.model;
using game.view.system.mouse_tool;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.building;
using types.material;
using UnityEngine;
using UnityEngine.UI;
using util.lang;

namespace game.view.ui.toolbar {
    // shows buttons for selecting material for construction or building.
    // TODO keep selected material between usages
    // TODO add multi-ingredient buildings
    public class MaterialSelectionWidgetHandler : MonoBehaviour {
        private List<GameObject> buttons = new();

        public void close() => gameObject.SetActive(false);

        public void open() => gameObject.SetActive(true);

        public void clear() {
            foreach (GameObject button in buttons) Destroy(button);
            buttons.Clear();
        }

        public bool fill(BuildingVariant[] variants) {
            clear();
            Dictionary<BuildingVariant, MultiValueDictionary<int, EcsEntity>> items =
                GameModel.get().currentLocalModel.itemContainer.util.findForBuildingVariants(variants);
            foreach (BuildingVariant variant in variants) {
                if (items.ContainsKey(variant)) {
                    foreach (int material in items[variant].Keys) {
                        createButton(variant.itemType, material, variant.amount, items[variant][material].Count);
                    }
                } else {
                    // TODO add "not enough materials label"
                }
            }
            return buttons.Count > 0;
        }

        public void selectFirst() {
            buttons[0].GetComponentInChildren<Button>().onClick.Invoke(); // select first material by default
        }

        private void createButton(string typeName, int materialId, int requiredAmount, int amount) {
            GameObject button = PrefabLoader.create("materialButton", gameObject.transform);
            float buttonWidth = button.GetComponent<RectTransform>().rect.width;
            button.transform.localPosition = new Vector3(buttonWidth * buttons.Count, 0, 0);
            button.GetComponentInChildren<Button>().onClick
                .AddListener(() => MouseToolManager.get().setItem(typeName, materialId));
            Material_ material = MaterialMap.get().material(materialId);
            button.GetComponentInChildren<TextMeshProUGUI>().text = 
                material.name + " " + typeName + " " + amount + "/" + requiredAmount;
            button.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ItemTypeMap.get().getSprite(typeName);
            buttons.Add(button);
        }
    }
}