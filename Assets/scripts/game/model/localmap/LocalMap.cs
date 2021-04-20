using System;
using Assets.scripts.game.model.localmap.passage;
using Assets.scripts.util.geometry;
using static Assets.scripts.enums.PassageEnum;

namespace Assets.scripts.game.model.localmap {
    public class LocalMap {
        public readonly BlockTypeMap blockType;
        public readonly IntBounds3 bounds;

        // public LightMap light;
        public PassageMap passageMap;                                 // not saved to savegame,
        //private LocalTileMapUpdater localTileMapUpdater;              // not saved to savegame,

        public readonly int xSize;
        public readonly int ySize;
        public readonly int zSize;

        public LocalMap(int xSize, int ySize, int zSize) {
            this.xSize = xSize;
            this.ySize = ySize;
            this.zSize = zSize;
            blockType = new BlockTypeMap(this);
            //light = new LightMap(this);
            bounds = new IntBounds3(0, 0, 0, xSize - 1, ySize - 1, zSize - 1);
        }

        public void init() {
            //Logger.LOADING.logDebug("Initing local map");
            //light.initLight();
            //localTileMapUpdater = new LocalTileMapUpdater();
            //localTileMapUpdater.flushLocalMap();
        }

        public void initAreas() {
            passageMap = new PassageMap(this);
            passageMap.init();
        }

        public bool inMap(int x, int y, int z) {
            return !(x < 0 || y < 0 || z < 0 || x >= xSize || y >= ySize || z >= zSize);
        }

        public bool inMap(IntVector3 position) {
            return inMap(position.x, position.y, position.z);
        }

        public bool inMap(IntVector2 vector) {
            return inMap(vector.x, vector.y, 0);
        }

        public bool isBorder(int x, int y) {
            return x == 0 || y == 0 || x == xSize - 1 || y == ySize - 1;
        }

        public bool isBorder(IntVector3 position) {
            return isBorder(position.x, position.y);
        }

        // change postition to move it inside map
        public void normalizePosition(IntVector3 position) {
            normalizeRectangle(position, 1, 1);
        }

        // change position to move rectangle with position in [0,0] inside map
        public void normalizeRectangle(IntVector3 position, int width, int height) {
            position.x = Math.Min(Math.Max(0, position.x), xSize - width);
            position.y = Math.Min(Math.Max(0, position.y), ySize - height);
            position.z = Math.Min(Math.Max(0, position.z), zSize - 1);
        }

        public bool isWalkPassable(IntVector3 pos) {
            return isWalkPassable(pos.x, pos.y, pos.z);
        }

        public bool isWalkPassable(int x, int y, int z) {
            //TODO reuse
            return passageMap.getPassage(x, y, z) == PASSABLE.VALUE;
        }

        public bool isFlyPassable(int x, int y, int z) {
            //TODO
            return inMap(x, y, z) && blockType.getEnumValue(x, y, z).CODE != IMPASSABLE.VALUE;
        }

        public void updateTile(IntVector3 position, bool updateRamps) {
            updatePassage(position);
            if(GameView.get().tileUpdater != null) GameView.get().tileUpdater.updateTile(position, false);
        }

        public void updatePassage(IntVector3 position) {
            if (passageMap != null) passageMap.updater.update(position.x, position.y, position.z);
        }
    }
}