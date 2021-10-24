using Leopotam.Ecs;

namespace util.lang.extension {
    public static class EcsEntityExtension {
        public static T? get<T >(this EcsEntity entity) where T : struct {
            return entity.Has<T>() ? (T?)entity.Get<T>() : null;
        }
    }
}