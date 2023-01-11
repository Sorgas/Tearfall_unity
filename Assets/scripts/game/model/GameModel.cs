using System.Collections.Generic;
using game.model.localmap;
using game.model.system;
using UnityEngine;
using util.lang;

namespace game.model {
    // global game model. Contains one WorldModel and multiple LocalMapModels. Passes updates to all models.
    public class GameModel : Singleton<GameModel> {
        public World world; // global game state
        public Dictionary<string, LocalModel> localMapModels = new();
        public LocalModel currentLocalModel;
        public GameTime time = new();

        public GameModelUpdateCounter counter = new();
        public GameModelUpdateController updateController;

        // public static EcsWorld ecsWorld => get()._ecsWorld;
        // public static LocalMap localMap => get().world.localMaps[get().localMapName];

        public void init(string name) {
            Debug.Log("initializing model for " + name);
            currentLocalModel = localMapModels[name];
            currentLocalModel.init();
            updateController = new(this);
            // selectorSystem.selector = selector;
            // selectorSystem.placeSelectorAtMapCenter();
            Debug.Log("model initialized");
        }


        // init with entities generated on new game or loaded from savegame
        public void update() {
            counter.update();
            time.update();
            foreach (LocalModel model in localMapModels.Values) {
                model.update();
            }
        }

        public string getDebugInfo() {
            return currentLocalModel.getDebugInfo();
        }

        public void addLocalModel(string name, LocalModel model) {
            world.localMapModels.Add(name, model);
            localMapModels.Add(name, model);
        }
    }
}