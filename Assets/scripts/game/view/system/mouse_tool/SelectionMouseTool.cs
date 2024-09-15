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
    // Selects entities from local map. Does not generate ModelUpdateEvents.
    // Keeps selected position and entity. When tile is selected again, chooses next entity on this tile
    // TODO add frame selection (units only)
    public class SelectionMouseTool : MouseTool {
        private Vector3Int previousPosition;
        private EcsEntity previousEntity;
        private EcsEntity selectedEntity;
        
        public SelectionMouseTool() {
            name = "selection mouse tool";
            selectionType = SelectionType.SINGLE;
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            Vector3Int position = new(bounds.minX, bounds.minY, bounds.minZ);
            EcsEntity entity = getEntityForSelection(position);
            if (selectedEntity == entity) return; // clicking on already selected entity, do nothing. 
            handleEntitySelection(entity);
            selectedEntity = entity;
        }

        public void handleRightClick(Vector3Int position) {
            if (selectedEntity != EcsEntity.Null) {
                if (selectedEntity.Has<UnitComponent>()) {
                    GameView.get().selectionTooltip.showForEntity(selectedEntity, position);
                }
            }
        }
        
        public override void onPositionChange(Vector3Int position) { } // TODO

        // calls WindowManager to show appropriate window for given entity
        private void handleEntitySelection(EcsEntity entity) {
            if (entity == EcsEntity.Null) { 
                WindowManager.get().closeAll();
            } else if (entity.Has<WorkbenchComponent>()) {
                WindowManager.get().showWindowForBuilding(entity);
            } else if (entity.Has<ItemComponent>()) {
                ItemMenuHandler window = (ItemMenuHandler)WindowManager.get().windows[ItemMenuHandler.name];
                window.FillForItem(entity);
                WindowManager.get().showWindowByName(ItemMenuHandler.name, false);
            } else if (entity.Has<ZoneComponent>()) {
                selectZone(entity);
            } else if (entity.Has<PlantComponent>()) {
                PlantMenuHandler window = (PlantMenuHandler)WindowManager.get().windows[PlantMenuHandler.name];
                window.fillForPlant(entity);
                WindowManager.get().showWindowByName(PlantMenuHandler.name, false);
            } else if (entity.Has<UnitComponent>()) {
                UnitMenuHandler window = (UnitMenuHandler)WindowManager.get().windows[UnitMenuHandler.NAME];
                window.showUnit(entity);
                WindowManager.get().showWindowByName(UnitMenuHandler.NAME, false);
            }
        }
        
        private List<EcsEntity> raycastUnits() {
            Vector3 selectorPos = ViewUtil.fromModelToScene(GameView.get().cameraAndMouseHandler.selector.position);
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

        public void reset() {
            selectedEntity = EcsEntity.Null;
            previousEntity = EcsEntity.Null;
            previousPosition = Vector3Int.back;
        }
    }
}