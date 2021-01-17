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
    public WorldMapDrawer drawer;
    public Button continueButton;

    private WorldGenSequence sequence = new WorldGenSequence();
    private WorldMap worldMap;


    protected override void initButtons() {
        buttons = new List<ButtonData> {
            new ButtonData("CreateButton", KeyCode.C, createWorld),
            new ButtonData("BackButton", KeyCode.Q, toMainMenu),
            new ButtonData("ContinueButton", KeyCode.V, toGamePreparation)
        };
    }

    new public void Start() {
        base.Start();
        seedField.text = new Random().Next().ToString();
    }
    
    public void createWorld() {
        int seed = Convert.ToInt32(seedField.text);
        int size = (int) sizeSlider.value * 100;
        Debug.Log("creating world " + seed + " " + size);
        WorldGenConfig config = new WorldGenConfig(seed, size);
        WorldGenContainer container = sequence.run(config);
        worldMap = container.createWorldMap(); // actual generation
        drawer.drawWorld(worldMap);

    }

    public void toMainMenu() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainMenuStage").gameObject.SetActive(true);
    }

    public void toGamePreparation() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("PreparationStage").gameObject.SetActive(true);
    }
}