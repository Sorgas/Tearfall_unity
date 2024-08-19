using game.view.camera;
using Leopotam.Ecs;
using types;
using types.building;
using UnityEngine;
using util.geometry.bounds;
using util.lang;

namespace game.view.system.mouse_tool {
    public sealed class MouseToolManager : Singleton<MouseToolManager> {
        private readonly SelectionMouseTool selectionTool = new();
        private readonly DesignationMouseTool designationTool = new();
        private readonly ConstructionMouseTool constructionTool = new();
        private readonly BuildingMouseTool buildingTool = new();
        private readonly UnitMovementTargetTool unitMovementTargetTool = new();
        private readonly ZoneMouseTool zoneTool = new();
        public readonly DebugTileMouseTool debugTileTool = new();
        public readonly DebugBuildingMouseTool debugBuildingMouseTool = new();
        private readonly SelectorSpriteUpdater updater = new();
        public MouseTool tool;

        // apply tool when player finishes dragging selection frame
        public void handleSelection(IntBounds3 bounds, Vector3Int start) => tool?.applyTool(bounds, start);

        public void handleRightClick(Vector3Int position) {
            if (tool == selectionTool) {
                selectionTool.handleRightClick(position);
                // TODO open tooltip
            } else {
                reset();
            }
        }
        
        // revalidate position when mouse moved
        public void mouseMoved(Vector3Int position) {
            tool?.onPositionChange(position);
            updater.updateSprite(position);
        }

        // sets SelectionTool as current
        public void reset() => set(selectionTool);

        public void set(DesignationType type) {
            designationTool.designation = type;
            set(designationTool);
        }

        public void set(BuildingType buildingType) {
            buildingTool.setBuildingType(buildingType);
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

        private void set(MouseTool tool) {
            Debug.Log($"selected mouse tool: {tool.name}");
            this.tool = tool;
            tool.onSelectionInToolbar(); // enough items for building or items not required
            GameView.get().cameraAndMouseHandler.selectionHandler.state.selectionType = tool.selectionType;
        }

        public void setUnitMovementTarget(EcsEntity unit) {
            unitMovementTargetTool.unit = unit;
            set(unitMovementTargetTool);
        }

        public void rotate() => tool.rotate();

        public void setDebugTileTool(string blockType, string material) {
            debugTileTool.set(blockType, material);
            set(debugTileTool);
        }

        public void setDebugBuildingTool(string type) {
            debugBuildingMouseTool.setBuildingType(BuildingTypeMap.get(type));
            set(debugBuildingMouseTool);
        }
    }
}