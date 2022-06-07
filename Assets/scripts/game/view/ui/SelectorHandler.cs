using UnityEngine;

namespace game.view.ui {
    public class SelectorHandler : MonoBehaviour {
        public SpriteRenderer frameIcon;
        public SpriteRenderer toolIcon;
        public SpriteRenderer constructionIcon;
        private Color transparent = new(1, 1, 1, 0.5f);
        
        public void setCurrentZ(int value) {
            toolIcon.sortingOrder = value;
            frameIcon.sortingOrder = value;
            constructionIcon.sortingOrder = value;
        }

        public void setToolSprite(Sprite sprite) => setSpriteToRenderer(sprite, toolIcon, Color.white);

        public void setConstructionSprite(Sprite sprite) => setSpriteToRenderer(sprite, constructionIcon, transparent);

        private void setSpriteToRenderer(Sprite sprite, SpriteRenderer renderer, Color color) {
            toolIcon.sprite = null;
            constructionIcon.sprite = null;
            if (sprite == null) return;
            renderer.sprite = sprite;
            renderer.color = color;
            float width = gameObject.GetComponent<RectTransform>().rect.width;
            float scale = width / sprite.rect.width * sprite.pixelsPerUnit;
            renderer.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}