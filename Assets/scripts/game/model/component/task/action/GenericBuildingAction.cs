using System.Collections.Generic;
using enums.action;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using game.model.container.item;
using game.model.localmap;
using game.model.localmap.passage;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;

namespace game.model.component.task.action {
    // base action for building constructions and buildings.
    // defines bringing items to designation, removing other items from designation position, consuming actions.
    public class GenericBuildingAction : Action {
        protected EcsEntity designation;
        protected readonly GenericBuildingOrder order;
        private Vector3Int offSitePosition; // used to remove items and performer out of site
        private IntBounds3 bounds;
        public bool offSitePositionOk = true;
        
        protected GenericBuildingAction(EcsEntity designation, GenericBuildingOrder order) :
            base(new BuildingActionTarget(designation.pos())) {
            this.designation = designation;
            this.order = order;
            bounds = getBuildingBounds(order);
            findOffSitePosition();
            
            startCondition = () => {
                if (!offSitePositionOk) return failAction();
                ActionConditionStatusEnum status = checkItemsInContainer();
                if (status == NEW) return NEW;
                if (status == FAIL) return failAction();
                if (checkClearingSite(bounds)) return NEW;
                Vector3Int pos = performer.pos();
                if (!bounds.validate((x, y, z) => pos.x != x || pos.y != y || pos.z != z)) {
                    Debug.Log("Building area check: bounds: " + bounds.toString() + " performer: " + pos + " offSitePosition: " + offSitePosition);
                    addPreAction(new MoveAction(offSitePosition));
                    return NEW;
                }
                return OK;
            };
        }

        // items for building should be brought into designation container. creates action to bring if possible
        private ActionConditionStatusEnum checkItemsInContainer() {
            Debug.Log("checking items in container");
            DesignationItemContainerComponent container = designation.take<DesignationItemContainerComponent>();
            if (container.items.Count == order.amount) return OK;
            List<EcsEntity> foundItems = GameModel.get().itemContainer.availableItemsManager
                .findNearest(order.itemType, order.material, order.amount, designation.pos());
            if (foundItems.Count != order.amount) return FAIL;
            // TODO lock items
            foreach (EcsEntity item in foundItems) {
                addPreAction(new PutItemToDesignationContainer(designation, item));
            }
            return NEW;
        }

        private bool checkClearingSite(IntBounds3 bounds) {
            Debug.Log("checking clearing site");
            ItemContainer container = GameModel.get().itemContainer;
            bool actionsAdded = false;
            bounds.iterate((x, y, z) => {
                Vector3Int pos = new(x, y, z);
                foreach (EcsEntity item in container.onMap.itemsOnMap.get(pos)) {
                    Debug.Log(item.name());
                    addPreAction(new PutItemToPositionAction(item, offSitePosition));
                    actionsAdded = true;
                }
            });
            return actionsAdded;
        }

        private ActionConditionStatusEnum failAction() {
            DesignationItemContainerComponent container = designation.take<DesignationItemContainerComponent>();
            foreach (EcsEntity item in container.items) {
                // TODO unlock items
                GameModel.get().itemContainer.onMap.putItemToMap(item, order.position);
            }
            container.items.Clear();
            return FAIL;
        }

        private IntBounds3 getBuildingBounds(GenericBuildingOrder order) {
            Vector3Int offset = new();
            if (order is BuildingOrder) {
                BuildingOrder buildingOrder = (BuildingOrder) order;
                if (OrientationUtil.isHorisontal(buildingOrder.orientation)) {
                    offset = new(buildingOrder.type.size[1], buildingOrder.type.size[0], 0);
                } else {
                    offset = new(buildingOrder.type.size[0], buildingOrder.type.size[1], 0);
                }
            }
            offset.x -= 1;
            offset.y -= 1;
            return new IntBounds3(order.position, order.position + offset);
        }
        private bool findOffSitePosition() {
            LocalMap map = GameModel.localMap;
            PassageMap passageMap = map.passageMap;
            // position to step into should be in map, reachable for performer, and connected to adjacent tile inside building
            bool checkPositionReachability(int x1, int y1, int z1, int x2, int y2, int z2) {
                if (map.inMap(x1, y1, z1) && passageMap.hasPathBetweenNeighbours(x1, y1, z1, x2, y2, z2)) {
                    offSitePosition = new Vector3Int(x1, y1, z1);
                    return true;
                }
                return false;
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
            offSitePositionOk = false;
            return false; // unreachable position, 
        }

        protected void consumeItems() {
            foreach (EcsEntity item in designation.take<DesignationItemContainerComponent>().items) {
                item.Destroy();
            }
        }
    }
}