using System.Collections.Generic;
using UnityEngine;

namespace mainMenu
{
// handler to be attached to parent object of button group
public class MainMenuButtonHandler : ButtonHandler {
    public GameObject worldGenStage;
    public GameObject optionsStage;
    public GameObject gameLoadStage;

    public void toPreviousGame() {
        // todo
    }

    public void toSaveGameSelection() {
        // todo
    }

    public void quitGame() {
        Application.Quit();
    }

    protected override void initButtons() {
        buttons = new List<ButtonData>{
            new ButtonData("ContinueGameButton", KeyCode.C, toPreviousGame),
            new ButtonData("NewGameButton", KeyCode.E, ()=> switchTo(worldGenStage)),
            new ButtonData("LoadGameButton", KeyCode.S, toSaveGameSelection),
            new ButtonData("OptionsButton", KeyCode.D, () => switchTo(optionsStage)),
            new ButtonData("QuitButton", KeyCode.Q, quitGame)
        };
        Debug.Log("handler2 start");
    }
}
}