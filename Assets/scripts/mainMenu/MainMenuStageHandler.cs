using System.Collections.Generic;
using System.Threading.Tasks;
using generation;
using mainMenu.location_selection_stage;
using mainMenu.worldmap_stage;
using UnityEngine;

namespace mainMenu {
// handler to be attached to parent object of button group

public class MainMenuStageHandler : GameMenuPanelHandler {
    public WorldGenStageHandler worldGenStage;
    public OptionsStageHandler optionsStage;
    public GameMenuPanelHandler gameLoadStage; 
    public LocalGenerationHandler localGenStage;
    public LocationSelectionStageHandler locationSelectStage;
    public WorldMapStageHandler worldMapStageHandler;
    
    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("TestLevelButton", KeyCode.T, () => toTestLevel(false)),
            new("TestLevelCombatButton", KeyCode.P, () => toTestLevel(true)),
            new("ContinueGameButton", KeyCode.C, toPreviousGame),
            new("NewGameButton", KeyCode.E, toNewGameWorldGen),
            new("LoadGameButton", KeyCode.S, () => switchTo(gameLoadStage)),
            new("OptionsButton", KeyCode.D, () => switchTo(optionsStage)),
            new("QuitButton", KeyCode.Q, quitGame)
        };
    }

    private void toTestLevel(bool combat) {
        gameObject.SetActive(false);
        if (combat) {
            new GenerationStateTestInitializer().initCombatState();
        } else {
            new GenerationStateTestInitializer().initState();
        }
        Task.Run(() => GenerationState.get().generateLocalMap("main"));
        localGenStage.gameObject.SetActive(true);
    }

    private void toNewGameWorldGen() {
        gameObject.SetActive(false);
        worldGenStage.gameObject.SetActive(true);
        worldMapStageHandler.gameObject.SetActive(true);
    }
    
    public void quitGame() {
        Application.Quit();
    }
    
    // TODO
    public void toPreviousGame() { }
}
}