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

    public GameObject mainMenuStage;
    public GameObject preparationStage;

    private WorldGenSequence sequence = new WorldGenSequence();
    private WorldMap worldMap;


    protected override void initButtons() {
        buttons = new List<ButtonData> {
            new ButtonData("CreateButton", KeyCode.C, createWorld),
            new ButtonData("BackButton", KeyCode.Q, () => switchTo(mainMenuStage)),
            new ButtonData("ContinueButton", KeyCode.V, () => switchTo(preparationStage))
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
}