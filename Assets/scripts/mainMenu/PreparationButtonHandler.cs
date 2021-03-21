using System.Collections;
using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
using Assets.scripts.generation.localgen;
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
            GenerationState.get().generateLocalMap();
            GameModel.get().localMap = GenerationState.get().localMap;
            SceneManager.LoadScene("LocalWorldScene");
        }

        private void back() {
            switchTo(worldGenStage);
        }
    }
}