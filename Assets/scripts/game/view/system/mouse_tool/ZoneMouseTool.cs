using game.model;
using game.view.util;
using types;
using UnityEngine;
using util.geometry.bounds;
using static game.view.system.mouse_tool.ZoneTypeEnum;

namespace game.view.system.mouse_tool {
    public class ZoneMouseTool : MouseTool {
        public ZoneTypeEnum zoneType; 
        // public TODO zone data

        public void set(ZoneTypeEnum zoneType) {
            this.zoneType = zoneType;
        }

        public override bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }

        public override void applyTool(IntBounds3 bounds) {
            if (zoneType == NONE) {
            GameModel.get().currentLocalModel.zoneContainer.eraseZones(bounds);
                
            }
            GameModel.get().currentLocalModel.zoneContainer.createZone(bounds, zoneType);
        }

        public override void updateSprite() {
            selectorGO.setToolSprite(IconLoader.get("mousetool/zone"));
        }

        public override void rotate() {}

        public override void updateSpriteColor(Vector3Int position) {
            bool passable = GameModel.get().currentLocalModel.localMap.passageMap.passage.get(position) == PassageTypes.PASSABLE.VALUE;
            selectorGO.designationValid(passable);
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setToolSprite(null);
        }
    }

    public enum ZoneTypeEnum {
        STOCKPILE,
        FARM,
        NONE // for deletion
    }
}