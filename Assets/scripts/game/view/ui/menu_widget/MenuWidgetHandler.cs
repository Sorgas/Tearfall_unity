using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace game.view.ui.menu_widget {
    
    // create buttons in side menu widget
    public class MenuWidgetHandler : MonoBehaviour, IHotKeyAcceptor {
        public RectTransform buttonPrefab;
        private readonly Dictionary<KeyCode, Button> hotKeys = new Dictionary<KeyCode, Button>();

        private void Start() {
            createButton("toolbar/scroll-quill", 0, "jobs", KeyCode.J);
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

        public bool accept(KeyCode key) {
            if (hotKeys.ContainsKey(key)) {
                hotKeys[key].onClick.Invoke();
                return true;
            }
            return false;
        }
    }
}