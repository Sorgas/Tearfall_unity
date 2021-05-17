using Assets.scripts.enums;
using Assets.scripts.game.model;
using Assets.scripts.game.model.localmap;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.model.entity_selector {
    public class EntitySelectorSystem {
        // public final EntitySelector selector; // TODO not required before large building are implemented
        public EntitySelector selector;
        private Vector3Int cachePosition = new Vector3Int();
        public bool allowChangingZLevelOnSelection = true;
        public bool allowTwoDimensionsOnSelection = true;

        public EntitySelectorSystem() {
            // selector = GameModel.get().selector;
            // selector.add(new SelectionAspect(selector));
            // selector.add(new BoxSelectionAspect(selector));
            // selector.add(new RenderAspect(AtlasesEnum.ui_tiles.getBlockTile(0, 0)));
            // inputHandler = new EntitySelectorInputHandler(this);
        }

        public void handleSelection() {
            // selector.get(SelectionAspect).tool.handleSelection(selector.get(BoxSelectionAspect.class).getBox());
        }

        public void rotateSelector(bool clockwise) {
            // selector.get(SelectionAspect).tool.rotate(clockwise);
            // setSelectorPosition(selector.position);
        }

        // changes selector position by delta. takes map bounds into account, returns applied delta
        public Vector3Int moveSelector(int dx, int dy, int dz) {
            Vector3Int prev = selector.position;
            setSelectorPosition(selector.position.x + dx, selector.position.y + dy, selector.position.z + dz);
            return selector.position - prev;
        }

        // updates selector position, applies map size restriction. returns new position of selector
        public Vector3Int setSelectorPosition(int x, int y, int z) {
            selector.position.Set(x, y, z);
            GameModel.get().localMap.normalizeRectangle(ref selector.position, selector.size.x, selector.size.y); // selector should not move out of map
            ensureMovementRestrictions();
            return selector.position;
        }

        public Vector3Int setSelectorPosition(Vector3Int position) {
            return setSelectorPosition(position.x, position.y, position.z);
        }

        public void placeSelectorAtMapCenter() {
            LocalMap localMap = GameModel.get().localMap;
            selector.position.x = localMap.xSize / 2;
            selector.position.y = localMap.ySize / 2;
            for (int z = localMap.zSize - 1; z >= 0; z--) {
                if (localMap.blockType.get(selector.position.x, selector.position.y, z) != BlockTypeEnum.SPACE.CODE) {
                    selector.position.z = z;
                    break;
                }
            }
        }

        /**
         * Cancels selection box or sets tool to SELECTION.
         * @return false, if there were SELECTION tool with no selection box already
         */
        public bool cancelSelection() {
            //             BoxSelectionAspect boxAspect = selector.get(BoxSelectionAspect.class);
            // if (boxAspect.boxStart != null) { // cancel selection if box started
            //     boxAspect.boxStart = null;
            //     return true;
            // }
            //     SelectionAspect selectionAspect = selector.get(SelectionAspect.class);
            // if (selectionAspect.tool != SelectionTools.SELECT) { // set tool to default
            //     selectionAspect.set(SelectionTools.SELECT);
            //     if (GameSettings.CLOSE_TOOLBAR_ON_TOOL_CANCEL.VALUE == 1) GameMvc.view().toolbarStage.toolbar.reset();
            //     return true;
            // }

            return false;
        }

        /**
         * Corrects selector position if selection is active.
         */
        private void ensureMovementRestrictions() {

            //     Position boxStart = selector.get(BoxSelectionAspect.class).boxStart;
            // if (boxStart == null) return; // no correction for no selection
            // if (!allowChangingZLevelOnSelection) selector.position.z = boxStart.z; // should stay on same z level
            // if (!allowTwoDimensionsOnSelection) { // set min delta to 0
            //     int dx = Math.abs(boxStart.x - selector.position.x);
            //     int dy = Math.abs(boxStart.y - selector.position.y);
            //     if (dx == 0 || dy == 0) return;
            //     if (dx < dy) {
            //         selector.position.x = boxStart.x;
            //     } else {
            //         selector.position.y = boxStart.y;
            //     }
            // }
        }
    }
}