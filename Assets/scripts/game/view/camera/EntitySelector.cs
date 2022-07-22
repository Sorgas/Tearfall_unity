using game.model;
using game.model.localmap;
using game.view.system.mouse_tool;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;

namespace game.view {
    public class EntitySelector {
        public Vector3Int position = new();
        public readonly IntBounds3 bounds = new();
        public Vector2Int size = new(1, 1);
        public readonly ValueRangeInt zRange = new(); // range for current z in model units
        
        public Vector3Int updatePosition(Vector3Int position) {
            this.position.set(bounds.putInto(position));
            MouseToolManager.get().mouseMoved(this.position);
            return this.position;
        }

        public int changeLayer(int dz) => setLayer(position.z + dz);

        public int setLayer(int z) {
            int oldZ = position.z;
            position.z = zRange.clamp(z);
            if (oldZ != position.z) {
                GameView.get().tileUpdater.updateLayersVisibility(position.z);
            }
            return position.z - oldZ;
        }

        public void changeSelectorSize(int x, int y) {
            size.Set(x, y);
            updateBounds();
            updatePosition(position);
        }

        public void updateBounds() {
            LocalMap map = GameModel.localMap;
            bounds.set(0, 0, 0, map.bounds.maxX - size.x + 1, map.bounds.maxY - size.y + 1, map.bounds.maxZ);
        }
    }
}