using types;
using types.building;
using util.geometry.bounds;
using util.lang;

namespace game.view.system.mouse_tool {
    // TODO split into different tools
    public class MouseToolManager : Singleton<MouseToolManager> {
        private MouseTool tool;
        private static DesignationMouseTool designationTool = new();
        private static ConstructionMouseTool constructionTool = new();
        private static BuildingMouseTool buildingTool = new();
        
        public static void set(DesignationType type) {
            designationTool.designation = type;
            get()._set(designationTool);
        }
        
        public static void set(BuildingType buildingType) {
            buildingTool.type = buildingType;
            get()._set(buildingTool);
        }

        public static void set(ConstructionType type) {
            constructionTool.type = type;
            get()._set(constructionTool);
        }

        // public void validate() {
        //     if (validator == null) return;
        //     if (tool == BUILD) {
        //         bool flip = orientation == Orientations.E || orientation == Orientations.W;
        //         int x = buildingType.size[flip ? 1 : 0];
        //         int y = buildingType.size[flip ? 0 : 1];
        //         for (int i = 0; i < x; i++) {
        //             
        //         }
        //     }
        // }

        private void _set(MouseTool tool) {
            this.tool = tool;
            bool materialsOk = tool.updateMaterialSelector(); // enough items for building or items not required
            tool.updateSelectionType(materialsOk);
            tool.updateSprite(materialsOk);
        }

        public void setItem(string typeName, int materialId) {
            if (tool is ItemConsumingMouseTool) ((ItemConsumingMouseTool)tool).setItem(typeName, materialId);
        }

        public static void handleSelection(IntBounds3 bounds) => get().handleSelection_(bounds);

        public void rotateBuilding() {
            tool.rotate();
        }

        private void handleSelection_(IntBounds3 bounds) {
            if(tool != null) tool.applyTool(bounds);
        }
    }
}