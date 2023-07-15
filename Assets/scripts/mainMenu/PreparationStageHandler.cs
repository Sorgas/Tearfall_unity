using System.Collections.Generic;
using generation;
using UnityEngine;

namespace mainMenu {
    public class PreparationStageHandler : StageHandler {
        public GameObject worldGenStage;
        public GameObject localGenStage;

        protected override List<ButtonData> getButtonsData() {
            return new List<ButtonData> {
                new("StartGameButton", KeyCode.E, startGame),
                new("BackButton", KeyCode.Q, back),
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