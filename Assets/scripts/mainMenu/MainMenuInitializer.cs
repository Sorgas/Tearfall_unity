using UnityEngine;

namespace mainMenu {
// Main menu scene consists of stages (screens). Player can navigate between them pressing buttons on active stage.
// This initializer activates stage with main menu.
public class MainMenuInitializer : MonoBehaviour {
    public GameObject mainMenuStage;
    public GameObject worldGenStage;
    public GameObject locationSelectionStage;
    public GameObject preparationStage;
    public GameObject optionsStage;
    public GameObject localGenStage;

    public void Start() {
        mainMenuStage.SetActive(true);
        worldGenStage.SetActive(false);
        locationSelectionStage.SetActive(false);
        preparationStage.SetActive(false); 
        optionsStage.SetActive(false);
        localGenStage.SetActive(false);
    }
}
}