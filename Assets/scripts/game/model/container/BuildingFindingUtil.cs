using System.Linq;
using game.model.component;
using game.model.component.building;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.lang.extension;

namespace game.model.container {
    public class BuildingFindingUtil : LocalModelContainer {
        private BuildingContainer container;

        public BuildingFindingUtil(LocalModel model, BuildingContainer container) : base(model) {
            this.container = container;
        }

        public EcsEntity findFreeChairWithTable(Vector3Int position) {
            return container.buildings.Values
                .Where(building => building.Has<BuildingSitFurnitureC>())
                .Where(building => !building.Has<LockedComponent>())
                .Where(chair => model.localMap.passageMap.inSameArea(position, chair.pos()))
                .Where(chair => tableExistsInFrontOfChair(chair))
                .FirstOrDefault();
        }

        private bool tableExistsInFrontOfChair(EcsEntity chair) {
            BuildingSitFurnitureC component = chair.take<BuildingSitFurnitureC>();
            Vector3Int tablePosition = chair.pos() + OrientationUtil.getOffset(component.orientation);
            return container.buildings.ContainsKey(tablePosition)
                   && container.buildings[tablePosition].Has<BuildingTableFurnitureC>();
        }
    }
}