using System.Collections.Generic;
using game.model;
using generation;
using mainMenu.preparation;
using mainMenu.worldmap_stage;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Button continueButton;

    protected override List<ButtonData> getButtonsData() {
        return new List<ButtonData> {
            new("BackButton", KeyCode.Q, backToWorldgen),
            new("ContinueButton", KeyCode.V, toPreparationStage)
        };
    }

    public new void Start() {
        base.Start();
        continueButton.gameObject.SetActive(false); // disabled, until player clicks on worldmap
    }

    public void Update() {
        if (worldmapController.pointerMoved) {
            updateText();
            worldmapController.pointerMoved = false;
        }
        if (worldmapController.locationChanged) {
            // TODO regenerate local map
            worldmapController.locationChanged = false;
            Vector3 locationPosition = worldmapController.getLocationPosition();
            PreparationState.get().location = new Vector2Int((int)locationPosition.x, (int)locationPosition.y);
            // TODO draw local map to image
            continueButton.gameObject.SetActive(true); // TODO validate location (not sea, not mountain)
        }
    }

    public void init() {
        worldmapController.setWorldMap(GameModel.get().world.worldModel.worldMap);
        worldmapController.enablePointer();
        worldmapController.enableLocationSelector();
        worldmapController.setCameraToCenter();
        // TODO generate local map, generate image from local map.
    }

    private void updateText() {
        WorldMap worldMap = GameModel.get().world.worldModel.worldMap;
        Vector3 pointerPosition = worldmapController.getPointerPosition();
        Vector2Int cacheVector = new((int)pointerPosition.x, (int)pointerPosition.y);
        text.text = cacheVector + " " + worldMap.elevation[cacheVector.x, cacheVector.y];
    }

    private void backToWorldgen() {
        switchTo(worldGenStage);
        worldGenStage.initWorldMapController();
        worldmapController.clear();
    }

    private void toPreparationStage() {
        switchTo(preparationStage);
        worldmapController.gameObject.SetActive(false);
    }
}
}