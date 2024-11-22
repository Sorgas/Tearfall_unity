using UnityEngine;

namespace mainMenu {
// Main menu scene consists of stages (screens). Player can navigate between them pressing buttons on active stage.
// This initializer activates stage with main menu.
public class MainMenuInitializer : MonoBehaviour {
    public GameObject mainMenuStage;
    public GameObject worldGenStage;
    public GameObject worldMapStage;
    public GameObject locationSelectionStage;
    public GameObject preparationStage;
    public GameObject optionsStage;
    public GameObject localGenStage;

    public void Start() {
        Application.targetFrameRate = 60;
        mainMenuStage.SetActive(true);
        worldGenStage.SetActive(false);
        worldMapStage.SetActive(false);
        locationSelectionStage.SetActive(false);
        preparationStage.SetActive(false); 
        optionsStage.SetActive(false);
        localGenStage.SetActive(false);
    }
}
}