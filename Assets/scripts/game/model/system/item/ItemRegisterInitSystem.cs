using game.model.component.item;
using game.model.container.item;
using Leopotam.Ecs;

namespace game.model.system.item {
    public class ItemRegisterInitSystem : IEcsInitSystem {
        public EcsFilter<ItemComponent> filter;

        public void Init() {
            ItemContainer container = GameModel.get().itemContainer;
            foreach (var i in filter) {
                container.registerItem(filter.GetEntity(i));
            }
        }
    }
}