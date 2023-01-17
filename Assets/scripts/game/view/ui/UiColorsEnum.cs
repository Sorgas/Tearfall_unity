using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui {
    public class UiColorsEnum {
        public static readonly Color BUTTON_NORMAL = new Color(137f / 255, 137f / 255, 137f / 255, 1);
        public static readonly Color BUTTON_HIGHLIGHTED = new Color(183f / 255, 183f / 255, 183f / 255, 1);
        public static readonly Color BUTTON_PRESSED  = new Color(226f / 255, 209f / 255, 152f / 255, 1);
        public static readonly Color BUTTON_SELECTED = new Color(1, 1, 1, 1);
        public static readonly Color BUTTON_DISABLED = new Color(1, 1, 1, 1);
        public static readonly Color BUTTON_CHOSEN = new Color(239f / 255, 195f / 255, 85f / 255, 1);

        public static void initButton(Button button) {
            // TODO init colors of button
        }
    }
}