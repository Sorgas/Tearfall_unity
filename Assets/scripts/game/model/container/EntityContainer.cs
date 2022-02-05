using System.Collections.Generic;
using Leopotam.Ecs;
using util.lang;

namespace game.model.container {
    public class EntityContainer : Singleton<EntityContainer> {
        public HashSet<EcsEntity> entities = new HashSet<EcsEntity>();

        public EntityContainer() { }

        public void add(EcsEntity entity) {
            entities.Add(entity);
        }

        public void remove(EcsEntity entity) {
            if (entities.Contains(entity)) entities.Remove(entity);
        }
    }
}