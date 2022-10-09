using game.model;
using game.model.component.building;
using game.view.camera;
using game.view.ui;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class SelectionMouseTool : MouseTool {

        public override void applyTool(IntBounds3 bounds) {
            Vector3Int position = new(bounds.minX, bounds.minY, bounds.minZ);
            if(GameModel.get().currentLocalModel.buildingContainer.buildings.ContainsKey(position)) {
                EcsEntity entity = GameModel.get().currentLocalModel.buildingContainer.buildings[position];
                if(entity.Has<WorkbenchComponent>()) {
                    WindowManager.get().showWindowForBuilding(entity);
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
            materialSelector.close();
            return true;
        }

        public override void updateSprite() {
            selectorGO.setToolSprite(null);
        }

        public override void updateSpriteColor(Vector3Int position) { }
    }
}
