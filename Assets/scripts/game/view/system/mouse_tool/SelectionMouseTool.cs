using game.model;
using game.model.component.building;
using game.view.camera;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class SelectionMouseTool : MouseTool {

        public override void applyTool(IntBounds3 bounds) {
            Vector3Int position = new(bounds.minX, bounds.minY, bounds.minZ);
            if(GameModel.get().buildingContainer.buildings.ContainsKey(position)) {
                EcsEntity entity = GameModel.get().buildingContainer.buildings[position];
                if(entity.Has<WorkbenchComponent>()) {
                    
                }
            }
        }

        public override void reset() {
            materialSelector.close();
            GameView.get().cameraAndMouseHandler.selectionHandler.state.type = SelectionTypes.AREA;
            selectorGO.setToolSprite(null);
        }

        public override void rotate() { }

        public override bool updateMaterialSelector() {
            return false;
        }

        public override void updateSprite() {
            selectorGO.setToolSprite(null);
        }

        public override void updateSpriteColor(Vector3Int position) { }
    }
}
