using System;
using game.model.component;
using Leopotam.Ecs;
using UnityEngine;

namespace util.lang.extension {
    public static class EcsEntityExtension {
        public static T? optional<T>(this EcsEntity entity) where T : struct {
            return entity.Has<T>() ? (T?)entity.Get<T>() : null;
        }
        
        public static T take<T>(this EcsEntity entity) where T : struct {
            if (entity.Has<T>()) {
                return entity.Get<T>();
            }
            throw new EcsException("Entity does not have component " + typeof(T).Name);
        }
        
        public static Vector3Int pos(this EcsEntity entity) {
            if(entity.hasPos()) return entity.Get<PositionComponent>().position;
            throw new ArgumentException("entity has no PositionComponent!");
        }

        public static bool hasPos(this EcsEntity entity) {
            return entity.Has<PositionComponent>();
        }
    }

    public class EcsException : Exception {
        public EcsException(string message) : base(message) {}
    }
}