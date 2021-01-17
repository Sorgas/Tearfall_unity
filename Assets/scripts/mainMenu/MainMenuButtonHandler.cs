using System.Collections.Generic;
using UnityEngine;

// handler to be attached to parent object of button group
public class MainMenuButtonHandler : ButtonHandler {
    public GameObject worldGenStage;
    public GameObject optionsStage;
    public GameObject gameLoadStage;


    public void toWorldGen() {
        switchTo(worldGenStage);
    }

    public void toPreviousGame() {
        // todo
    }

    public void toSaveGameSelection() {
        // todo
    }

    public void toOptions() {
        switchTo(optionsStage);
    }

    public void quitGame() {
        Debug.Log("press");
        Application.Quit();
    }

    protected override void initButtons() {
        buttons = new List<ButtonData>{
            new ButtonData("ContinueGameButton", KeyCode.C, toPreviousGame),
            new ButtonData("NewGameButton", KeyCode.E, toWorldGen),
            new ButtonData("LoadGameButton", KeyCode.S, toSaveGameSelection),
            new ButtonData("OptionsButton", KeyCode.D, toOptions),
            new ButtonData("QuitButton", KeyCode.Q, quitGame)
        };
        Debug.Log("handler2 start");
    }
}
