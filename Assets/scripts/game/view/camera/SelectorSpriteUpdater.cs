using game.model;
using game.model.localmap;
using game.view.ui;
using types;
using UnityEngine;

namespace game.view.camera {
    // changes selector frame between flat/wall variants, and white/gray colors depending on tile
    public class SelectorSpriteUpdater {
        private SelectorHandler selector;
        private Sprite selectors;
        private Sprite flatTile;
        private Sprite wallTile;
        private Vector3 flatIconPosition = new(0.5f, 0.5f, 0);
        private Vector3 wallIconPosition = new(0.5f, 90f / 96, 0);
        private Color lightGrey = new(0.75f, 0.75f, 0.75f);

        public SelectorSpriteUpdater() {
            selector = GameView.get().sceneElementsReferences.selector.GetComponent<SelectorHandler>();
            selectors = Resources.Load<Sprite>("icons/selectors");
            flatTile = Sprite.Create(selectors.texture, new Rect(0, 0, 64, 96), Vector2.zero, 64);
            wallTile = Sprite.Create(selectors.texture, new Rect(64, 0, 64, 96), Vector2.zero, 64);
        }

        public void updateSprite(Vector3Int position) {
            LocalMap map = GameModel.get().currentLocalModel.localMap;
            if (!map.inMap(position)) return;
            selector.setCurrentZ(position.z);
            BlockType blockType = map.blockType.getEnumValue(position);
            if (blockType.CODE == BlockTypes.SPACE.CODE) {
                setSprite(true, lightGrey);
            } else if (blockType.flat) {
                setSprite(true, Color.white);
            } else {
                setSprite(false, Color.white);
            }
        }

        private void setSprite(bool flat, Color color) {
            selector.toolIcon.transform.localPosition = flat ? flatIconPosition : wallIconPosition;
            selector.frameIcon.sprite = flat ? flatTile : wallTile;
            selector.frameIcon.color = color;
        }
    }
}