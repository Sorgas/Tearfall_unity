using System.Collections.Generic;
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
            LocalModel model = GameModel.get().currentLocalModel;
            if (model.buildingContainer.buildings.ContainsKey(position)) {
                EcsEntity entity = GameModel.get().currentLocalModel.buildingContainer.buildings[position];
                if (entity.Has<WorkbenchComponent>()) {
                    WindowManager.get().showWindowForBuilding(entity);
                }
            }
            if (model.itemContainer.onMap.itemsOnMap.ContainsKey(position)) {
                handleItemSelection(model.itemContainer.onMap.itemsOnMap[position]);
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

        private void handleItemSelection(List<EcsEntity> items) {
            int itemIndex = 0;
            ItemMenuHandler window = (ItemMenuHandler)WindowManager.get().windows[ItemMenuHandler.name];
            if (WindowManager.get().activeWindowName == ItemMenuHandler.name) {
                EcsEntity currentItem = window.item;
                itemIndex = (items.IndexOf(currentItem) + 1) % items.Count;
            } 
            window.FillForItem(items[itemIndex]);
            WindowManager.get().showWindowByName(ItemMenuHandler.name);    
        }
    }
}
