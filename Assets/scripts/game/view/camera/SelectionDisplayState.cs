using UnityEngine;
using util.geometry;

namespace game.view.camera {
    
    // stores bounds of tiles, displayed as selected
    // adds new tiles when selection is updated.
    public class SelectionDisplayState {
        private IntBounds3 oldBounds;

        public void startSelection(Vector3Int pos) {
            
        }
        
        public void update(IntBounds3 newBounds) {
            
        }

        public void reset() {
            oldBounds.set(0, 0, 0, 0, 0, 0);
        }

        private void hide() {
            oldBounds.iterate((x, y, z) => {
                // hide tile
            });
        }
    }
}