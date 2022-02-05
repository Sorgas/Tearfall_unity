using game.model.component;
using game.model.component.item;
using Leopotam.Ecs;
using UnityEngine;

namespace game.view.system.item {
    // removes sprite go of items that have no position, as they are equipped on contained.
    public class ItemVisualRemoveSystem : IEcsRunSystem {
        public EcsFilter<ItemVisualComponent>.Exclude<PositionComponent> filter;
        
        public void Run() {
            foreach (var i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                ItemVisualComponent component = filter.Get1(i);
                Object.Destroy(component.go);
                entity.Del<ItemVisualComponent>();
            }
        }
    }
}