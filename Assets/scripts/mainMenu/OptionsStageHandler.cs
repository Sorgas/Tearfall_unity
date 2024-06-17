using System.Collections.Generic;
using UnityEngine;

namespace mainMenu {
public class OptionsStageHandler : GameMenuPanelHandler {
    public MainMenuStageHandler mainMenuStage;

    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("ApplyButton", KeyCode.E, () => {
                // todo apply changes
                switchTo(mainMenuStage);
            }),
            new("BackButton", KeyCode.Q, () => switchTo(mainMenuStage))
        };
    }
}
}