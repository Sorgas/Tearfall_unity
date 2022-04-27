using game.model.localmap;
using game.view.ui;
using UnityEngine;

namespace game.view.camera {
    public class SelectorSpriteUpdater {
        private LocalMap map;
        private SelectorHandler selector;
        private Sprite selectors;
        private Sprite flatTile;
        private Sprite wallTile;

        public SelectorSpriteUpdater(LocalMap map, SelectorHandler selector) {
            this.map = map;
            this.selector = selector;
            selectors = Resources.Load<Sprite>("icons/selectors");
            flatTile = Sprite.Create(selectors.texture, new Rect(0, 0, 64, 96), Vector2.zero, 64);;
            wallTile = Sprite.Create(selectors.texture, new Rect(64, 0, 64, 96), Vector2.zero, 64);;
        }

        public void updateSprite(Vector3Int position) {
            if (!map.inMap(position)) return;
            if (map.blockType.getEnumValue(position).FLAT) {
                selector.toolIcon.transform.localPosition = new Vector3(0.5f,0.5f,0);
                selector.frameIcon.sprite = flatTile;
            } else {
                selector.toolIcon.transform.localPosition = new Vector3(0.5f,90f / 96,0);
                selector.frameIcon.sprite = wallTile;
            }
        }
    }
}