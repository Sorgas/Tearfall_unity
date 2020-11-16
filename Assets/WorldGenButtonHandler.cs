using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGenButtonHandler : ButtonHandler
{
    protected override void initButtons() {
        buttons = new List<ButtonData>{
            new ButtonData("CreateButton", KeyCode.C, createWorld),
            new ButtonData("BackButton", KeyCode.Q, toMainMenu)
        };
        Debug.Log("handler2 start");
    }

    public void createWorld() {

    }

    public void toMainMenu() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("MainMenuButtons").gameObject.SetActive(true);
    }
}
