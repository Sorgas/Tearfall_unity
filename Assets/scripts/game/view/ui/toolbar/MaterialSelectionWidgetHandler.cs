using System.Collections.Generic;
using enums.item.type;
using enums.material;
using game.model;
using game.view.system.mouse_tool;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.building;
using UnityEngine;
using UnityEngine.UI;
using util.lang;

namespace game.view.ui.toolbar {
    // shows buttons for selecting material for building.
    public class MaterialSelectionWidgetHandler : MonoBehaviour {
        private List<GameObject> buttons = new();

        public void close() {
            gameObject.SetActive(false);
        }

        public void open() {
            gameObject.SetActive(true);
        }

        public void clear() {
            foreach (GameObject button in buttons) Destroy(button);
            buttons.Clear();
        }

        public void fillForConstructionType(ConstructionType type) {
            clear();
            foreach (string materialOption in type.materials) {
                string[] args = materialOption.Split("/");
                string typeName = args[0];
                int requiredAmount = int.Parse(args[1]);
                MultiValueDictionary<int, EcsEntity>
                    map = GameModel.get().itemContainer.availableItemsManager.findByType(typeName);
                foreach (int materialId in map.Keys) {
                    if (map[materialId].Count >= requiredAmount) {
                        createButton(typeName, materialId, requiredAmount, map[materialId].Count);
                    }
                }
            }
        }

        public void createButton(string typeName, int materialId, int requiredAmount, int amount) {
            GameObject button = PrefabLoader.create("materialButton", gameObject.transform);
            float buttonWidth = button.GetComponent<RectTransform>().rect.width;
            button.transform.localPosition = new Vector3(buttonWidth * buttons.Count, 0, 0);
            button.GetComponentInChildren<Button>().onClick
                .AddListener(() => MouseToolManager.get().setItemForConstruction(typeName, materialId));
            Material_ material = MaterialMap.get().material(materialId);
            button.GetComponentInChildren<TextMeshProUGUI>().text = 
                material.name + " " + typeName + " " + amount + "/" + requiredAmount;
            button.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = ItemTypeMap.get().getSprite(typeName);
            buttons.Add(button);
        }
    }
}