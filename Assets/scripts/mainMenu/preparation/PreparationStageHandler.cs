using System.Collections.Generic;
using generation;
using generation.unit;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace mainMenu.preparation {
    public class PreparationStageHandler : StageHandler {
        public WorldGenStageHandler worldGenStage;
        public LocalGenerationHandler localGenStage;
        public RectTransform characterListContent;
        public UnitDetailsHandler unitDetailsHandler;
        public Button addSettlerButton;
        public InputField settlementName;
        public TextMeshProUGUI pointsText;

        private SettlerDataGenerator settlerDataGenerator = new();
        
        protected override List<ButtonData> getButtonsData() {
            return new List<ButtonData> {
                new("StartGameButton", KeyCode.E, startGame),
                new("BackButton", KeyCode.Q, back),
            };
        }

        // generate and add new settler
        public void addSettler() {
            SettlerData data = settlerDataGenerator.generate();
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
                settler.type = "human";
                GenerationState.get().preparationState.settlers.Add(settler);
            }
            switchTo(localGenStage);
            localGenStage.startGeneration();
        }

        private void back() {
            switchTo(worldGenStage);
        }
    }
}