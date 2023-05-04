using System.Collections.Generic;
using game.model.container;
using game.model.localmap;
using game.model.system;
using UnityEngine;
using util.lang;

namespace game.model {
    // global game model. Contains one WorldModel and multiple LocalMapModels.
    // Passes updates to all models.
    public class GameModel : Singleton<GameModel> {
        public World world; // global game state
        private readonly Dictionary<string, LocalModel> localMapModels = new();
        public LocalModel currentLocalModel;
        public readonly GameTime gameTime = new();
        public readonly EcsGlobalSharedData globalSharedData = new();
        public GameModelUpdateController updateController;
        public readonly GameModelUpdateCounter counter = new();
        public readonly TechnologyContainer technologyContainer = new();

        public void init(string name) {
            Debug.Log("initializing model for " + name);
            currentLocalModel = localMapModels[name];
            currentLocalModel.init();
            updateController = new();
            Debug.Log("model initialized");
        }

        // TODO remove
        public static LocalModel local() => get().currentLocalModel;

        public void update() {
            int ticks = updateController.getTicksForUpdate();
            globalSharedData.set(ticks); // systems consume ticks from here
            counter.update(ticks); // debug thing
            gameTime.update(ticks); // calendar
            foreach (LocalModel model in localMapModels.Values) {
                model.update(ticks); // ticks passed with globalSharedData
            }
        }
        
        public void addLocalModel(string name, LocalModel model) {
            world.localMapModels.Add(name, model);
            localMapModels.Add(name, model);
        }
    }
    
    // injected to all systems of GameModel
    public class EcsGlobalSharedData {
        public int ticks => value; // number of ticks to calculate
        private int value;
        
        public void set(int value) {
            this.value = value;
        }
    }
}