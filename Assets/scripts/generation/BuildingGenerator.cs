using System.Collections.Generic;
using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using Leopotam.Ecs;
using types;
using UnityEngine;

namespace generation {
    public class BuildingGenerator {
        public EcsEntity generateByOrder(BuildingOrder order, EcsEntity entity) {
            entity.Replace(new BuildingComponent { type = order.type, orientation = order.orientation });
            entity.Replace(new PositionComponent { position = order.position });
            MultiPositionComponent multiPositionComponent = new() { positions = new List<Vector3Int>() };
            bool flip = order.orientation == Orientations.E || order.orientation == Orientations.W;
            int xSize = order.type.size[flip ? 1 : 0];
            int ySize = order.type.size[flip ? 0 : 1];
            for (int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    multiPositionComponent.positions.Add(new Vector3Int(x + order.position.x, y + order.position.y, order.position.z));
                }
            }
            entity.Replace(multiPositionComponent);
            return entity;
        }
    }
}