using System.Collections.Generic;
using System.Linq;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using game.model.container.item;
using game.model.localmap;
using game.model.localmap.passage;
using Leopotam.Ecs;
using MoreLinq;
using types;
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
    private IntBounds3 bounds;
    public bool offSitePositionOk = false;

    protected GenericBuildingAction(EcsEntity designation, GenericBuildingOrder order) : base(new BuildingConstructionActionTarget(order)) {
        this.designation = designation;
        this.order = order;
        name = "generic building action";
        bounds = getBuildingBounds(order);

        assignmentCondition = (unit) => OK;

        startCondition = () => {
            ActionCheckingEnum status = checkItemsInContainer();
            if (status == NEW) return NEW;
            if (status == FAIL) return failAction("items not found");
            return checkClearingSite(bounds);
        };
    }

    // items for building should be brought into designation container. Creates actions to bring if possible
    private ActionCheckingEnum checkItemsInContainer() {
        ItemContainerComponent container = designation.take<ItemContainerComponent>();
        int requiredItems = order.amount - container.items.Count;
        if (requiredItems == 0) return OK;
        List<EcsEntity> foundItems = model.itemContainer.availableItemsManager
            .findNearest(order.itemType, order.material, requiredItems, designation.pos());
        if (foundItems.Count != requiredItems) return FAIL;
        lockEntities(foundItems);
        foreach (EcsEntity item in foundItems) {
            addPreAction(new PutItemToContainerAction(designation, item));
        }
        return NEW;
    }

    // finds item that can be removed from construction site and not locked to other task.
    // For first found item creates sub-action to move it to nearby off-site tile
    private ActionCheckingEnum checkClearingSite(IntBounds3 bounds) {
        ItemContainer container = model.itemContainer;
        foreach (var position in bounds.toList()) {
            List<EcsEntity> itemsOnTile = container.onMap.itemsOnMap.get(position);
            if (itemsOnTile.Count > 0) {
                Vector3Int offSitePosition = findPositionToPutItem(position);
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
        ref ItemContainerComponent container = ref designation.takeRef<ItemContainerComponent>();
        foreach (EcsEntity item in container.items) {
            model.itemContainer.transition.fromContainerToGround(item, designation, order.position);
        }
        return FAIL;
    }

    private IntBounds3 getBuildingBounds(GenericBuildingOrder order) {
        Vector3Int offset = new(); // no offset for constructions (they are single-tile)
        if (order is BuildingOrder) {
            BuildingOrder buildingOrder = (BuildingOrder)order;
            bool horizontal = OrientationUtil.isHorizontal(buildingOrder.orientation);
            offset = new(buildingOrder.type.size[horizontal ? 1 : 0] - 1, buildingOrder.type.size[horizontal ? 0 : 1] - 1);
        }
        return new IntBounds3(order.position, order.position + offset);
    }

    private Vector3Int findPositionToPutItem(Vector3Int itemPosition) {
        List<Vector3Int> acceptablePositions = target.getAcceptablePositions(model);
        if (acceptablePositions.Count == 0) return Vector3Int.back; // not found
        int performerArea = model.localMap.passageMap.area.get(performer.pos());
        return acceptablePositions
            .Where(position => model.localMap.passageMap.area.get(position) == performerArea)
            .MinBy(position => (position - itemPosition).sqrMagnitude);
    }

    protected void consumeItems() {
        foreach (EcsEntity item in designation.take<ItemContainerComponent>().items.ToList()) {
            model.itemContainer.stored.removeItemFromContainer(item);
            item.Destroy();
        }
    }
}
}