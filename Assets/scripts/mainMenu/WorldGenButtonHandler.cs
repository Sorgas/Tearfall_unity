using System;
using System.Collections.Generic;
using System.Transactions;
using Assets.Scenes.MainMenu.script;
using mainMenu.WorldGen;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class WorldGenButtonHandler : ButtonHandler {
    public Slider sizeSlider;

    public InputField seedField;
    public WorldMapDrawer drawer;
    private WorldGenSequence sequence = new WorldGenSequence();
    private WorldMap worldMap;

    protected override void initButtons() {
        buttons = new List<ButtonData> {
            new ButtonData("CreateButton", KeyCode.C, createWorld),
            new ButtonData("BackButton", KeyCode.Q, toMainMenu)
        };
    }

    public void Start() {
        base.Start();
        seedField.text = new Random().Next().ToString();
    }
    
    public void createWorld() {
        Debug.Log("world");
        int seed = Convert.ToInt32(seedField.text);
        int size = (int) sizeSlider.value * 100;
        WorldGenConfig config = new WorldGenConfig(seed, size);
        WorldGenContainer container = sequence.run(config);
        worldMap = container.createWorldMap();
        drawer.drawWorld(worldMap);
    }

    public void toMainMenu() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainMenuButtons").gameObject.SetActive(true);
    }
}