using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.scripts.game.model.system;
using System;

namespace Assets.scripts.game {
    public class GameModel {
        private Dictionary<Type, ModelComponent> components = new Dictionary<Type, ModelComponent>();
        private List<Updatable> updatableComponents; // not all components are Updatable
        //public const GameTime gameTime = new GameTime();

        //public <T extends ModelComponent> T get(Class<T> type) {
        //    return (T)components.get(type);
        //}
        //public <T extends ModelComponent> Optional<T> optional(Class<T> type) {
        //    return Optional.ofNullable(get(type));
        //}

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