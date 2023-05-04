using System.Collections.Generic;
using UnityEngine;

namespace game.model.localmap.update {
    public class TileUpdateUtil {
        private LocalModel model;
        private readonly ZoneUpdateUtil zoneUpdateUtil;

        public TileUpdateUtil(LocalModel model) {
            this.model = model;
            zoneUpdateUtil = new(model);
        }

        public void updateTile(Vector3Int tile) {
            zoneUpdateUtil.updateZone(tile);
        }

        public void updateTiles(ICollection<Vector3Int> tiles) {
            foreach (Vector3Int tile in tiles) {
                zoneUpdateUtil.updateZone(tile);
            }
        }
    }
}