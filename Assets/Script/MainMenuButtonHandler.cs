using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// handler to be attached to parent object of button group
public class MainMenuButtonHandler : ButtonHandler {
    
    Button continueButton;
    Button newGameButton;
    Button loadGameButton;
    Button optionsButton;
    Button quitButton;

    // Start is called before the first frame update
    void Start() {
        continueButton = transform.Find("ContinueGameButton").GetComponent<Button>();
        newGameButton = transform.Find("NewGameButton").GetComponent<Button>();
        loadGameButton = transform.Find("LoadGameButton").GetComponent<Button>();
        optionsButton = transform.Find("OptionsButton").GetComponent<Button>();
        quitButton = transform.Find("QuitButton").GetComponent<Button>();

        continueButton.onClick.AddListener(toPreviousGame);
        newGameButton.onClick.AddListener(toWorldGen);
        loadGameButton.onClick.AddListener(toSaveGameSelection);
        optionsButton.onClick.AddListener(toOptions);
        quitButton.onClick.AddListener(quitGame);

        // 1 if there is savegame, enable continue button
        continueButton.gameObject.SetActive(false);
        // 3 if there is savegame, enable load button
        loadGameButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKey(KeyCode.Q))
    }

    public void toWorldGen() {
        gameObject.SetActive(false);
        gameObject.transform.parent.Find("WorldGenLeftPanel").gameObject.SetActive(true);
    }

    public void toPreviousGame() {
        
    }

    public void toSaveGameSelection() {

    }

    public void toOptions() {

    }

    public void quitGame() {
        Debug.Log("press");
        Application.Quit();
    }
}
