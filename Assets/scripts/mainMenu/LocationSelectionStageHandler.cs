using System.Collections.Generic;
using mainMenu.worldmap;
using UnityEngine;

namespace mainMenu {
// allow player to select position in the world 
public class LocationSelectionStageHandler : StageHandler {
    public WorldmapController worldmapController;

    public GameObject worldGenStage;
    public GameObject preparationStage;

    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("BackButton", KeyCode.Q, backToWorldgen),
            new("ContinueButton", KeyCode.V, toPreparationStage)
        };
    }

    public new void Start() {
        base.Start();
        // continueButton = buttons["ContinueButton"].gameObject;
        // EventSystem.current.SetSelectedGameObject(null, null);
        // TODO set position to center
        // worldmapController.
    }
    
    private void backToWorldgen() {
        switchTo(worldGenStage);
        worldmapController.clear();
        // TODO set position to center
    }
    
    private void toPreparationStage() {
        switchTo(preparationStage);
    }
}
}