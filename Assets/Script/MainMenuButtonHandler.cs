using System.Collections;
using System.Collections.Generic;
using System.Transactions;
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
        buttons = new List<ButtonData>{
            new ButtonData("ContinueGameButton", KeyCode.C, toPreviousGame),
            new ButtonData("NewGameButton", KeyCode.E, toWorldGen),
            new ButtonData("LoadGameButton", KeyCode.S, toSaveGameSelection),
            new ButtonData("OptionsButton", KeyCode.D, toOptions),
            new ButtonData("QuitButton", KeyCode.Q, quitGame)
        };
        base.Start();
        Debug.Log("handler2 start");
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKey(KeyCode.Q)) { }
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
