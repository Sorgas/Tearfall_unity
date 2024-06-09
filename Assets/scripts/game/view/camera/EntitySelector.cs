using game.model;
using game.view.system.mouse_tool;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;

namespace game.view.camera {
// Selector size can be more than 1 tile (when designating building)
// This object only maintains position consistent with map bounds and size.
public class EntitySelector {
    public Vector3Int position => internalPosition;

    private Vector3Int internalPosition = new(); // used by mouse tools
    private Vector2Int size = new(1, 1);
    private readonly IntBounds3 bounds = new(); // inclusive
    // private readonly ValueRangeInt zRange = new(); // range for current z in model units

    public void init() {
        // zRange.set(0, GameModel.get().currentLocalModel.localMap.bounds.maxZ);
        updateBounds();
    }

    public Vector3Int setPosition(Vector3Int newPosition) {
        int oldZ = internalPosition.z;
        internalPosition.set(bounds.putInto(newPosition));
        if (oldZ != internalPosition.z) {
            GameView.get().tileUpdater.updateLayersVisibility(internalPosition.z);
        }
        MouseToolManager.get().mouseMoved(internalPosition);
        return internalPosition;
    }
    
    // // accepts model position
    // public Vector3Int setPositionOnSameLayer(Vector3Int position) {
    //     internalPosition.set(bounds.putInto(position));
    //     MouseToolManager.get().mouseMoved(internalPosition);
    //     return internalPosition;
    // }

    public int changeLayer(int dz) => setLayer(internalPosition.z + dz);
    
    public int setLayer(int z) {
        int oldZ = internalPosition.z;
        internalPosition.z = z;
        internalPosition = bounds.putInto(internalPosition);
        
        if (oldZ != internalPosition.z) {
            GameView.get().tileUpdater.updateLayersVisibility(internalPosition.z);
        }
        
        return internalPosition.z - oldZ;
    }

    public void changeSelectorSize(Vector2Int newSize) {
        size.Set(newSize[0], newSize[1]);
        updateBounds();
        setPosition(internalPosition); // to move selector if was on maps edge and size increased
    }

    public void updateBounds() {
        IntBounds3 mapBounds = GameModel.get().currentLocalModel.localMap.bounds; // inclusive
        bounds.set(0, 0, 0, mapBounds.maxX - size.x + 1, mapBounds.maxY - size.y + 1, mapBounds.maxZ);
    }
}
}