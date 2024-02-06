using game.model.container;
using game.model.localmap.passage;
using game.view;
using UnityEngine;
using util.geometry.bounds;

namespace game.model.localmap {
    // stores arrays of blocks, passage and area values
    public class LocalMap : LocalModelContainer {
        public readonly PassageMap passageMap; // not saved to savegame,
        public readonly BlockTypeMap blockType;
        
        public readonly SubstrateMap substrateMap; 

        public readonly LocalMapUtil util;
        public readonly IntBounds3 bounds; // inclusive
        public readonly Vector3Int sizeVector; // exclusive

        // public LightMap light;
        //private LocalTileMapUpdater localTileMapUpdater;              // not saved to savegame,

        public LocalMap(int xSize, int ySize, int zSize, LocalModel model) : base(model) {
            bounds = new IntBounds3(0, 0, 0, xSize - 1, ySize - 1, zSize - 1);
            sizeVector = new Vector3Int(xSize, ySize, zSize);
            blockType = new BlockTypeMap(this);
            util = new LocalMapUtil(this);
            passageMap = new PassageMap(model, this);
            substrateMap = new(model);
        }

        public void init() {
            passageMap.init();
            //Logger.LOADING.logDebug("Initing local map");
            //light.initLight();
            //localTileMapUpdater = new LocalTileMapUpdater();
            //localTileMapUpdater.flushLocalMap();
        }
        
        public void updateTile(Vector3Int position, bool updateRamps) {
            passageMap.update(position);
            GameView.get().tileUpdater?.updateTile(position.x, position.y, position.z, updateRamps);
            model.itemContainer.addPositionForUpdate(new Vector3Int(position.x, position.y, position.z));
        }

        public bool inMap(int x, int y, int z) => bounds.isIn(x, y, z);

        public bool inMap(Vector3Int vector) => inMap(vector.x, vector.y, vector.z);
    }
}