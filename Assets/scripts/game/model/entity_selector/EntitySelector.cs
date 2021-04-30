using System.Collections.Generic;
using Assets.scripts.util.geometry;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.model.entity_selector {
    public class EntitySelector {
        public Vector3Int position = new Vector3Int();
        public Vector2Int size = new Vector2Int(1, 1);

        public List<Vector3Int> getSelectorPositions() {
            List<Vector3Int> positions = new List<Vector3Int>();
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    positions.Add(new Vector3Int().set(position).add(x, y, 0));
                }
            }
            return positions;
        }
    }
}