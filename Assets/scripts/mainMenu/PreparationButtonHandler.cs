using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.scripts.mainMenu {
    public class PreparationButtonHandler : ButtonHandler {
        public GameObject worldGenStage;

        protected override void initButtons() {
            buttons = new List<ButtonData> {
                new ButtonData("StartGameButton", KeyCode.E, startGame),
                new ButtonData("BackButton", KeyCode.Q, back),
            };
        }

        // preparation to game model 

        private void startGame() {
            SceneManager.LoadScene("LocalWorldScene");
        }

        private void back() {
            switchTo(worldGenStage);
        }
    }
}