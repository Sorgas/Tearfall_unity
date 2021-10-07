using System;
using System.Collections.Generic;
using generation;
using mainMenu.worldmap;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using util.geometry;
using Random = System.Random;

namespace mainMenu {
    public class WorldGenButtonHandler : ButtonHandler {
        public Slider sizeSlider;
        public InputField seedField;
        public WorldmapController worldmapController;
        public Button continueButton;

        public GameObject mainMenuStage;
        public GameObject preparationStage;

        protected override void initButtons() {
            buttons = new List<ButtonData> {
                new ButtonData("CreateButton", KeyCode.C, createWorld),
                new ButtonData("BackButton", KeyCode.Q, backToMainMenu),
                new ButtonData("ContinueButton", KeyCode.V, toPreparation)
            };
        }

        public new void Start() {
            base.Start();
            seedField.text = new Random().Next().ToString();
            GUI.FocusControl(sizeSlider.name);
            EventSystem.current.SetSelectedGameObject(null, null);
        }

        // invoked several times
        public void createWorld() {
            int seed = Convert.ToInt32(seedField.text);
            int size = (int)sizeSlider.value * 100;
            GenerationState.get().worldGenConfig.seed = seed;
            GenerationState.get().worldGenConfig.size = size;
            GenerationState.get().generateWorld();
            worldmapController.drawWorld(GenerationState.get().world.worldMap);
            continueButton.gameObject.SetActive(true);
        }

        private void backToMainMenu() {
            switchTo(mainMenuStage);
            resetState();
        }

        private void resetState() {
            worldmapController.clear();
            continueButton.gameObject.SetActive(false);
        }

        private void toPreparation() {
            Vector3 pointerPosition = worldmapController.pointer.localPosition;
            GenerationState.get().localGenConfig.location = new IntVector2((int)pointerPosition.x, (int)pointerPosition.y);
            switchTo(preparationStage);
        }
    }
}