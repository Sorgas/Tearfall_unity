using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui {
public class UiColorsEnum {
    // for text on background
    public static readonly Color FONT_COLOR = new(0.901f, 0.901f, 0.901f); //  #E6E6E6
    public static readonly Color BACKGROUND_DARK = new Color(0.137f, 0.141f, 0.180f); // 23242e
    public static readonly Color BACKGROUND = new Color(0.216f, 0.220f, 0.267f); // 373844
    public static readonly Color BACKGROUND_HIGHLIGHT = new Color(0.329f, 0.333f, 0.392f); // 545564
    
    public static readonly Color FRAME_COLOR = new(0.718f, 0.584f, 0.259f); // B79542
    public static readonly Color FRAME_COLOR_HIGHLIGHT = new(1f, 0.878f, 0.373f); // FFE15F
    
    // for text on paper elements
    public static readonly Color PAPER_FONT = new(0.239f, 0.271f, 0.259f); // 262b29
    public static readonly Color PAPER = new(0.922f, 0.902f, 0.773f); // ebe6c5
    public static readonly Color PAPER_LIGHT = new(0.961f, 0.945f, 0.855f); // f5f1da
    public static readonly Color PAPER_RED = new(0.678f, 0.337f, 0.325f); // ad5653
    public static readonly Color PAPER_GREEN = new(0.302f, 0.549f, 0.310f); // 4d8c4f
    public static readonly Color RED_BRIGHT;
    public static readonly Color GREEN_BRIGHT;

    public static readonly Color BUTTON_NORMAL = new Color(0.5765f, 0.5765f, 0.6235f); // 93939F
    public static readonly Color BUTTON_HIGHLIGHTED = new Color(0.7804f, 0.7804f, 0.8196f, 1f); // C7C7D1
    public static readonly Color BUTTON_PRESSED = new(226f / 255, 209f / 255, 152f / 255, 1);
    public static readonly Color BUTTON_SELECTED = new(1, 1, 1, 1);
    public static readonly Color BUTTON_DISABLED = new(1, 1, 1, 1);
    public static readonly Color BUTTON_CHOSEN = new(239f / 255, 195f / 255, 85f / 255, 1);

    static UiColorsEnum() {
        ColorUtility.TryParseHtmlString("#AD403D", out RED_BRIGHT);
        ColorUtility.TryParseHtmlString("#318C34", out GREEN_BRIGHT);
    }
    
    public static void initButton(Button button) {
        // TODO init colors of button
    }
}
}