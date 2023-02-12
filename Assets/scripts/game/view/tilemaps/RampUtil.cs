using game.model.localmap;
using types;

namespace game.view.tilemaps {
    public class RampUtil {
        private LocalMap map;

        public RampUtil(LocalMap map) {
            this.map = map;
        }

        // Chooses ramp tile by surrounding walls. Don't touch!
        public string selectRampPrefix(int x, int y, int z) {
            uint walls = observeWalls(x, y, z);
            if ((walls & 0b00001010) == 0b00001010) return "SW";
            if ((walls & 0b01010000) == 0b01010000) return "NE";
            if ((walls & 0b00010010) == 0b00010010) return "SE";
            if ((walls & 0b01001000) == 0b01001000) return "NW";
            if ((walls & 0b00010000) != 0) return "E";
            if ((walls & 0b01000000) != 0) return "N";
            if ((walls & 0b00000010) != 0) return "S";
            if ((walls & 0b00001000) != 0) return "W";
            if ((walls & 0b10000000) != 0) return "CNE";
            if ((walls & 0b00000100) != 0) return "CSE";
            if ((walls & 0b00100000) != 0) return "CNW";
            if ((walls & 0b00000001) != 0) return "CSW";
            return "C";
        }

        // Counts walls to choose ramp type and orientation.
        private uint observeWalls(int cx, int cy, int cz) {
            uint bitpos = 1;
            uint walls = 0;
            for (int y = cy - 1; y <= cy + 1; y++) {
                for (int x = cx - 1; x <= cx + 1; x++) {
                    if (x == cx && y == cy) continue;
                    if (map.blockType.get(x, y, cz) == BlockTypes.WALL.CODE) walls |= bitpos;
                    bitpos *= 2; // shift to 1 bit
                }
            }
            return walls;
        }
    }
}