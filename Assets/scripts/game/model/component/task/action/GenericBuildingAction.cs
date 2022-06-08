using System.Collections.Generic;
using enums.action;
using game.model.component.task.action.equipment.use;
using game.model.component.task.action.target;
using game.model.component.task.order;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;
using static enums.action.ActionConditionStatusEnum;

namespace game.model.component.task.action {
    // base action for building constructions and buildings.
    // defines bringing items to designation, removing other items from designation position, consuming actions.
    public class GenericBuildingAction : Action {
        protected EcsEntity designation;
        protected readonly GenericBuildingOrder order;

        protected GenericBuildingAction(EcsEntity designation, GenericBuildingOrder order) :
            base(new BuildingActionTarget(designation.pos())) {
            this.designation = designation;
            this.order = order;

            startCondition = () => {
                ActionConditionStatusEnum status = checkItemsInContainer();
                if (status == NEW) return NEW;
                if (status == FAIL) return failAction();
                if (!checkClearingSite()) return NEW;
                if (performer.pos().Equals(order.position)) { // todo handle multi tile buildings
                    addPreAction(new StepOffAction(order.position));
                    return NEW;
                }
                return OK;
            };
        }

        // items for building should be brought into designation container. creates action to bring if possible
        private ActionConditionStatusEnum checkItemsInContainer() {
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

        private bool checkClearingSite() {
            // todo handle multi tile buildings
            List<EcsEntity> items = GameModel.get().itemContainer.onMap.itemsOnMap.get(order.position);
            if (items.Count <= 0) return true;
            foreach (EcsEntity item in items) {
                Vector3Int positionToPut = GameModel.localMap.util.findFreePositionNearCenter(order.position);
                addPreAction(new PutItemToPositionAction(item, positionToPut));
            }
            return false;
        }

        // protected Int2dBounds getBuildingBounds() {
        //     Int2dBounds bounds = new Int2dBounds(order.position, 1, 1); // construction are 1x1
        //     if (!order.blueprint.construction) {
        //         BuildingType type = BuildingTypeMap.getBuilding(order.blueprint.building);
        //         IntVector2 size = RotationUtil.orientSize(type.size, order.orientation);
        //         bounds.extendTo(size.x - 1, size.y - 1);
        //     }
        //     return bounds;
        // }

        private ActionConditionStatusEnum failAction() {
            DesignationItemContainerComponent container = designation.take<DesignationItemContainerComponent>();
            foreach (EcsEntity item in container.items) {
                // TODO unlock items
                GameModel.get().itemContainer.onMap.putItemToMap(item, order.position);
            }
            container.items.Clear();
            return FAIL;
        }

        protected void consumeItems() {
            foreach (EcsEntity item in designation.take<DesignationItemContainerComponent>().items) {
                item.Destroy();
            }
        }
    }
}