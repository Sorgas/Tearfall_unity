using System;
using System.Collections.Generic;
using game.model;
using generation;
using mainMenu.worldmap;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Slider = UnityEngine.UI.Slider;

namespace mainMenu {
public class WorldGenStageHandler : StageHandler {
    public InputField seedField;
    public Slider sizeSlider;
    public Slider deityCountSlider;
    public Slider civCountSlider;
    public WorldmapController worldmapController;

    public GameObject mainMenuStage;
    public GameObject locationSelectionStage;
    private GameObject continueButton;

    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("CreateButton", KeyCode.C, createWorld),
            new("BackButton", KeyCode.Q, backToMainMenu),
            new("ContinueButton", KeyCode.V, toPositionSelection)
        };
    }

    public new void Start() {
        base.Start();
        seedField.text = new Random().Next().ToString();
        continueButton = buttons["ContinueButton"].gameObject;
    }

    // invoked several times
    public void createWorld() {
        int seed = Convert.ToInt32(seedField.text);
        int size = (int)sizeSlider.value * 100;
        GenerationState.get().worldGenConfig.seed = seed;
        GenerationState.get().worldGenConfig.size = size;
        GenerationState.get().generateWorld();
        worldmapController.drawWorld(GameModel.get().world.worldModel.worldMap);
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
    
    private void toPositionSelection() {
        switchTo(locationSelectionStage);
    }
}
}