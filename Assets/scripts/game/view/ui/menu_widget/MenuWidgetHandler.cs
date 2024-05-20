using System.Collections.Generic;
using game.input;
using game.view.ui.jobs_window;
using game.view.ui.util;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.menu_widget {
// create buttons in side menu widget
    public class MenuWidgetHandler : GameWidget {
        public RectTransform buttonPrefab;
        private readonly Dictionary<KeyCode, Button> hotKeys = new();

        private void Start() {
            createButton("toolbar/scroll-quill", 0, JobsWindowHandler.name, KeyCode.J);
            // addMenu("toolbar/scroll-quill", 1, "jobs", KeyCode.K);
            // addMenu("toolbar/scroll-quill", 2, "jobs", KeyCode.L);
        }

        private void createButton(string icon, int index, string menuName, KeyCode hotKey) {
            RectTransform button = Instantiate(buttonPrefab, transform, false);
            button.transform.localPosition = new Vector3(0, index * button.rect.height, 0);
            button.GetComponentInChildren<Button>().onClick.AddListener(() => WindowManager.get().toggleWindowByName(menuName));
            Sprite sprite = Resources.Load<Sprite>("icons/" + icon);
            button.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            hotKeys.Add(hotKey, button.GetComponentInChildren<Button>());
        }

        public override void reset() { }

        public override bool accept(KeyCode key) {
            if (hotKeys.ContainsKey(key)) {
                hotKeys[key].onClick.Invoke();
                return true;
            }
            return false;
        }

        public override string getName() {
            return "menu_widget";
        }

        public override void init() { }
    }
}