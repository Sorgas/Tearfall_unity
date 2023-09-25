using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using game.model.container.item;
using Leopotam.Ecs;
using MoreLinq;
using types.action;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action {
// base action for building constructions and buildings. Defines bringing items to designation, consuming items.
// If there are other items on building bounds, they are instantly moved to adjacent position. 
// If there are other unit on building bounds, builder will wait for some time 
public abstract class GenericBuildingAction : Action {
    protected EcsEntity designation;
    protected readonly GenericBuildingOrder order;
    protected IntBounds3 bounds; // should be precalculated in subclasses
    
    protected GenericBuildingAction(EcsEntity designation, GenericBuildingOrder order) : base(new BuildingConstructionActionTarget(order)) {
        this.designation = designation;
        this.order = order;
        name = "generic building action";

        assignmentCondition = (unit) => OK;

        startCondition = () => {
            if (!checkItemsInOrder()) return FAIL;
            ActionCheckingEnum itemsInContainer = checkItemsInContainer();
            if (itemsInContainer == NEW) return NEW;
            if (itemsInContainer == FAIL) return failAction("items not found");
            return checkClearingSite(bounds);
        };
    }

    // checks if order has correct amount of correct and available items. If not, clears current items and finds new ones. 
    private bool checkItemsInOrder() {
        if (!model.itemContainer.craftingUtil.buildingOrderValid(order, designation, designation.pos())) {
            unlockEntities(order.items);
            order.items.Clear();
            List<EcsEntity> items = model.itemContainer.craftingUtil.findItemsForBuildingOrder(order, designation.pos());
            if (items == null || items.Count < order.amount) return false; // cannot find new items
            lockEntities(items);
            order.items.AddRange(items);
        }
        return true;
    }

    // checks that items listed in order are stored inside designation container. Creates actions to bring if needed.
    private ActionCheckingEnum checkItemsInContainer() {
        ItemContainerComponent container = designation.take<ItemContainerComponent>();
        foreach (var item in order.items) {
            if (!container.items.Contains(item)) {
                return addPreAction(new PutItemToContainerAction(designation, item));
            }
        }
        return OK;
    }

    // finds item that can be removed from construction site and not locked to other task.
    // For first found item creates sub-action to move it to nearby off-site tile
    private ActionCheckingEnum checkClearingSite(IntBounds3 bounds) {
        ItemContainer container = model.itemContainer;
        foreach (var position in bounds.toList()) {
            List<EcsEntity> itemsOnTile = container.onMap.itemsOnMap.get(position);
            if (itemsOnTile.Count > 0) {
                Vector3Int offSitePosition = findNearestOutsidePosition(position);
                foreach (EcsEntity item in itemsOnTile) {
                    if (entityCanBeLocked(item)) { // ignores items locked by other tasks
                        if (offSitePosition == Vector3Int.back) return FAIL; // no position to remove
                        lockEntity(item);
                        return addPreAction(new PutItemToPositionAction(item, offSitePosition));
                    }
                }
            }
        }
        return OK;
    }

    // drops item from designation container to ground
    private ActionCheckingEnum failAction(string message) {
        log("failing action " + message);
        dropFromDesignation(order.position);
        return FAIL;
    }

    // finds non construction site position nearest to given position 
    private Vector3Int findNearestOutsidePosition(Vector3Int referencePosition) {
        List<Vector3Int> acceptablePositions = target.getAcceptablePositions(model);
        if (acceptablePositions.Count == 0) return Vector3Int.back; // not found
        int performerArea = model.localMap.passageMap.area.get(performer.pos());
        return acceptablePositions
            .Where(position => model.localMap.passageMap.area.get(position) == performerArea)
            .MinBy(position => (position - referencePosition).sqrMagnitude);
    }

    // destroys items of order stored in designation container. Items that do not belong to order are dropped at performer's position
    protected void consumeItems() {
        foreach (var orderItem in order.items) {
            model.itemContainer.transition.destroyItem(orderItem);
        }
        dropFromDesignation(performer.pos());
    }

    // moves all items from designation item container to given position on map 
    private void dropFromDesignation(Vector3Int targetPosition) {
        foreach (var item in getDesignationContainer().items.ToList()) {
            model.itemContainer.transition.fromContainerToGround(item, designation, targetPosition);
        }
    }

    protected ref ItemContainerComponent getDesignationContainer() => ref designation.takeRef<ItemContainerComponent>();
}
}