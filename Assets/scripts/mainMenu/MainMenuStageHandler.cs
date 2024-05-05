using System.Collections.Generic;
using generation;
using mainMenu.location_selection_stage;
using UnityEngine;

namespace mainMenu {
// handler to be attached to parent object of button group
public class MainMenuStageHandler : StageHandler {
    public WorldGenStageHandler worldGenStage; 
    public OptionsStageHandler optionsStage; // TODO 
    public StageHandler gameLoadStage; // TODO 
    public LocalGenerationHandler localGenStage;
    public LocationSelectionStageHandler locationSelectStage;
    
    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("TestLevelButton", KeyCode.T, toTestLevel),
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

    private void toTestLevel() {
        new GenerationStateTestInitializer().initState(1);
        switchTo(localGenStage);
        localGenStage.startGeneration();
    }
    
    public void quitGame() {
        Application.Quit();
    }
}
}