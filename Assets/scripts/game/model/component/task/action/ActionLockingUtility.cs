using System;
using System.Collections.Generic;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.component.task.action {
    // part of Action. Provides locking functionality for items and zone tiles.
    public static class ActionLockingUtility {

        public static void lockEntities(List<EcsEntity> items, EcsEntity task) => items.ForEach(entity => lockEntity(entity, task));

        // locks item to task of this action. Item can be locked only to one task. 
        // Items are unlocked when task ends, see TaskCompletionSystem.
        public static void lockEntity(EcsEntity item, EcsEntity task) {
            if (!itemCanBeLocked(item, task)) throw new ArgumentException("Cannot lock item. Item locked to another task");
            ref TaskLockedItemsComponent lockedItems = ref task.Get<TaskLockedItemsComponent>(); // can create component
            if (item.Has<LockedComponent>()) return; // item locked to this task
            item.Replace(new LockedComponent { task = task });
            lockedItems.lockedItems.Add(item);
        }

        public static void lockZoneTile(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            if (tracking.locked.ContainsKey(tile)) {
                if (tracking.locked[tile] != task) throw new ArgumentException("Cannot lock tile. Tile locked to another task");
                // already locked to this task
            } else {
                tracking.locked.Add(tile, task);
            }
        }

        public static bool itemCanBeLocked(EcsEntity item, EcsEntity task) {
            return !item.Has<LockedComponent>() || item.take<LockedComponent>().task == task;
        }

        public static void unlockZoneTile(EcsEntity zone, Vector3Int tile, EcsEntity task) {
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            if (!tracking.locked.ContainsKey(tile)) return; // already unlocked
            if (tracking.locked[tile] != task) throw new ArgumentException("Cannot unlock tile. Tile locked to another task");
            tracking.locked.Remove(tile);
        }

        // public static bool tileCanBeLocked(EcsEntity zone, Vector3Int tile, EcsEntity task) {
        //     ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
        //     return !tracking.locked.ContainsKey(tile) || tracking.locked[tile] == task;
        // }
    }
}