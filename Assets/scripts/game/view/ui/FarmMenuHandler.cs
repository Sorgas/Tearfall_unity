using System.Collections.Generic;
using System.Linq;
using game.model;
using game.model.component;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.item.type;
using types.plant;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;

namespace game.view.ui {
    public class FarmMenuHandler : MbWindow {
        public const string name = "farm";
        public TextMeshProUGUI farmName;
        public Image selectedPlantIcon;
        public TextMeshProUGUI selectedPlantName;
        public Button pauseButton;
        public Button priorityPlusButton;
        public Button priorityMinusButton;
        public Button plantListButton;
        public RectTransform plantList;

        private EcsEntity farm;

        public void initFor(EcsEntity farm) {
            this.farm = farm;
            farmName.text = farm.name();
            // TODO task priority
            showSelectedPlant();
            fillPlantList();
            plantListButton.onClick.AddListener(() => {
                plantList.gameObject.SetActive(true);
            });
        }

        private void showSelectedPlant() {
            FarmComponent farmComponent = farm.take<FarmComponent>();
            if (farmComponent.plant != null) {
                PlantType type = PlantTypeMap.get().get(farmComponent.plant);
                selectedPlantName.text = type.title;
                string product = type.productItem;
                if (product != null) {
                    selectedPlantIcon.sprite = ItemTypeMap.get().getSprite(product);
                }
            }
            selectedPlantName.text = "no plant selected";
        }

        private void fillPlantList() {
            List<string> list = GameModel.get().technologyContainer.farmPlants;
            GameObject buttonPrefab = PrefabLoader.get("WideButtonWithIcon");
            float height = buttonPrefab.GetComponent<RectTransform>().rect.height;
            plantList.sizeDelta = new Vector2(210, list.Count * (height + 5) + 5);
            
            for (int i = 0; i < list.Count; i++) {
                string plant = list[i];
                PlantType type = PlantTypeMap.get().get(plant);
                GameObject button = PrefabLoader.create("WideButtonWithIcon", plantList, 
                    new Vector3(5, 5 + i * (height + 5), 0));
                Debug.Log(5 + i * (height + 5));
                button.GetComponent<Button>().onClick.AddListener(() => selectPlant(plant));
                button.GetComponentInChildren<TextMeshProUGUI>().text = type.title;
                button.GetComponentsInChildren<Image>()[1].sprite = createSprite(type);
            }
        }

        private Sprite createSprite(PlantType type) {
            List<string> productItems = type.lifeStages
                .Where(stage => stage.harvestProduct != null)
                .Select(stage => stage.harvestProduct)
                .ToList();
            return productItems.Count > 0 ? ItemTypeMap.get().getSprite(productItems[0]) : null;
        }
        
        private void selectPlant(string plant) {
            ref FarmComponent farmComponent = ref farm.takeRef<FarmComponent>();
            farmComponent.plant = plant;
        }

        public override string getName() {
            return name;
        }
    }
}