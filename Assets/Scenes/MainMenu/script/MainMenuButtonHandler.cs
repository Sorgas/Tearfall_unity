using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// handler to be attached to parent object of button group
public class MainMenuButtonHandler : ButtonHandler {

    public void toWorldGen() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("WordGenLeftPanel").gameObject.SetActive(true);
    }

    public void toPreviousGame() {
        // todo
    }

    public void toSaveGameSelection() {
        // todo
    }

    public void toOptions() {
        // todo
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
