using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui {
    public class UiColorsEnum {
        public static readonly Color fontColor = new(0.901f, 0.901f, 0.901f); // e6e6e6
        public static readonly Color background = new(0.176f, 0.180f, 0.231f); // 2d2e3b
        public static readonly Color backgroundHighlight = new(0.251f, 0.255f, 0.302f); // 40414d
        public static readonly Color frameColor = new(0.761f, 0.745f, 0.325f); // c2be53
        public static readonly Color frameColorHighlight = new(0.929f, 0.886f, 0.420f); // ede26b
        public static readonly Color paper = new(0.922f, 0.902f, 0.773f); // ebe6c5
        public static readonly Color paperLight = new(0.961f, 0.945f, 0.855f); // f5f1da
        public static readonly Color paperFont = new(0.239f, 0.271f, 0.259f); // 3d4542
        public static readonly Color paperRed = new(0.678f, 0.337f, 0.325f); // ad5653
        public static readonly Color paperGreen = new(0.302f, 0.549f, 0.310f); // 4d8c4f
        
        public static readonly Color BUTTON_NORMAL = new(0.4f, 0.4f, 0.4f, 1);
        public static readonly Color BUTTON_HIGHLIGHTED = new(0.75f, 0.75f, 0.75f, 1);
        public static readonly Color BUTTON_PRESSED  = new(226f / 255, 209f / 255, 152f / 255, 1);
        public static readonly Color BUTTON_SELECTED = new(1, 1, 1, 1);
        public static readonly Color BUTTON_DISABLED = new(1, 1, 1, 1);
        public static readonly Color BUTTON_CHOSEN = new(239f / 255, 195f / 255, 85f / 255, 1);

        public static void initButton(Button button) {
            // TODO init colors of button
        }
    }
}