using enums;
using game.model;
using game.model.util.validation;
using game.view.util;
using UnityEngine;

namespace game.view.system.mouse_tool {
    // icons taken from icons/orders
    public class MouseToolEnum {
        public static readonly MouseToolType NONE = new MouseToolType("none");
        public static readonly MouseToolType CLEAR = new MouseToolType("cancel");
        
        public static readonly MouseToolType DIG = new MouseToolType("dig", DesignationTypes.D_DIG);
        public static readonly MouseToolType CHANNEL = new MouseToolType("channel", DesignationTypes.D_CHANNEL);
        public static readonly MouseToolType RAMP = new MouseToolType("ramp", DesignationTypes.D_RAMP);
        public static readonly MouseToolType STAIRS = new MouseToolType("stairs", DesignationTypes.D_STAIRS);
        public static readonly MouseToolType DOWNSTAIRS = new MouseToolType("downstairs", DesignationTypes.D_DOWNSTAIRS);

        public static readonly MouseToolType CHOP = new MouseToolType("choptrees", DesignationTypes.D_CHOP);

        public static bool validateDigging(BlockType targetBlockType, Vector3Int position) {
            return targetBlockType.OPENNESS > BlockTypeEnum.get(GameModel.localMap.blockType.get(position)).OPENNESS;
        }
    }
    
    public class MouseToolType {
        public string iconName;
        public string iconPath;
        public DesignationType designation;
        
        public MouseToolType(string iconName) {
            this.iconName = iconName;
            iconPath = "orders/" + iconName;
        }

        public MouseToolType(string iconName, DesignationType designation) {
            this.iconName = iconName;
            iconPath = "orders/" + iconName;
            this.designation = designation;
        }
    }
}