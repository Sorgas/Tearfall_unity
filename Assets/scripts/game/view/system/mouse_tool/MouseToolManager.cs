using game.view.camera;
using Leopotam.Ecs;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using util.lang;
using static game.view.camera.SelectionTypes;

namespace game.view.system.mouse_tool {
    // TODO split into different tools
    public class MouseToolManager : Singleton<MouseToolManager> {
        private MouseTool tool;
        private static SelectionMouseTool selectionTool = new();
        private static DesignationMouseTool designationTool = new();
        private static ConstructionMouseTool constructionTool = new();
        private static BuildingMouseTool buildingTool = new();
        private static UnitMovementTargetTool unitMovementTargetTool = new();
        private SelectorSpriteUpdater updater = new();

        public static void handleSelection(IntBounds3 bounds) => get().handleSelection_(bounds);
        
        public void mouseMoved(Vector3Int position) {
            tool?.updateSpriteColor(position); // TODO use position in tools (for performance)
            updater.updateSprite(position);
        }
        
        public static void reset() {
            get().tool?.reset();
            get()._set(selectionTool);
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = SINGLE;
        }

        public static void set(DesignationType type) {
            designationTool.designation = type;
            get()._set(designationTool);
        }
        
        public static void set(BuildingType buildingType) {
            buildingTool.type = buildingType;
            get()._set(buildingTool);
        }

        public static void set(ConstructionType type) {
            constructionTool.set(type);
            get()._set(constructionTool);
        }

        public void setUnitMovementTarget(EcsEntity unit) {
            unitMovementTargetTool.unit = unit; 
            _set(unitMovementTargetTool);
        }
        
        public void setItem(string typeName, int materialId) {
            if (tool is ItemConsumingMouseTool) ((ItemConsumingMouseTool)tool).setItem(typeName, materialId);
        }

        public virtual void rotateBuilding() {
            tool.rotate();
        }

        private void handleSelection_(IntBounds3 bounds) {
            tool?.applyTool(bounds);
        }
        
        private void _set(MouseTool tool) { 
            this.tool = tool;
            if (tool == null) return;
            bool enoughItems = tool.updateMaterialSelector(); // enough items for building or items not required
            tool.updateSprite();
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = tool.selectionType;
        }
    }
}