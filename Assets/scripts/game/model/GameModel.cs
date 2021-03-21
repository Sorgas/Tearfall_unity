using UnityEngine;
using System.Collections.Generic;
using Assets.scripts.game.model.system;
using System;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.lang;

namespace Assets.scripts.game.model {
    public class GameModel : Singleton<GameModel> {
        public World world;
        public LocalMap localMap;
        public Vector2Int position; // position of player settlement in the world.
        private Dictionary<Type, ModelComponent> components = new Dictionary<Type, ModelComponent>();
        private List<Updatable> updatableComponents; // not all components are Updatable
                                                     //public const GameTime gameTime = new GameTime();

        public static T get<T>() where T : ModelComponent {
            ModelComponent value = null;
            get().components.TryGetValue(typeof(T), out value);
            return (T)value;
        }
        public static Optional<T> optional<T>() where T : ModelComponent {
            return new Optional<T>(get<T>());
        }

        //public <T extends ModelComponent> void put(T object) {
        //    components.put(object.getClass(), object);
        //    if (object instanceof Updatable) updatableComponents.add((Updatable)object);
        //}

        ///**
        // * Inits all stored components that are {@link Initable}.
        // * Used for components binding.
        // */
        //@Override
        //public void init() {
        //    components.values().stream()
        //            .filter(component->component instanceof Initable)
        //            .map(component-> (Initable) component)
        //            .forEach(Initable::init);
        //    gameTime.initTimer();
        //}

        //@Override
        //public void update(TimeUnitEnum unit) {
        //    updatableComponents.forEach(component->component.update(unit));
        //    if (unit == TimeUnitEnum.TICK) GameMvc.view().overlayStage.update(); // count model updates
        //}

    }
}