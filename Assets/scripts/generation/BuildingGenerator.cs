using System.Collections.Generic;
using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using Leopotam.Ecs;
using types;
using types.action;
using types.building;
using types.unit;
using UnityEngine;
using util.geometry.bounds;

namespace generation {
    public class BuildingGenerator {

        public EcsEntity generateByOrder(BuildingOrder order, EcsEntity entity) {
            entity.Replace(new BuildingComponent { type = order.type, orientation = order.orientation,
                bounds = createBounds(order.position, order.type.getSizeByOrientation(order.orientation))
            });
            entity.Replace(new PositionComponent { position = order.position });
            if (order.type.category == "workbenches") {
                entity.Replace(new WorkbenchComponent { orders = new(), job = Jobs.getByName(order.type.job) });
                entity.Replace(new PriorityComponent { priority = TaskPriorities.JOB });
                entity.Replace(new ItemContainerComponent {items = new()});
            }
            // entity.Replace(createMultiPositionComponent(order.type, order.position, order.orientation));
            entity.Replace(new NameComponent{name = order.type.name});
            if(order.type.components.Contains(BuildingType.SLEEP_FURNITURE)) {
                entity.Replace(new BuildingSleepFurnitureC());
            }
            return entity;
        }

        // private MultiPositionComponent createMultiPositionComponent(BuildingType type, Vector3Int position, Orientations orientation) {
        //     MultiPositionComponent component = new() { positions = new List<Vector3Int>() };
        //     bool flip = OrientationUtil.isHorizontal(orientation);
        //     int xSize = type.size[flip ? 1 : 0];
        //     int ySize = type.size[flip ? 0 : 1];
        //     for (int x = 0; x < xSize; x++) {
        //         for (int y = 0; y < ySize; y++) {
        //             component.positions.Add(new Vector3Int(x + position.x, y + position.y, position.z));
        //         }
        //     }
        //     return component;
        // }

        private IntBounds3 createBounds(Vector3Int position, Vector2Int size) {
            return new IntBounds3(position.x, position.y, position.z, position.x + size.x, position.y + size.y, position.z);
        }
    }
}