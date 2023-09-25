using System;
using game.model.component;
using game.model.component.building;
using game.model.component.item;
using game.model.component.task;
using game.model.container.item.finding;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.container.item {
// stores all items on the level. has separate storage classes for items on ground, stored in containers, equipped on units.
// transitions are made in actions. Does not consider items locking.
public class ItemContainer : LocalModelUpdateContainer {
    public readonly EquippedItemsManager equipped = new();
    public readonly OnMapItemsManager onMap;
    public readonly StoredItemsManager stored;

    public readonly AvailableItemsManager availableItemsManager = new();
    public readonly ItemFindingUtil findingUtil;
    public readonly CraftingItemFindingUtil craftingUtil;
    public readonly ItemTransitionUtil transition;

    public ItemContainer(LocalModel model) : base(model) {
        onMap = new(model, this);
        findingUtil = new(model, this);
        craftingUtil = new(model, this);
        transition = new(model, this);
        stored = new(model, this);
    }

    // checks that item's position is accessible from 'position'
    public bool itemAccessibleFromPosition(EcsEntity item, Vector3Int position) {
        Vector3Int targetPosition = getItemAccessPosition(item);
        return model.localMap.passageMap.tileIsAccessibleFromArea(targetPosition, position);
    }

    // should be never called for equipped items, items in equipped containers
    public Vector3Int getItemAccessPosition(EcsEntity item) {
        if (item.Has<PositionComponent>()) return item.pos(); // item is on the ground
        if (item.Has<ItemContainedComponent>()) {
            EcsEntity container = item.take<ItemContainedComponent>().container;
            if (container.Has<ItemComponent>()) {
                // containers with content should not be contained in another container
                return container.pos();
            }
            if (container.Has<BuildingComponent>()) {
                // building content 
                BuildingComponent building = container.take<BuildingComponent>();
                return building.type.getAccessByPositionAndOrientation(container.pos(), building.orientation);
            }
        }
        throw new ArgumentException("Unsupported item placement detected: " + item.name());
    }
}

public class ItemContainerPart : LocalModelUpdateContainer {
    protected readonly ItemContainer container;

    protected ItemContainerPart(LocalModel model, ItemContainer container) : base(model) {
        this.container = container;
    }
}
}