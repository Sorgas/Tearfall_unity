using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WorldGenButtonHandler : ButtonHandler {

    public Slider slider;
    public WorldMapDrawer drawer;

    protected override void initButtons() {
        buttons = new List<ButtonData>{
            new ButtonData("CreateButton", KeyCode.C, createWorld),
            new ButtonData("BackButton", KeyCode.Q, toMainMenu)
        };
    }

    public void createWorld() {
        int size = (int)slider.value;
        
        //    drawer.drawWorld();

    }

    public void toMainMenu() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainMenuButtons").gameObject.SetActive(true);
    }
}
