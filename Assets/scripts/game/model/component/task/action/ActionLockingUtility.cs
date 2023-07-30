using System;
using System.Collections.Generic;
using game.model.util;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action {
    // part of Action. Provides locking functionality for items and zone tiles.
    public static class ActionLockingUtility {

        public static void lockEntities(List<EcsEntity> entities, EcsEntity task) => entities.ForEach(entity => lockEntity(entity, task));
        public static void unlockEntities(List<EcsEntity> entities, EcsEntity task) => entities.ForEach(entity => unlockEntity(entity, task));

        // locks item to task of this action. Item can be locked only to one task. 
        // Items are unlocked when task ends, see TaskCompletionSystem.
        public static void lockEntity(EcsEntity entity, EcsEntity task) {
            if (!entityCanBeLocked(entity, task)) throw new ArgumentException("Cannot lock entity: locked to another task");
            if (entity.Has<LockedComponent>()) return; // item locked to this task
            entity.Replace(new LockedComponent { task = task });
            task.Get<TaskLockedEntitiesComponent>().entities.Add(entity); // can create component
        }

        public static void unlockEntity(EcsEntity entity, EcsEntity task) {
            if(!entityCanBeUnlocked(entity, task)) throw new ArgumentException("Cannot unlock entity: locked to another task");
            if (!entity.Has<LockedComponent>()) return; // already not locked
            entity.Del<LockedComponent>();
            task.Get<TaskLockedEntitiesComponent>().entities.Remove(new EcsEntity()); // can create component
        }

        public static bool entityCanBeLocked(EcsEntity entity, EcsEntity task) => 
            !entity.Has<LockedComponent>() || entity.take<LockedComponent>().task == task;

        public static bool entityCanBeUnlocked(EcsEntity entity, EcsEntity task) => 
            !entity.Has<LockedComponent>() || entity.take<LockedComponent>().task == task;
    }
}