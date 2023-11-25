using static types.PassageTypes;

namespace game.model.localmap.passage {
// Standard area initializer for settlers
public class AreaInitializerGroundPassableDoors : AbstractAreaInitializer {

    protected override bool tilePassable(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE || passage.getPassage(x, y, z) == DOOR.VALUE;
    }
}
}