using System.Collections.Generic;
using game.model.component.task.order;
using game.model.localmap;
using game.model.util.validation;
using generation;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;

namespace game.model.container {
    // registry for player buildings in game.
    public class BuildingContainer {
        public Dictionary<Vector3Int, EcsEntity> buildings = new();
        private BuildingGenerator generator = new();

        public bool createBuilding(BuildingOrder order) {
            IntBounds3 buildingBounds = createBuildingBounds(order);
            if (!validatePosition(buildingBounds)) {
                Debug.LogError("building site is occupied");
                return false;
            }
            EcsEntity building = generator.generateByOrder(order, GameModel.get().createEntity());
            buildingBounds.iterate((x, y, z) => {
                buildings.Add(new Vector3Int(x, y, z), building);
            });
            return true;
        }

        public bool validatePosition(IntBounds3 bounds) {
            LocalMap map = GameModel.localMap;
            BuildingValidator validator = new();
            return bounds.validate((x, y, z) => validator.validate(x, y, z, map));
        }

        private IntBounds3 createBuildingBounds(BuildingOrder order) {
            bool flip = order.orientation == Orientations.E || order.orientation == Orientations.W;
            int xSize = order.type.size[flip ? 1 : 0];
            int ySize = order.type.size[flip ? 0 : 1];
            return new IntBounds3(order.position.x, order.position.y, order.position.z,
                order.position.x + xSize - 1, order.position.y + ySize - 1, order.position.z);
        }
    }
}