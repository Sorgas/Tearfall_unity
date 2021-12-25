using game.view.util;
using UnityEngine;

namespace game.view.system {
    public enum MouseToolEnum : int {
        NONE,
        CLEAR,
        DIG,
        CHANNEL,
        RAMP,
        STAIRS,
        DOWNSTAIRS
        
    }

    public class MouseToolUtil {
        public static Sprite getSprite(MouseToolEnum tool) {
            switch (tool) {
                case MouseToolEnum.NONE:
                    return null;
                case MouseToolEnum.DIG:
                    return loadToolSprite("dig");
                case MouseToolEnum.CHANNEL:
                    return loadToolSprite("channel");
                case MouseToolEnum.RAMP:
                    return loadToolSprite("ramp");
                case MouseToolEnum.STAIRS:
                    return loadToolSprite("stairs");
                case MouseToolEnum.DOWNSTAIRS:
                    return loadToolSprite("downstairs");
                case MouseToolEnum.CLEAR:
                    return loadToolSprite("cancel");
                default:
                    return loadToolSprite("none");
            }
        }

        private static Sprite loadToolSprite(string name) {
            return IconLoader.get("orders/" + name);
        }
    }
}