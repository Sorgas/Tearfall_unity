using Leopotam.Ecs;
using types;
using UnityEngine;

namespace game.view.util {

    // TODO add status icons, thought cloud, 
    // unit sprites should look left by default
    public class UnitGoHandler : MonoBehaviour {
        private static readonly Vector3 DEFAULT_SCALE = new(0, 0.1f, 1);
        public SpriteRenderer unitRenderer;
        public SpriteMask mask;

        public SpriteRenderer background;
        public SpriteRenderer progressBar;
        public GameObject actionProgressBarHolder;
        public RectTransform actionProgressBar;

        public Sprite sprite {
            set {
                unitRenderer.sprite = value;
                mask.sprite = value;
            }
        }

        public EcsEntity unit;

        public void updateZ(int value) {
            unitRenderer.sortingOrder = value;
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

        public void mirrorX(bool value) {
            unitRenderer.flipX = value;
        }

        // Standing straight orientation is N
        public void rotate(Orientations orientation) {
            RectTransform transform = unitRenderer.gameObject.GetComponent<RectTransform>();
            switch(orientation) {
                case Orientations.N : {
                    transform.localPosition = Vector3.zero;
                    transform.rotation = Quaternion.Euler(Vector3.zero);
                }
                break;
                case Orientations.S : {
                    transform.localPosition = new Vector3(1,1,0);
                    transform.rotation = Quaternion.Euler(new Vector3(0,0, 180));
                }
                break;
                case Orientations.E : {
                    transform.localPosition = new Vector3(0,1,0);
                    transform.rotation = Quaternion.Euler(new Vector3(0,0, -90));
                }
                break;
                case Orientations.W : {
                    transform.localPosition = new Vector3(1,0,0);
                    transform.rotation = Quaternion.Euler(new Vector3(0,0, 90));
                }
                break;
            }
        }
    }
}