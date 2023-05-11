using System;
using game.view.camera;
using Leopotam.Ecs;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using util.lang;
using static game.view.camera.SelectionType;

namespace game.view.system.mouse_tool {
    
    // TODO rewrite to generate ModelUpdateEvents to be consumed by ModelUpdateSystem to be run in same thread as model time updates
    public sealed class MouseToolManager : Singleton<MouseToolManager> {
        private readonly SelectionMouseTool selectionTool = new();
        private readonly DesignationMouseTool designationTool = new();
        private readonly ConstructionMouseTool constructionTool = new();
        private readonly BuildingMouseTool buildingTool = new();
        private readonly UnitMovementTargetTool unitMovementTargetTool = new();
        private readonly ZoneMouseTool zoneTool = new();
        private readonly SelectorSpriteUpdater updater = new();
        private readonly DebugTileMouseTool debugTileTool = new();
        private MouseTool tool;

        public void handleSelection(IntBounds3 bounds, Vector3Int start) => tool?.applyTool(bounds, start);

        public void mouseMoved(Vector3Int position) {
            tool?.updateSpriteColor(position); // TODO use position in tools (for performance)
            updater.updateSprite(position);
        }
        
        public void reset() {
            tool?.reset();
            set(selectionTool);
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = SINGLE;
        }

        public void set(DesignationType type) {
            designationTool.designation = type;
            set(designationTool);
        }
        
        public void set(BuildingType buildingType) {
            buildingTool.type = buildingType;
            set(buildingTool);
        }

        public void set(ConstructionType type) {
            constructionTool.set(type);
            set(constructionTool);
        }

        public void set(ZoneTypeEnum type) {
            zoneTool.set(type);
            set(zoneTool);
        }

        public void set(ZoneMouseToolType type) {
            zoneTool.set(type);
            set(zoneTool);
        }
        
        public void setUnitMovementTarget(EcsEntity unit) {
            unitMovementTargetTool.unit = unit; 
            set(unitMovementTargetTool);
        }
        
        // sets item type and material selection from material selector widget
        public void setItem(string typeName, int materialId) {
            if (tool is not ItemConsumingMouseTool mouseTool) {
                throw new ArgumentException("item set to inappropriate tool in MouseToolManager.");
            } else {
                mouseTool.setItem(typeName, materialId);
            }
        }

        public void rotate() => tool.rotate();

        public void setDebug(string blockType, string material) {
            debugTileTool.set(blockType, material);
            set(debugTileTool);
        }
        
        private void set(MouseTool tool) { 
            if (tool == null) tool = selectionTool;
            this.tool = tool;
            bool enoughItems = tool.updateMaterialSelector(); // enough items for building or items not required
            tool.updateSprite();
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = tool.selectionType;
        }
    }
}