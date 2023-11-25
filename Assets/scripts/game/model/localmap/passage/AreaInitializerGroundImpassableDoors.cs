using static types.PassageTypes;

namespace game.model.localmap.passage {
// Some creatures cannot use doors
public class AreaInitializerGroundImpassableDoors : AbstractAreaInitializer {

    protected override bool tilePassable(int x, int y, int z) {
        return passage.getPassage(x, y, z) == PASSABLE.VALUE;
    }
}
}