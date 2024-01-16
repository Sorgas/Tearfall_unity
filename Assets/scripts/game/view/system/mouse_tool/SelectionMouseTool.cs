using System.Collections.Generic;
using System.Linq;
using game.input;
using game.model;
using game.model.component;
using game.model.component.building;
using game.model.component.item;
using game.model.component.plant;
using game.model.component.unit;
using game.model.localmap;
using game.view.camera;
using game.view.ui;
using game.view.ui.stockpileMenu;
using game.view.ui.unit_menu;
using game.view.util;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    // selects entities from local map. does not generate ModelUpdateEvents.
    // keeps selected position and entity. when tile is selected again, chooses next entity on this tile
    // TODO add frame selection (units only)
    public class SelectionMouseTool : MouseTool {
        private Vector3Int previousPosition;
        private EcsEntity previousEntity;

        public SelectionMouseTool() {
            name = "selection mouse tool";
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            Vector3Int position = new(bounds.minX, bounds.minY, bounds.minZ);
            EcsEntity entity = getEntityForSelection(position);
            if (entity == EcsEntity.Null) return;
            if (entity.Has<WorkbenchComponent>()) {
                WindowManager.get().showWindowForBuilding(entity);
            }
            if (entity.Has<ItemComponent>()) {
                ItemMenuHandler window = (ItemMenuHandler)WindowManager.get().windows[ItemMenuHandler.name];
                window.FillForItem(entity);
                WindowManager.get().showWindowByName(ItemMenuHandler.name, false);
            }
            if (entity.Has<ZoneComponent>()) {
                selectZone(entity);
            }
            if (entity.Has<PlantComponent>()) {
                PlantMenuHandler window = (PlantMenuHandler)WindowManager.get().windows[PlantMenuHandler.name];
                window.fillForPlant(entity);
                WindowManager.get().showWindowByName(PlantMenuHandler.name, false);
            }
            if (entity.Has<UnitComponent>()) {
                UnitMenuHandler window = (UnitMenuHandler)WindowManager.get().windows[UnitMenuHandler.NAME];
                window.initFor(entity);
                WindowManager.get().showWindowByName(UnitMenuHandler.NAME, false);
            }
        }

        public override void reset() {
            materialSelector.close();
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = SelectionType.AREA;
            selectorGO.setToolSprite(null);
        }

        public override void rotate() { }

        public override void updateSprite() {
            selectorGO.setToolSprite(null);
        }

        public override void updateSpriteColor(Vector3Int position) { }

        public List<EcsEntity> raycastUnits() {
            Vector3 selectorPos = ViewUtil.fromModelToScene(GameView.get().selector.position);
            Vector3 scenePos = ViewUtil.fromScreenToSceneGlobal(Input.mousePosition, GameView.get());
            Vector2 castPos = new(scenePos.x, scenePos.y);
            RaycastHit2D[] hits = Physics2D.RaycastAll(castPos, new Vector2(1, 1), 0.01f, 1, selectorPos.z - 0.6f, selectorPos.z + 1.4f);
            return hits.Where(hit => hit.collider != null)
                .Select(hit => hit.collider.gameObject.GetComponent<UnitGoHandler>())
                .Where(component => component != null)
                .Select(component => component.unit)
                .ToList();
        }

        private void selectZone(EcsEntity entity) {
            if (entity.Has<StockpileComponent>()) {
                StockpileMenuHandler window = (StockpileMenuHandler)WindowManager.get().windows[StockpileMenuHandler.name];
                window.initFor(entity);
                WindowManager.get().showWindowByName(StockpileMenuHandler.name, false);
            } else if (entity.Has<FarmComponent>()) {
                FarmMenuHandler window = (FarmMenuHandler)WindowManager.get().windows[FarmMenuHandler.NAME];
                window.initFor(entity);
                WindowManager.get().showWindowByName(FarmMenuHandler.NAME, false);
            }
        }

        private EcsEntity getEntityForSelection(Vector3Int position) {
            List<EcsEntity> entities = collectEntities(position);
            if (entities.Count == 0) return EcsEntity.Null;
            int index = 0;
            if (previousPosition == position) {
                index = entities.IndexOf(previousEntity) + 1;
                if (index >= entities.Count) {
                    index = 0;
                }
            }
            previousPosition = position;
            previousEntity = entities[index];
            return previousEntity;
        }

        // collects all entities on a tile into list
        private List<EcsEntity> collectEntities(Vector3Int position) {
            LocalModel model = GameModel.get().currentLocalModel;
            List<EcsEntity> result = new();
            result.AddRange(raycastUnits());
            EcsEntity building = model.buildingContainer.getBuilding(position);
            if (building != EcsEntity.Null) {
                result.Add(building);
            }
            EcsEntity zone = model.zoneContainer.getZone(position);
            if (zone != EcsEntity.Null) {
                result.Add(zone);
            }
            IList<EcsEntity> items = model.itemContainer.onMap.getItems(position);
            result.AddRange(items);
            EcsEntity plant = model.plantContainer.getPlant(position);
            if (plant != EcsEntity.Null) {
                result.Add(plant);
            }
            return result;
        }
    }
}