using System.Collections.Generic;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using game.model.container.item;
using game.model.localmap;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;
using static types.action.ActionCheckingEnum;

namespace game.model.component.task.action {
    // base action for building constructions and buildings.
    // defines bringing items to designation, removing other items from designation position, consuming items.
    // TODO add place and offsiteposition after start validation
    public abstract class GenericBuildingAction : Action {
        protected EcsEntity designation;
        protected readonly GenericBuildingOrder order;
        private Vector3Int offSitePosition; // used to remove items and performer out of site
        private IntBounds3 bounds;
        public bool offSitePositionOk = false;

        protected GenericBuildingAction(EcsEntity designation, GenericBuildingOrder order) {
            this.designation = designation;
            this.order = order;
            name = "generic building action";
            bounds = getBuildingBounds(order);
            target = new BuildingConstructionActionTarget(designation.pos());

            startCondition = () => {
                if (!offSitePositionOk) {
                    findOffSitePosition();
                    if (!offSitePositionOk) {
                        return failAction("no offsitePosition found");
                    }
                }
                ActionCheckingEnum status = checkItemsInContainer();
                if (status == NEW) return NEW;
                if (status == FAIL) return failAction("items not found");
                if (checkClearingSite(bounds)) return NEW;
                Vector3Int pos = performer.pos();
                if (!bounds.validate((x, y, z) => pos.x != x || pos.y != y || pos.z != z)) {
                    log("Building area check: bounds: " + bounds.toString() + " performer: " + pos + " offSitePosition: " + offSitePosition);
                    addPreAction(new MoveAction(offSitePosition));
                    return NEW;
                }
                return OK;
            };
        }

        // items for building should be brought into designation container. creates action to bring if possible
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

        private bool checkClearingSite(IntBounds3 bounds) {
            ItemContainer container = model.itemContainer;
            bool actionsAdded = false;
            bounds.iterate((x, y, z) => {
                Vector3Int pos = new(x, y, z);
                foreach (EcsEntity item in container.onMap.itemsOnMap.get(pos)) {
                    Debug.Log(item.name());
                    // TODO add special 'move to near cell' non-locking action. 
                    // When units locks an item on the ground and other unit tries to build upon this item
                    lockEntity(item);
                    addPreAction(new PutItemToPositionAction(item, offSitePosition));
                    actionsAdded = true;
                }
            });
            return actionsAdded;
        }

        // drops item from designation container to ground
        private ActionCheckingEnum failAction(string message) {
            log("failing action " + message);
            ref ItemContainerComponent container = ref designation.takeRef<ItemContainerComponent>();
            foreach (EcsEntity item in container.items) {
                model.itemContainer.transition.fromContainerToGround(item, designation, order.position);
            }
            container.items.Clear();
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

        // TODO use performer area
        private bool findOffSitePosition() {
            LocalMap map = model.localMap;
            PassageMap passageMap = map.passageMap;
            // position to step into should be in map, reachable for performer, and connected to adjacent tile inside building
            bool checkPositionReachability(int x1, int y1, int z1, int x2, int y2, int z2) {
                if (!map.inMap(x1, y1, z1) || !passageMap.hasPathBetweenNeighbours(x1, y1, z1, x2, y2, z2)) return false;
                offSitePosition = new Vector3Int(x1, y1, z1);
                offSitePositionOk = true;
                target = new BuildingConstructionActionTarget(offSitePosition);
                return true;
            }
            // try on same z levels
            for (int z = bounds.minZ; z <= bounds.maxZ; z++) {
                for (int x = bounds.minX; x <= bounds.maxX; x++) {
                    if (checkPositionReachability(x, bounds.minY - 1, z, x, bounds.minY, z)) return true;
                    if (checkPositionReachability(x, bounds.maxY + 1, z, x, bounds.minY, z)) return true;
                }
                for (int y = bounds.minY; y <= bounds.maxY + 1; y++) {
                    if (checkPositionReachability(bounds.minX - 1, y, z, bounds.minX, y, z)) return true;
                    if (checkPositionReachability(bounds.maxX + 1, y, z, bounds.minX, y, z)) return true;
                }
            }
            // try with z levels
            for (int x = bounds.minX - 1; x <= bounds.maxX + 1; x++) {
                for (int y = bounds.minY - 1; y <= bounds.maxY + 1; y++) {
                    if (checkPositionReachability(x, y, bounds.minZ - 1, x, y, bounds.minZ)) return true;
                    if (checkPositionReachability(x, y, bounds.maxZ + 1, x, y, bounds.maxZ)) return true;
                }
            }
            return false; // unreachable position, 
        }

        protected void consumeItems() {
            foreach (EcsEntity item in designation.take<ItemContainerComponent>().items) {
                model.itemContainer.stored.removeItemFromContainer(item);
                item.Destroy();
            }
        }
    }
}