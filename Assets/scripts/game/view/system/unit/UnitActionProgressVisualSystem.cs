using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;

// controls progress bar for current action of unit
class UnitActionProgressBarUpdateSystem : IEcsRunSystem {
    public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent>.Exclude<UnitVisualProgressBarComponent> createFilter;
    public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent, UnitVisualProgressBarComponent> filter;
    public EcsFilter<UnitVisualComponent, UnitVisualProgressBarComponent>.Exclude<UnitCurrentActionComponent> removeFilter;
    public bool debug = false;

    public void Run() {
        foreach(int i in createFilter) {
            enableProgressBarForUnit(createFilter.GetEntity(i), createFilter.Get1(i));
        }
        foreach(int i in filter) {
            updateProgressBarForUnit(createFilter.Get1(i), createFilter.Get2(i));
        }
        foreach(int i in removeFilter) {
            disableProgressBarForUnit(removeFilter.GetEntity(i), createFilter.Get1(i));
        }
    }

    private void enableProgressBarForUnit(EcsEntity entity, UnitVisualComponent visual) {
        log("creating");
        visual.handler.toggleProgressBar(true);
        entity.Replace(new UnitVisualProgressBarComponent());
    }

    private void updateProgressBarForUnit(UnitVisualComponent visual, UnitCurrentActionComponent actionComponent) {
        log("updating");
        float progress = actionComponent.action.progress / actionComponent.action.maxProgress;
        visual.handler.setProgress(progress);
    }

    private void disableProgressBarForUnit(EcsEntity entity, UnitVisualComponent visual) {
        log("deleting");
        visual.handler.toggleProgressBar(false);
        entity.Del<UnitVisualProgressBarComponent>();
    }

    private void log(string message) {
        if(debug) Debug.Log("[UnitActionProgressBarUpdateSystem]: " + message);
    }
}