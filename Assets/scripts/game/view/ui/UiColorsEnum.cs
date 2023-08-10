using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui {
public class UiColorsEnum {
    public static readonly Color fontColor = new(0.901f, 0.901f, 0.901f); //  #E6E6E6
    public static readonly Color backgroundDark = new Color(0.137f, 0.141f, 0.180f); // 23242e
    public static readonly Color background = new Color(0.216f, 0.220f, 0.267f); // 373844
    public static readonly Color backgroundHighlight = new Color(0.329f, 0.333f, 0.392f); // 545564
    public static readonly Color frameColor = new(0.718f, 0.584f, 0.259f); // B79542
    public static readonly Color frameColorHighlight = new(1f, 0.878f, 0.373f); // FFE15F
    public static readonly Color paper = new(0.922f, 0.902f, 0.773f); // ebe6c5
    public static readonly Color paperLight = new(0.961f, 0.945f, 0.855f); // f5f1da
    public static readonly Color paperFont = new(0.239f, 0.271f, 0.259f); // 262b29
    public static readonly Color paperRed = new(0.678f, 0.337f, 0.325f); // ad5653
    public static readonly Color paperGreen = new(0.302f, 0.549f, 0.310f); // 4d8c4f

    public static readonly Color BUTTON_NORMAL = new(0.4f, 0.4f, 0.4f, 1);
    public static readonly Color BUTTON_HIGHLIGHTED = new(0.75f, 0.75f, 0.75f, 1);
    public static readonly Color BUTTON_PRESSED = new(226f / 255, 209f / 255, 152f / 255, 1);
    public static readonly Color BUTTON_SELECTED = new(1, 1, 1, 1);
    public static readonly Color BUTTON_DISABLED = new(1, 1, 1, 1);
    public static readonly Color BUTTON_CHOSEN = new(239f / 255, 195f / 255, 85f / 255, 1);

    public static void initButton(Button button) {
        // TODO init colors of button
    }
}
}