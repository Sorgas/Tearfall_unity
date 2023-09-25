using game.model.component;
using game.model.component.item;
using game.model.localmap;
using Leopotam.Ecs;
using util.lang.extension;

namespace game.model.container.item {
// should be the only place where items transferred in or from container.
// should be the only place where ItemContainedComponent and ItemContainerComponent are modified.
public class StoredItemsManager : ItemContainerPart {
    public StoredItemsManager(LocalModel model, ItemContainer container) : base(model, container) { }

    public void addItemToContainer(EcsEntity item, EcsEntity containerEntity) {
        container.availableItemsManager.add(item);
        ref ItemContainerComponent containerComponent = ref containerEntity.takeRef<ItemContainerComponent>();
        containerComponent.addItem(item);
        item.Replace(new ItemContainedComponent { container = containerEntity });
    }

    public void removeItemFromContainer(EcsEntity item) {
        container.availableItemsManager.remove(item);
        ref ItemContainerComponent containerComponent = ref item.takeRef<ItemContainedComponent>().container.takeRef<ItemContainerComponent>();
        containerComponent.removeItem(item);
        item.Del<ItemContainedComponent>();
    }
}
}