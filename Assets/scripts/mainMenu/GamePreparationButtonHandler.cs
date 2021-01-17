using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.mainMenu {
    public class GamePreparationButtonHandler : ButtonHandler {
        protected override void initButtons() {
            buttons = new List<ButtonData> {
                new ButtonData("StartGameButton", KeyCode.E, startGame),
                new ButtonData("BackButton", KeyCode.Q, back),
            };
        }

        private void startGame() {

        }

        private void back() {

        }
    }
}