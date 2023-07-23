using System.Collections.Generic;
using mainMenu.location_selection_stage;
using UnityEngine;

namespace mainMenu {
// handler to be attached to parent object of button group
public class MainMenuStageHandler : StageHandler {
    public WorldGenStageHandler worldGenStage; 
    public OptionsStageHandler optionsStage; // TODO 
    public StageHandler gameLoadStage; // TODO 
    public LocationSelectionStageHandler locationSelectStage;
    
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