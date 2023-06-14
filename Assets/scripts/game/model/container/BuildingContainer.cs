using System.Collections.Generic;
using game.model.component.building;
using game.model.component.task.order;
using game.model.localmap;
using game.model.util.validation;
using generation;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;

namespace game.model.container {
    // registry for player buildings in game.
    public class BuildingContainer : LocalModelContainer {
        public Dictionary<Vector3Int, EcsEntity> buildings = new(); // links position to buildings. one tile can have only one building
        public BuildingFindingUtil util;
        private BuildingGenerator generator = new();

        public BuildingContainer(LocalModel model) : base(model) {
            util = new(model, this);
        }

        public EcsEntity createBuilding(BuildingOrder order) {
            IntBounds3 buildingBounds = createBuildingBounds(order);
            if (!validatePosition(buildingBounds)) {
                Debug.LogError("building site is occupied");
                return EcsEntity.Null;
            }
            EcsEntity building = generator.generateByOrder(order, model.createEntity());

            buildingBounds.iterate((x, y, z) => {
                buildings.Add(new Vector3Int(x, y, z), building);
            });
            if (order.type.passage == "impassable") {
                buildingBounds.iterate((x, y, z) => {
                    model.localMap.passageMap.updater.update(x, y, z);
                });
            }
            Debug.Log("[BuildingContainer] building " + building.name() + " created in " + building.pos());
            return building;
        }

        // public bool createBuilding(BuidingType type, Vector3Int position, Orientations orientation, int material) {
        //     EcsEntity building = generator.
        //     return true;            
        // }

        private bool validatePosition(IntBounds3 bounds) {
            LocalMap map = model.localMap;
            BuildingValidator validator = new();
            return bounds.validate((x, y, z) => validator.validate(x, y, z, map));
        }

        public bool isBuildingBlockPassable(int x, int y, int z) {
            Vector3Int position = new(x, y, z);
            return !buildings.ContainsKey(position) || buildings[position].take<BuildingComponent>().type.passage != "impassable";
        }

        public EcsEntity getBuilding(Vector3Int position) {
            return buildings.ContainsKey(position) ? buildings[position] : EcsEntity.Null;
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