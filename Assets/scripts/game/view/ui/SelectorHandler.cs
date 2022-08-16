using UnityEngine;

namespace game.view.ui {
    // handles entity selector - representation of mouse on the map
    // selector has position in model units, used for drawing building sprites, 
    public class SelectorHandler : MonoBehaviour {
        public SpriteRenderer frameIcon;
        public SpriteRenderer toolIcon; // raised for walls to look 'overwall'
        public SpriteRenderer constructionIcon; // not raised for walls
        private Color transparent = new(1, 1, 1, 0.5f);
        private Color validTint = new(1, 1, 1, 0.5f);
        private Color invalidTint = new(1, 0, 0, 0.5f);
        private Color warnTint = new(1, 1, 0, 0.5f);

        public void setCurrentZ(int value) {
            toolIcon.sortingOrder = value;
            frameIcon.sortingOrder = value;
            constructionIcon.sortingOrder = value;
        }

        public void setToolSprite(Sprite sprite) => setSpriteToRenderer(sprite, 1, toolIcon, Color.white);

        public void setConstructionSprite(Sprite sprite) => setSpriteToRenderer(sprite, 1, constructionIcon, transparent);

        public void setBuildingSprite(Sprite sprite, int width) => setSpriteToRenderer(sprite, width, constructionIcon, transparent);

        public void buildingValid(bool value) {
            constructionIcon.color = value ? validTint : invalidTint;
        }

        public void designationValid(bool value) {
            toolIcon.color = value ? validTint : warnTint;
        }

        // spriteWidth - desired width of sprite in number of selector's width
        private void setSpriteToRenderer(Sprite sprite, int spriteWidth, SpriteRenderer renderer, Color color) {
            toolIcon.sprite = null;
            toolIcon.color = transparent;
            constructionIcon.sprite = null;
            constructionIcon.color = transparent;
            if (sprite == null) return;
            renderer.sprite = sprite;
            renderer.color = color;
            float width = gameObject.GetComponent<RectTransform>().rect.width; // 1
            float scale = width * sprite.pixelsPerUnit / sprite.rect.width * spriteWidth;
            renderer.transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}