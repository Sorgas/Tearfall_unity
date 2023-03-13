using game.model.localmap.passage;
using game.view;
using UnityEngine;
using util.geometry.bounds;

namespace game.model.localmap {
    // stores arrays of blocks, passage and area values
    public class LocalMap : LocalModelComponent {
        public readonly BlockTypeMap blockType;
        public readonly PassageMap passageMap; // not saved to savegame,
        public readonly LocalMapUtil util;
        public readonly IntBounds3 bounds; // inclusive
        public readonly Vector3Int sizeVector; // exclusive
        public readonly SubstrateMap substrateMap; 
        public readonly int[,] lightMap;

        // public LightMap light;
        //private LocalTileMapUpdater localTileMapUpdater;              // not saved to savegame,

        public LocalMap(int xSize, int ySize, int zSize, LocalModel model) : base(model) {
            bounds = new IntBounds3(0, 0, 0, xSize - 1, ySize - 1, zSize - 1);
            sizeVector = new Vector3Int(xSize, ySize, zSize);
            blockType = new BlockTypeMap(this);
            util = new LocalMapUtil(this);
            passageMap = new PassageMap(model, this);
            substrateMap = new(model);
            lightMap = new int[xSize, ySize];
        }

        public void init() {
            passageMap.init();
            //Logger.LOADING.logDebug("Initing local map");
            //light.initLight();
            //localTileMapUpdater = new LocalTileMapUpdater();
            //localTileMapUpdater.flushLocalMap();
        }
        
        public void updateTile(int x, int y, int z, bool updateRamps) {
            if (passageMap != null) passageMap.updater.update(x, y, z);
            if (GameView.get().tileUpdater != null ) GameView.get().tileUpdater.updateTile(x, y, z, updateRamps);
            model.updateOnLocalMapChange(new Vector3Int(x, y, z));
        }

        public bool inMap(int x, int y, int z) => bounds.isIn(x, y, z);

        public bool inMap(Vector3Int vector) => inMap(vector.x, vector.y, vector.z);
    }
}