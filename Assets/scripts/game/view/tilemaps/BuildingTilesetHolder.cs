using System.Collections.Generic;
using types.building;
using UnityEngine;
using util.lang;

namespace game.view.tilemaps {
    public class BuildingTilesetHolder : Singleton<BuildingTilesetHolder> {
        public Dictionary<BuildingType, BuildingSprites> sprites = new();

        public BuildingTilesetHolder() {
            loadAll();
        }

        private void loadAll() {
            Dictionary<string, Sprite> spritesCache = new();
            BuildingTilesetSlicer slicer = new();
            foreach (BuildingType type in BuildingTypeMap.get().all()) {
                if(!spritesCache.ContainsKey(type.tileset))
                    spritesCache[type.tileset] = TexturePacker.createSpriteFromAtlas(type.tileset);
                Sprite sprite = spritesCache[type.tileset];
                sprites.Add(type, slicer.slice(type, sprite));
            }
        }
    }

    public class BuildingSprites {
        public readonly Sprite n;
        public readonly Sprite s;
        public readonly Sprite e;
        public readonly Sprite w;

        public BuildingSprites(Sprite n, Sprite s, Sprite e, Sprite w) {
            this.n = n;
            this.s = s;
            this.e = e;
            this.w = w;
        }
    }
}