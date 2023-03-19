using System.Collections.Generic;
using game.input;
using game.model;
using game.model.component;
using game.model.component.building;
using game.model.localmap;
using game.view.camera;
using game.view.ui;
using game.view.ui.stockpileMenu;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    public class SelectionMouseTool : MouseTool {

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
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
            if (model.zoneContainer.zones.ContainsKey(position)) {
                handleZoneSelection(model.zoneContainer.getZone(position));
            }
            raycastUnit();
        }

        public override void reset() {
            materialSelector.close();
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = SelectionType.AREA;
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
            WindowManager.get().showWindowByName(ItemMenuHandler.name, false);
        }

        public void raycastUnit() {
            Vector3 selectorPos = ViewUtil.fromModelToScene(GameView.get().selector.position);
            Vector3 scenePos = ViewUtil.fromScreenToSceneGlobal(Input.mousePosition, GameView.get());
            Vector2 castPos = new Vector2(scenePos.x, scenePos.y);
            RaycastHit2D hit = Physics2D.Raycast(castPos, new Vector2(1,1), 0.01f, 1, selectorPos.z - 0.6f, selectorPos.z + 1.4f);
            if(hit.collider != null) {
                UnitGoHandler unitComponent = hit.collider.gameObject.GetComponent<UnitGoHandler>();
                if(unitComponent != null) {
                    WindowManager.get().showWindowForUnit(unitComponent.unit);
                }
            }
        }

        private void handleZoneSelection(EcsEntity entity) {
            if (entity.Has<StockpileComponent>()) {
                StockpileMenuHandler window = (StockpileMenuHandler)WindowManager.get().windows[StockpileMenuHandler.name];
                window.initFor(entity);
                WindowManager.get().showWindowByName(StockpileMenuHandler.name, false);
            } else if (entity.Has<FarmComponent>()) {
                FarmMenuHandler window = (FarmMenuHandler )WindowManager.get().windows[FarmMenuHandler .name];
                window.initFor(entity);
                WindowManager.get().showWindowByName(FarmMenuHandler .name, false);
            }
        }
    }
}
