using System.Collections.Generic;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
using Tearfall_unity.Assets.scripts.generation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.scripts.mainMenu {
    public class PreparationButtonHandler : ButtonHandler {
        public GameObject worldGenStage;
        public GameObject localGenStage;

        protected override void initButtons() {
            buttons = new List<ButtonData> {
                new ButtonData("StartGameButton", KeyCode.E, startGame),
                new ButtonData("BackButton", KeyCode.Q, back),
            };
        }

        // generate and add new settler
        public void addSettler() {
            // generate // add to state // add to view // update buttons
        }

        public void removeSettler() {
            // remove from state and view // update buttons
        }

        // preparation to game model 
        private void startGame() {
            for (int i = 0; i < 1; i++) {
                SettlerData settler = new SettlerData();
                settler.name = "qwer" + i;
                settler.age = 30;
                GenerationState.get().preparationState.settlers.Add(settler);
            }
            switchTo(localGenStage);
        }

        private void back() {
            switchTo(worldGenStage);
        }
    }
}