using System.Collections.Generic;
using UnityEngine;

namespace mainMenu {
// handler to be attached to parent object of button group
public class MainMenuStageHandler : StageHandler {
    public GameObject worldGenStage;
    public GameObject optionsStage;
    public GameObject gameLoadStage;

    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("ContinueGameButton", KeyCode.C, toPreviousGame),
            new("NewGameButton", KeyCode.E, () => switchTo(worldGenStage)),
            new("LoadGameButton", KeyCode.S, () => switchTo(gameLoadStage)),
            new("OptionsButton", KeyCode.D, () => switchTo(optionsStage)),
            new("QuitButton", KeyCode.Q, quitGame)
        };
    }

    public void toPreviousGame() {
        // todo
    }

    public void quitGame() {
        Application.Quit();
    }
}
}