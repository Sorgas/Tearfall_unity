using game.input;
using game.model.component.unit;
using game.view.ui;
using game.view.ui.unit_menu;
using Leopotam.Ecs;

namespace game.view.system.unit {
// TODO make menus to be updated only on changes in the entity
public class UnitMenuUpdateSystem : IEcsRunSystem {

    public void Run() {
        if (WindowManager.get().activeWindowName == UnitMenuHandler.NAME) {
            UnitMenuHandler menu = (UnitMenuHandler)WindowManager.get().activeWindow;
            menu.updateFor(menu.unit);
        }
    }
}
}