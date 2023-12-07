using util;

namespace game.model.localmap.passage {
public abstract class AbstractArea : UtilByteArrayWithCounter {
    public readonly AbstractAreaUpdater updater;
    protected readonly LocalMap map;
    protected readonly PassageMap passage;
    
    protected AbstractArea(LocalMap map, PassageMap passage) : base(map.sizeVector) {
        this.map = map;
        this.passage = passage;
        createInitializer().initArea(map, passage, this);
        updater = createUpdater();
    }

    protected abstract bool tileCanHaveArea(int x, int y, int z);

    protected abstract AbstractAreaUpdater createUpdater();

    protected abstract AbstractAreaInitializer createInitializer();
}
}