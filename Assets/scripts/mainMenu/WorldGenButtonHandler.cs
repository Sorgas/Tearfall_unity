using System;
using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.generation;
using Assets.scripts.generation.worldgen;
using Assets.scripts.mainMenu.worldmap;
using Assets.scripts.util.geometry;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

namespace Assets.scripts.mainMenu {
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

        public void createWorld() {
            Debug.Log("creating world");
            int seed = Convert.ToInt32(seedField.text);
            int size = (int)sizeSlider.value * 100;
            Debug.Log("creating world " + seed + " " + size);
            GenerationState.get().generateWorld(seed, size);
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
            GenerationState.get().setLocalPosition(new IntVector2((int)pointerPosition.x, (int)pointerPosition.y));
            Debug.Log("player position = " + pointerPosition);
            switchTo(preparationStage);
        }
    }
}