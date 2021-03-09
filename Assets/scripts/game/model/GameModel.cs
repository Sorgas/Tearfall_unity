using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts.game.model.system;
using System;
using Assets.scripts.mainMenu;
using Assets.scripts.game.model.localmap;
using Assets.scripts.util.lang;

namespace Assets.scripts.game.model {
    public class GameModel {
        public static GameModel instance;
        public WorldMap worldMap;
        public LocalMap localMap;
        public static readonly object qwer = new object();
        private Dictionary<Type, ModelComponent> components = new Dictionary<Type, ModelComponent>();
        private List<Updatable> updatableComponents; // not all components are Updatable
                                                     //public const GameTime gameTime = new GameTime();

        public static GameModel get() {
            if (instance == null) {
                lock (qwer) {
                    instance = new GameModel();
                }
            }
            return instance;
        }

        public static T get<T>() where T : ModelComponent {
            return (T)get().components[typeof(T)];
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