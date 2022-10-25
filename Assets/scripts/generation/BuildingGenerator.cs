using System.Collections.Generic;
using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using Leopotam.Ecs;
using types;
using types.building;
using UnityEngine;

namespace generation {
    public class BuildingGenerator {

        public EcsEntity generateByOrder(BuildingOrder order, EcsEntity entity) {
            entity.Replace(new BuildingComponent { type = order.type, orientation = order.orientation });
            entity.Replace(new PositionComponent { position = order.position });
            if (order.type.category == "workbenches") {
                entity.Replace(new WorkbenchComponent { orders = new(), hasActiveOrders = false });
                entity.Replace(new BuildingItemContainerComponent {items = new()});
            }
            entity.Replace(createMultiPositionComponent(order.type, order.position, order.orientation));
            entity.Replace(new NameComponent{name = order.type.name});
            return entity;
        }

        private MultiPositionComponent createMultiPositionComponent(BuildingType type, Vector3Int position, Orientations orientation) {
            MultiPositionComponent component = new() { positions = new List<Vector3Int>() };
            bool flip = OrientationUtil.isHorisontal(orientation);
            int xSize = type.size[flip ? 1 : 0];
            int ySize = type.size[flip ? 0 : 1];
            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    component.positions.Add(new Vector3Int(x + position.x, y + position.y, position.z));
                }
            }
            return component;
        }
    }
}