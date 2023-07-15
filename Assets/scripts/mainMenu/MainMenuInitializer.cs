using UnityEngine;

namespace mainMenu {
public class MainMenuInitializer : MonoBehaviour {
    public GameObject mainMenuStage;
    public GameObject worldGenStage;
    public GameObject locationSelectionStage;
    public GameObject preparationStage;
    public GameObject settingsStage;

    public void Start() {
        mainMenuStage.SetActive(true);
        worldGenStage.SetActive(false);
        locationSelectionStage.SetActive(false);
        preparationStage.SetActive(false);
        settingsStage.SetActive(false);
    }
}
}