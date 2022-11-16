using game.model.component.unit;
using game.view.ui;
using Leopotam.Ecs;

public class UnitMenuUpdateSystem : IEcsRunSystem {
    public EcsFilter<UnitVisualComponent> filter;

    public void Run() {
        if (WindowManager.get().activeWindowName != UnitMenuHandler.NAME) return;
        UnitMenuHandler menu = (UnitMenuHandler)WindowManager.get().activeWindow;
        foreach (int i in filter) {
            EcsEntity unit = filter.GetEntity(i);
            if (menu.unit == unit) {
                menu.initFor(unit);
                return;
            }
        }
    }
}