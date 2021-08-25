using UnityEngine;
using System.Collections.Generic;
using Assets.scripts.game.model.system;
using System;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.lang;
using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.game.model.entity_selector;

namespace Assets.scripts.game.model {
    public class GameModel : Singleton<GameModel> {
        public World world;
        public LocalMap localMap;
        public EcsWorld ecsWorld;
        public EcsSystems systems; // model systems
        public EntitySelector selector = new EntitySelector(); // in-model representation of mouse
        public EntitySelectorSystem selectorSystem = new EntitySelectorSystem();

        private Dictionary<Type, ModelComponent> components = new Dictionary<Type, ModelComponent>();
        private List<Updatable> updatableComponents; // not all components are Updatable
        public float id = Time.realtimeSinceStartup;

        // init with entities generated on new game or loaded from savegame
        public void init(EcsWorld ecsWorld) {
            Debug.Log("initializing model");
            initEcs(ecsWorld);
            selectorSystem.selector = selector;
            selectorSystem.placeSelectorAtMapCenter();
            Debug.Log("model initialized");
        }

        public void update() {
            systems.Run();
        }

        public static T get<T>() where T : ModelComponent {
            ModelComponent value = null;
            get().components.TryGetValue(typeof(T), out value);
            return (T)value;
        }

        public static Optional<T> optional<T>() where T : ModelComponent {
            return new Optional<T>(get<T>());
        }

        private void initEcs(EcsWorld ecsWorld) {
            this.ecsWorld = ecsWorld;
            systems = new EcsSystems(ecsWorld);
            // systems.Add(new TestSystem());
            systems.Init();
        }
    }
}