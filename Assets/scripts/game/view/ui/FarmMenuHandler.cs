﻿using System.Collections.Generic;
using System.Linq;
using game.input;
using game.model;
using game.model.component;
using game.view.system.mouse_tool;
using game.view.ui.util;
using game.view.util;
using Leopotam.Ecs;
using TMPro;
using types.action;
using types.item.type;
using types.plant;
using UnityEngine;
using UnityEngine.UI;
using util.lang.extension;
using ZoneTasksComponent = game.model.component.ZoneTasksComponent;

namespace game.view.ui {
    public class FarmMenuHandler : MbWindow, IHotKeyAcceptor {
        public const string NAME = "farm";
        public TextMeshProUGUI farmName;
        public Image selectedPlantIcon;
        public TextMeshProUGUI selectedPlantName;
        public Button pauseButton;
        public TextMeshProUGUI priorityText;
        public Button priorityPlusButton;
        public Button priorityMinusButton;
        public Button plantListButton;
        public RectTransform plantList;
        public Button closeButton;

        public Color activeColor = new(0.75f, 0.75f, 0.75f, 1);
        public Color inactiveColor = new(0.4f, 0.4f, 0.4f, 1);

        private const int maxPriority = (int)TaskPriorityEnum.TASK_MAX_PRIORITY;
        private const int minPriority = (int)TaskPriorityEnum.TASK_MIN_PRIORITY;
        private EcsEntity farm;

        public void Start() {
            plantListButton.onClick.AddListener(() => plantList.gameObject.SetActive(true));
            pauseButton.onClick.AddListener(() => {
                ref ZoneTasksComponent tasksComponent = ref farm.takeRef<ZoneTasksComponent>();
                tasksComponent.paused = !tasksComponent.paused;
                updatePauseButton();
            });
            priorityPlusButton.onClick.AddListener(() => changePriority(1));
            priorityMinusButton.onClick.AddListener(() => changePriority(-1));
            closeButton.onClick.AddListener(() => WindowManager.get().closeWindow(NAME));
        }
        
        public void initFor(EcsEntity farm) {
            this.farm = farm;
            farmName.text = farm.name();
            // TODO task priority
            showSelectedPlant();
            fillPlantList();
            updatePauseButton();
            updatePriority();
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
            } else {
                selectedPlantName.text = "no plant selected";
            }
        }

        private void fillPlantList() {
            List<string> list = GameModel.get().technologyContainer.farmPlants;
            GameObject buttonPrefab = PrefabLoader.get("WideButtonWithIcon");
            float height = buttonPrefab.GetComponent<RectTransform>().rect.height;
            float width = buttonPrefab.GetComponent<RectTransform>().rect.width;
            plantList.sizeDelta = new Vector2(width + 10, list.Count * (height + 5) + 5);

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

        private void updatePauseButton() {
            ZoneTasksComponent tasksComponent = farm.take<ZoneTasksComponent>();
            pauseButton.GetComponent<Image>().color = tasksComponent.paused ? activeColor : inactiveColor;
        }

        private void updatePriority() {
            ref ZoneTasksComponent tasksComponent = ref farm.takeRef<ZoneTasksComponent>();
            priorityText.text = tasksComponent.priority.ToString();
            setPriorityButtonActive(priorityPlusButton, tasksComponent.priority < maxPriority);
            setPriorityButtonActive(priorityMinusButton, tasksComponent.priority > minPriority);
        }

        private void setPriorityButtonActive(Button button, bool active) {
            // button.GetComponent<Image>().color = active ? activeColor : inactiveColor;
            button.GetComponent<Button>().interactable = active;
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
            // update all tiles if selection changes
            if (farmComponent.plant != plant) farm.Replace(new ZoneUpdatedComponent { tiles = new(farm.take<ZoneComponent>().tiles) });
            farmComponent.plant = plant;
            Debug.Log("selected plant " + farmComponent.plant + "_" + plant);
            showSelectedPlant();
        }

        private void changePriority(int delta) {
            ref ZoneTasksComponent tasksComponent = ref farm.takeRef<ZoneTasksComponent>();
            tasksComponent.priority += delta;
            if (tasksComponent.priority < minPriority) tasksComponent.priority = minPriority;
            if (tasksComponent.priority > maxPriority) tasksComponent.priority = maxPriority;
            updatePriority();
        }
        
        public override string getName() => NAME;

        public bool accept(KeyCode key) {
            if (key == KeyCode.Q) {
                WindowManager.get().closeWindow(NAME);
                MouseToolManager.get().reset();
                return true;
            }
            return false;
        }

        public override void close() {
            plantList.gameObject.SetActive(false);
            base.close();
        }
    }
}