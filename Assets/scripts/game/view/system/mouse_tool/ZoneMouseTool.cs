using System;
using game.model;
using game.model.container;
using game.view.util;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;

namespace game.view.system.mouse_tool {
    // creation tool:
    //      overwrites overlapped zones with new.
    // update tool:
    //      if selection starts on existing zone, it is extended. 
    //      if selection starts on free tile and overlaps zone, it is cleared
    //      if selection starts on existing zone and overlaps zone of the same type, they are merged
    //      if selection starts on existing zone and overlaps zone of another type, it is overwritten
    public class ZoneMouseTool : MouseTool {
        public ZoneMouseToolType toolType;
        public ZoneTypeEnum zoneType; // only for CREATE toolType
        
        public void set(ZoneTypeEnum type) {
            toolType = ZoneMouseToolType.CREATE;
            zoneType = type;
        }

        public void set(ZoneMouseToolType type) {
            toolType = type;
        }

        public override bool updateMaterialSelector() {
            materialSelector.close();
            return true;
        }

        public override void applyTool(IntBounds3 bounds, Vector3Int start) {
            switch (toolType) {
                case ZoneMouseToolType.CREATE:
                    GameModel.local().zoneContainer.createZone(bounds, zoneType);
                    break;
                case ZoneMouseToolType.UPDATE:
                    updateZones(bounds, start);
                    break;
                case ZoneMouseToolType.DELETE:
                    GameModel.local().zoneContainer.eraseZones(bounds);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void updateZones(IntBounds3 bounds, Vector3Int start) {
            ZoneContainer container = GameModel.local().zoneContainer;
            EcsEntity zone = container.getZone(start);
            if (zone == EcsEntity.Null) {
                container.eraseZones(bounds);
            } else {
                container.addTilesToZone(zone, bounds);
            }
        }
        
        public override void updateSprite() {
            selectorGO.setToolSprite(IconLoader.get("mousetool/zone"));
        }

        public override void rotate() {}

        public override void updateSpriteColor(Vector3Int position) {
            bool valid = ZoneTypes.get(zoneType).positionValidator.validate(position, GameModel.get().currentLocalModel);
            selectorGO.designationValid(valid);
        }

        public override void reset() {
            materialSelector.close();
            selectorGO.setToolSprite(null);
        }
    }

    public enum ZoneMouseToolType {
        CREATE,
        UPDATE,
        DELETE
    }
}