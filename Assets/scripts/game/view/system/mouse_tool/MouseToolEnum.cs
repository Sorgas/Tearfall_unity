using enums;

namespace game.view.system.mouse_tool {
    // icons taken from icons/orders
    public class MouseToolEnum {
        public static readonly MouseToolType NONE = new();
        public static readonly MouseToolType CLEAR = new("cancel");
        
        public static readonly MouseToolType DIG = new("dig", DesignationTypes.D_DIG);
        public static readonly MouseToolType CHANNEL = new("channel", DesignationTypes.D_CHANNEL);
        public static readonly MouseToolType RAMP = new("ramp", DesignationTypes.D_RAMP);
        public static readonly MouseToolType STAIRS = new("stairs", DesignationTypes.D_STAIRS);
        public static readonly MouseToolType DOWNSTAIRS = new("downstairs", DesignationTypes.D_DOWNSTAIRS);

        public static readonly MouseToolType CHOP = new("choptrees", DesignationTypes.D_CHOP);
        
        public static readonly MouseToolType BUILD = new();
        public static readonly MouseToolType CONSTRUCT = new();
    }
    
    public class MouseToolType {
        public string iconName;
        public string iconPath;
        public DesignationType designation;

        public MouseToolType() : this("none", null) { }

        public MouseToolType(string iconName) : this(iconName, null) { }

        public MouseToolType(string iconName, DesignationType designation) {
            this.iconName = iconName;
            iconPath = "mousetool/" + iconName;
            this.designation = designation;
        }
    }
}