using UnityEngine;

namespace game.view.util {
    
    public class UnitGoHandler : MonoBehaviour {
        public SpriteRenderer renderer;
        public SpriteMask mask;

        public Sprite sprite {
            set {
                renderer.sprite = value;
                mask.sprite = value;
            }
        }

        public void updateZ(int value) {
            renderer.sortingOrder = value;
            mask.frontSortingOrder = value + 2;
            mask.backSortingOrder = value + 1;
        }

        public void setTargetMaskRange(int value) {
            
        }

        public void setMaskEnabled(bool value) {
            mask.enabled = value;
        }
    }
}