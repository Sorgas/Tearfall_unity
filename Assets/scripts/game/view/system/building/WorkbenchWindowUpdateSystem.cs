using game.input;
using game.model.component;
using game.model.component.building;
using game.view.ui;
using game.view.ui.workbench;
using Leopotam.Ecs;

// updates opened window of workbench if selected workbench state updates
namespace game.view.system.building {
    class WorkbenchWindowUpdateSystem : IEcsRunSystem {
        public EcsFilter<WorkbenchComponent, UiUpdateComponent> filter;

        public void Run() {
            foreach (int i in filter) {
                EcsEntity entity = filter.GetEntity(i);
                entity.Del<UiUpdateComponent>();
                if (WindowManager.get().activeWindowName == WorkbenchWindowHandler.name) {
                    WorkbenchWindowHandler window = (WorkbenchWindowHandler) WindowManager.get().activeWindow;
                    if (window.entity == entity) {
                        window.updateState();
                    }
                }
            }
        }
    }
}