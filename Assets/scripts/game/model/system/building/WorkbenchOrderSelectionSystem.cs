using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

// rolls orders of WorkbenchComponent and creates WorkbenchCurrentOrderComponent. 
// First order from list is taken. 
// List is rolled on order's completion.
// if WB has orders, current order should be selected
namespace game.model.system.building {
    class WorkbenchOrderSelectionSystem : IEcsRunSystem {
        public EcsFilter<WorkbenchComponent>.Exclude<WorkbenchCurrentOrderComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                ref WorkbenchComponent component = ref filter.Get1(i);
                if (!component.hasActiveOrders) return;
                handleFirstOrder(filter.GetEntity(i), ref component);
            }
        }

        // chooses action for first order. 
        private void handleFirstOrder(EcsEntity entity, ref WorkbenchComponent component) {
            Debug.Log("[WorkbenchOrderSelectionSystem] selecting order for " + entity.name());
            CraftingOrder firstOrder = component.orders[0];
            if (firstOrder.paused) { // paused, move to end
                moveOrderToEnd(ref component, entity);
            } else {
                if (firstOrder.performedQuantity < firstOrder.targetQuantity) { // oreder quantity not reached, make current
                    CraftingOrder order = component.orders[0];
                    entity.Replace<WorkbenchCurrentOrderComponent>(new WorkbenchCurrentOrderComponent { currentOrder = order });
                    Debug.Log("[WorkbenchOrderSelectionSystem] order " + order.name + " selected for " + entity.name());
                } else { // order completed
                    if (firstOrder.repeated) { // move repeated order to end
                        firstOrder.performedQuantity = 0;
                        moveOrderToEnd(ref component, entity);
                    } else { // delete not repeated order
                        component.orders.RemoveAt(0);
                    }
                }
            }
            entity.Replace(new UiUpdateComponent());
            component.updateFlag();
        }

        private void moveOrderToEnd(ref WorkbenchComponent component, EcsEntity entity) {
            CraftingOrder firstOrder = component.orders[0];
            Debug.Log("[WorkbenchOrderSelectionSystem] moving order to end " + firstOrder.name);
            component.orders.RemoveAt(0);
            component.orders.Add(firstOrder);
        }
    }
}