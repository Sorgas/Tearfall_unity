using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui {
    public class UiColorsEnum {
        public static readonly Color fontColor = new Color(0.901f, 0.901f, 0.901f);
        public static readonly Color background = new Color(0.176f, 0.180f, 0.231f);
        public static readonly Color backgroundHighlight = new Color(0.251f, 0.255f, 0.302f);
        public static readonly Color frameColor = new Color(0.761f, 0.745f, 0.325f);
        public static readonly Color frameColorHighlight = new Color(0.929f, 0.886f, 0.420f);
        public static readonly Color paper = new Color(0.922f, 0.902f, 0.773f);
        public static readonly Color paperLight = new Color(0.961f, 0.945f, 0.855f);
        public static readonly Color paperFont = new Color(0.239f, 0.271f, 0.259f);
        public static readonly Color paperRed = new Color(0.678f, 0.337f, 0.325f);
        public static readonly Color paperGreen = new Color(0.302f, 0.549f, 0.310f);
        
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