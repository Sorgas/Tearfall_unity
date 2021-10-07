using game.model.entity_selector;
using game.model.localmap;
using game.model.system.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;

namespace game.model {
    public class GameModel : Singleton<GameModel> {
        public World world;
        public LocalMap localMap;
        public EcsWorld ecsWorld;
        public EcsSystems systems; // model systems
        public EntitySelector selector = new EntitySelector(); // in-model representation of mouse
        public EntitySelectorSystem selectorSystem = new EntitySelectorSystem();
        private int count = 0;

        // init with entities generated on new game or loaded from savegame
        public void init() {
            Debug.Log("initializing model");
            initEcs();
            selectorSystem.selector = selector;
            selectorSystem.placeSelectorAtMapCenter();
            localMap.initAreas();
            Debug.Log("model initialized");
        }

        public void update() {
            // count++;
            // if(count >= 5) {
            //     count = 0;
                if (systems != null) systems.Run();
            // }
        }

        private void initEcs() {
            systems = new EcsSystems(ecsWorld);
            systems.Add(new MovementSystem())
            .Add(new TaskAssignmentSystem()) // finds or creates tasks for units
            .Add(new ActionSystem())
            .Add(new ActionPerformingSystem());
            systems.Init();
        }

        //get full world state from GenerationState or savefile
        public void setWorld(World world, EcsWorld ecsWorld) {
            this.world = world;
            this.localMap = world.localMap;
            this.ecsWorld = ecsWorld;
        }
    }
}