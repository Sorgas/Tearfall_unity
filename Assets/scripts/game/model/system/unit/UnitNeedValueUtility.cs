using game.model.component.unit;
using Leopotam.Ecs;
using types.unit.need;
using util.lang.extension;

namespace game.model.system.unit {
// Units gain different status effects when their need values change.
// This class maintains correct effects on a unit when values change. 
// all need value changes should be made through this class. 
public class UnitNeedValueUtility {
    public void changeRestValue(EcsEntity unit, float delta) {
        changeRestValue(unit, ref unit.takeRef<UnitNeedComponent>(), delta);
    }

    public void changeRestValue(EcsEntity unit, ref UnitNeedComponent needs, float delta) {
        needs.rest += delta;
        needs.restPriority = Needs.rest.getPriority(needs.rest);
    }
}
}