using UnityEngine;

namespace game.view.ui {
    public class SelectorHandler : MonoBehaviour {
        public SpriteRenderer toolIcon;
        public SpriteRenderer frameIcon;

        public void setCurrentZ(int value) {
            toolIcon.sortingOrder = value;
            frameIcon.sortingOrder = value;
        }
    }
}