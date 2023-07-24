using System;
using System.Collections.Generic;
using game.model;
using generation;
using mainMenu.location_selection_stage;
using mainMenu.worldmap_stage;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;
using Slider = UnityEngine.UI.Slider;

namespace mainMenu {
// TODO add world config popup with more options
public class WorldGenStageHandler : StageHandler {
    // controls
    public InputField seedField;
    public Slider sizeSlider;
    public Slider deityCountSlider;
    public Slider civCountSlider;

    public WorldmapController worldmapController;
    public MainMenuStageHandler mainMenuStage; // previous
    public LocationSelectionStageHandler locationSelectionStage; // next
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

    public new void Update() {
        base.Update();
    }

    public void initWorldMapController() {
        worldmapController.gameObject.SetActive(true);
        worldmapController.setWorldMap(GameModel.get().world.worldModel.worldMap);
        worldmapController.setWorldName(GameModel.get().world.name);
        worldmapController.setCameraToCenter();
        worldmapController.enablePointer();
        continueButton.gameObject.SetActive(true);
    }
    
    // invoked several times
    // TODO add ui spinner for world generation
    private void createWorld() {
        int seed = Convert.ToInt32(seedField.text);
        int size = (int)sizeSlider.value * 100;
        GenerationState.get().worldGenConfig.seed = seed;
        GenerationState.get().worldGenConfig.size = size;
        // TODO add other parameters
        GenerationState.get().generateWorld();
        initWorldMapController();
    }

    private void resetState() {
        worldmapController.clear();
        continueButton.gameObject.SetActive(false);
    }

    private void backToMainMenu() {
        switchTo(mainMenuStage);
        resetState();
    }

    private void toPositionSelection() {
        switchTo(locationSelectionStage);
        locationSelectionStage.init();
    }
}
}