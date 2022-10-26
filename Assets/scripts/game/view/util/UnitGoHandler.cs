using UnityEngine;

namespace game.view.util {

    // TODO add status icons, thought cloud, 
    public class UnitGoHandler : MonoBehaviour {
        private static readonly Vector3 DEFAULT_SCALE = new(0, 0.1f, 1);
        public SpriteRenderer renderer;
        public SpriteMask mask;

        public SpriteRenderer background;
        public SpriteRenderer progressBar;
        public GameObject actionProgressBarHolder;
        public RectTransform actionProgressBar;

        public Sprite sprite {
            set {
                renderer.sprite = value;
                mask.sprite = value;
            }
        }

        public void updateZ(int value) {
            renderer.sortingOrder = value;
            background.sortingOrder = value;
            progressBar.sortingOrder = value;
            mask.frontSortingOrder = value + 2;
            mask.backSortingOrder = value + 1;
        }

        public void toggleProgressBar(bool enabled) {
            actionProgressBarHolder.SetActive(enabled);
            actionProgressBar.localScale = DEFAULT_SCALE;
        }

        public void setProgress(float value) {
            Vector3 scale = actionProgressBar.localScale;
            scale.x = value;
            actionProgressBar.localScale = scale;
        }

        public void setTargetMaskRange(int value) {

        }

        public void setMaskEnabled(bool value) {
            mask.enabled = value;
        }
    }
}