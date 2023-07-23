using System.Collections.Generic;
using game.model;
using mainMenu.worldmap_stage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using util.geometry;

namespace mainMenu.location_selection_stage {
// allow player to select position in the world 
// when player moves mouse over world map, updates text stats of world tile
// when player clicks over world map, selects position, TODO generates local map, shows image of local map 
public class LocationSelectionStageHandler : StageHandler {
    public WorldmapController worldmapController;
    public Image localMapImage; 
    public TextMeshProUGUI text;
    public WorldGenStageHandler worldGenStage;
    public PreparationStageHandler preparationStage;

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
        // worldmapController.
    }

    public void Update() {
        if (worldmapController.pointerMoved) {
            updateText();
            worldmapController.pointerMoved = false;
        }
        if (worldmapController.locationChanged) {
            // TODO regenerate local map
            worldmapController.locationChanged = false;
        }
    }
    
    public void init() {
        worldmapController.drawWorld(GameModel.get().world.worldModel.worldMap);
        worldmapController.enablePointer();
        worldmapController.enableLocationSelector();
        worldmapController.setCameraToCenter();
        // TODO generate local map, generate image from local map.
    }
    
    private void updateText() {
        WorldMap worldMap = GameModel.get().world.worldModel.worldMap;
        IntVector2 cacheVector = new();
        cacheVector.set(worldmapController.getPointerPosition());
        text.text = cacheVector + " " + worldMap.elevation[cacheVector.x, cacheVector.y];
    }
    
    private void backToWorldgen() {
        switchTo(worldGenStage);
        worldGenStage.init();
        worldmapController.clear();
    }
    
    private void toPreparationStage() {
        switchTo(preparationStage);
    }
}
}