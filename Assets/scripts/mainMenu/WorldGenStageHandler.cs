using System;
using System.Collections.Generic;
using game.model;
using generation;
using mainMenu.location_selection_stage;
using mainMenu.worldmap_stage;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = System.Random;
using Slider = UnityEngine.UI.Slider;

namespace mainMenu {
// TODO add world config popup with more options
// Panel for generating worlds.
// Can generate world through GenerationState singleton, and display it with WorldMapHandler.
public class WorldGenStageHandler : GameMenuPanelHandler {
    // world gen parameters controls
    public InputField seedField;
    public Slider sizeSlider;
    public Slider perlinScaleSlider;
    public Slider deityCountSlider;
    public Slider civCountSlider;

    public Button randomizeSeedButton;
    public Button createButton;
    public Button backButton;
    public Button continueButton;

    [FormerlySerializedAs("worldMapHandler")] public WorldMapStageHandler worldMapStageHandler;
    public MainMenuStageHandler mainMenuStage;
    public LocationSelectionStageHandler locationSelectionStage;

    private Random random = new (DateTime.Now.Millisecond);
    
    public new void Start() {
        base.Start();
        // seedField.text = new Random().Next().ToString();
        // createButtonListener(randomizeSeedButton, KeyCode.R, randomizeSeed);
        createButtonListener(createButton, KeyCode.C, createWorld);
        createButtonListener(backButton, KeyCode.Q, backToMainMenu);
        createButtonListener(continueButton, KeyCode.V, toPositionSelection);
    }
    
    // TODO add ui spinner for world generation
    // Creates world and stores it in GameModel singleton. Can be invoked several times.
    public void createWorld() {
        int seed = random.Next(); // 0..maxInt
        int size = (int)sizeSlider.value * 50;
        GenerationState.get().worldGenerator.worldGenConfig.seed = seed;
        GenerationState.get().worldGenerator.worldGenConfig.size = size;
        // TODO add other parameters
        GenerationState.get().generateWorldModel();
        showWorld(GameModel.get().worldModel);
    }

    // Passes world to WorldMapHandler, activates Continue button.
    private void showWorld(WorldModel world) {
        worldMapStageHandler.gameObject.SetActive(true);
        worldMapStageHandler.setWorld(world);
        continueButton.gameObject.SetActive(true);
    }

    private void clear() {
        worldMapStageHandler.clear();
        continueButton.gameObject.SetActive(false);
    }

    private void backToMainMenu() {
        switchTo(mainMenuStage);
        clear();
    }

    private void toPositionSelection() {
        switchTo(locationSelectionStage);
        locationSelectionStage.init();
    }

    private void randomizeSeed() {
        seedField.text = random.Next().ToString();
    }
}
}