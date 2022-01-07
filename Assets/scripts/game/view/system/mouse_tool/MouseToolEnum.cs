using enums;
using game.model;
using game.model.util.validation;
using game.view.util;
using UnityEngine;

namespace game.view.system.mouse_tool {
    public class MouseToolEnum {
        public static readonly MouseToolType NONE = new MouseToolType("none");
        public static readonly MouseToolType CLEAR = new MouseToolType("cancel");
        public static readonly MouseToolType DIG = new MouseToolType("dig", "dig", DesignationTypeEnum.D_DIG);
        public static readonly MouseToolType CHANNEL = new MouseToolType("channel", "channel", DesignationTypeEnum.D_CHANNEL);
        public static readonly MouseToolType RAMP = new MouseToolType("ramp", "ramp", DesignationTypeEnum.D_RAMP);
        public static readonly MouseToolType STAIRS = new MouseToolType("stairs", "stairs", DesignationTypeEnum.D_STAIRS);
        public static readonly MouseToolType DOWNSTAIRS = new MouseToolType("downstairs", "downstairs", DesignationTypeEnum.D_DOWNSTAIRS);
        
        public static bool validateDigging(BlockType targetBlockType, Vector3Int position) {
            return targetBlockType.OPENNESS > BlockTypeEnum.get(GameModel.localMap.blockType.get(position)).OPENNESS;
        }
    }
    
    public class MouseToolType {
        public Sprite sprite;
        public PositionValidator validator;
        public DesignationType designation;
        
        public MouseToolType(string iconName) {
            sprite = iconName != "none"
                ? IconLoader.get("orders/" + iconName)
                : null;
        }

        public MouseToolType(string iconName, string diggingType, DesignationType designation) {
            sprite = iconName != "none"
                ? IconLoader.get("orders/" + iconName)
                : null;
            validator = createValidator(diggingType);
            this.designation = designation;
        }

        // TODO create DiggingType as enum
        private static PositionValidator createValidator(string diggingType) {
            switch (diggingType) {
                    case "dig" : {
                        return new DiggingValidator(BlockTypeEnum.FLOOR);
                    }
                    case "channel" : {
                        return new DiggingChannelValidator();
                    }
                    case "ramp" : {
                        return new DiggingValidator(BlockTypeEnum.RAMP);
                    }
                    case "stairs" : {
                        return new DiggingValidator(BlockTypeEnum.STAIRS);
                    }
                    case "downstairs" : {
                        return new DiggingValidator(BlockTypeEnum.DOWNSTAIRS);
                    }
            }
            return null;
        }
    }
}