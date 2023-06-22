using System;
using System.Collections.Generic;
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
        
        public static void lockZoneTile(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            if (!tileCanBeLocked(zone, tile, task)) {
                if (tracking.locked[tile] != task) throw new ArgumentException("Cannot lock tile. Tile locked to another task");
                // already locked to this task
            } else {
                if (!tracking.locked.ContainsKey(tile)) tracking.locked.Add(tile, task);
            }
        }

        public static bool entityCanBeLocked(EcsEntity entity, EcsEntity task) => 
            !entity.Has<LockedComponent>() || entity.take<LockedComponent>().task == task;

        public static bool entityCanBeUnlocked(EcsEntity entity, EcsEntity task) => 
            !entity.Has<LockedComponent>() || entity.take<LockedComponent>().task == task;

        public static void unlockZoneTile(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            if (!tracking.locked.ContainsKey(tile)) return; // already unlocked
            if (tracking.locked[tile] != task) throw new ArgumentException("Cannot unlock tile. Tile locked to another task");
            tracking.locked.Remove(tile);
        }

        // tile is not locked or already locked to given task
        public static bool tileCanBeLocked(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            return !tracking.locked.ContainsKey(tile) || tracking.locked[tile] == task;
        }
    }
}