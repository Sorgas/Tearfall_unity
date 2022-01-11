using enums;
using game.model;
using game.model.util.validation;
using game.view.util;
using UnityEngine;

namespace game.view.system.mouse_tool {
    public class MouseToolEnum {
        public static readonly MouseToolType NONE = new MouseToolType("none");
        public static readonly MouseToolType CLEAR = new MouseToolType("cancel");
        public static readonly MouseToolType DIG = new MouseToolType("dig", DesignationTypeEnum.D_DIG);
        public static readonly MouseToolType CHANNEL = new MouseToolType("channel", DesignationTypeEnum.D_CHANNEL);
        public static readonly MouseToolType RAMP = new MouseToolType("ramp", DesignationTypeEnum.D_RAMP);
        public static readonly MouseToolType STAIRS = new MouseToolType("stairs", DesignationTypeEnum.D_STAIRS);
        public static readonly MouseToolType DOWNSTAIRS = new MouseToolType("downstairs", DesignationTypeEnum.D_DOWNSTAIRS);
        
        public static bool validateDigging(BlockType targetBlockType, Vector3Int position) {
            return targetBlockType.OPENNESS > BlockTypeEnum.get(GameModel.localMap.blockType.get(position)).OPENNESS;
        }
    }
    
    public class MouseToolType {
        public Sprite sprite;
        public DesignationType designation;
        
        public MouseToolType(string iconName) {
            sprite = iconName != "none"
                ? IconLoader.get("orders/" + iconName)
                : null;
        }

        public MouseToolType(string iconName, DesignationType designation) {
            sprite = iconName != "none"
                ? IconLoader.get("orders/" + iconName)
                : null;
            this.designation = designation;
        }
    }
}