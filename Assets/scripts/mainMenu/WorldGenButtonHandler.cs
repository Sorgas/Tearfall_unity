using System;
using System.Collections.Generic;
using Assets.Scenes.MainMenu.script;
using mainMenu.WorldGen;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class WorldGenButtonHandler : ButtonHandler {
    public Slider sizeSlider;

    public InputField seedField;
    public WorldmapController worldmapController;
    public Button continueButton;

    public GameObject mainMenuStage;
    public GameObject preparationStage;

    private WorldGenSequence sequence = new WorldGenSequence();
    private WorldMap worldMap;


    protected override void initButtons() {
        buttons = new List<ButtonData> {
            new ButtonData("CreateButton", KeyCode.C, createWorld),
            new ButtonData("BackButton", KeyCode.Q, backToMainMenu),
            new ButtonData("ContinueButton", KeyCode.V, () => switchTo(preparationStage))
        };
    }

    public void Start() {
        Debug.Log("handler start");
        base.Start();
        seedField.text = new Random().Next().ToString();
    }

    public void createWorld() {
        Debug.Log("creating world");
        int seed = Convert.ToInt32(seedField.text);
        int size = (int)sizeSlider.value * 100;
        Debug.Log("creating world " + seed + " " + size);
        WorldGenConfig config = new WorldGenConfig(seed, size);
        WorldGenContainer container = sequence.run(config);
        worldMap = container.createWorldMap(); // actual generation
        worldmapController.drawWorld(worldMap);
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
}