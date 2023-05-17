using game.view.system.mouse_tool;
using game.view.ui.util;
using UnityEngine;
using UnityEngine.UI;
using util;

namespace game.view.ui.toolbar {
    public class PrioritySelectionWidgetHandler : MbWindow {
        public Button button1;
        public Button button2;
        public Button button3;
        public Button button4;
        public Button button5;
        public Button button6;
        public Button button7;
        public Button button8;
        private readonly Button[] buttons = new Button[8];
        public Color activeColor = new(0.75f, 0.75f, 0.75f, 1);
        public Color inactiveColor = new(0.4f, 0.4f, 0.4f, 1);

        public void Start() {
            buttons[0] = button1;
            buttons[1] = button2;
            buttons[2] = button3;
            buttons[3] = button4;
            buttons[4] = button5;
            buttons[5] = button6;
            buttons[6] = button7;
            buttons[7] = button8;
            for (int i = 0; i < 8; i++) {
                int priority = i + 1;
                buttons[i].onClick.AddListener(() => setPriority(priority));
            }
        }

        public void init(MouseTool tool) {
            updateVisual(tool.priority);
        }

        private void setPriority(int priority) {
            MouseTool tool = MouseToolManager.get().tool;
            if (tool == null) {
                throw new GameException("Tool priority changed when no tool is selected in MouseToolManager.");
            }
            tool.priority = priority;
            updateVisual(priority);
        }

        private void updateVisual(int priority) {
            for (int i = 0; i < 8; i++) {
                buttons[i].gameObject.GetComponent<Image>().color =
                    priority == i + 1 ? activeColor : inactiveColor;
            }
        }
    }
}