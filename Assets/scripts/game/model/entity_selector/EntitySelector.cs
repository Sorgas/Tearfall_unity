using System.Collections.Generic;
using Assets.scripts.util.geometry;

namespace Tearfall_unity.Assets.scripts.game.model.entity_selector {
    public class EntitySelector {
        public IntVector3 position = new IntVector3();
        public IntVector2 size = new IntVector2(1, 1);

        public List<IntVector3> getSelectorPositions() {
            List<IntVector3> positions = new List<IntVector3>();
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    positions.Add(IntVector3.add(position, x, y, 0));
                }
            }
            return positions;
        }
    }
}