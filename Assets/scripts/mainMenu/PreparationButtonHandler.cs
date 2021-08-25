using System.Collections.Generic;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using Assets.scripts.generation;
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

        // generate and add new settler
        public void addSettler() {
            // generate
            // add to state
            // add to view
            // unlock buttons
        }

        public void removeSettler() {

        }

        // preparation to game model 
        private void startGame() {
            for (int i = 0; i < 3; i++) {
                GenerationState.get().preparationState.settlers.Add(new SettlerData());
            }
            GenerationState.get().generateLocalMap();
            GameModel.get().localMap = GenerationState.get().world.localMap;
            SceneManager.LoadScene("LocalWorldScene");
        }

        private void back() {
            switchTo(worldGenStage);
        }

        private void spawnSettlers() {
            LocalMap localMap = GenerationState.get().localGenContainer.localMap;
            Vector2Int center = new Vector2Int(localMap.xSize / 2, localMap.ySize / 2);

            GenerationState.get().preparationState.settlers.ForEach(settler => {
                Vector3Int spawnPoint = new Vector3Int(Random.Range(center.x - 5, center.x + 5), Random.Range(center.x - 5, center.x + 5), 0);
            });
        }

        private Vector3Int getSpawnPosition(Vector2Int center, int range) {
            Vector3Int spawnPoint = new Vector3Int(Random.Range(center.x - 5, center.x + 5), Random.Range(center.x - 5, center.x + 5), 0);
            return spawnPoint;
        }
    }
}