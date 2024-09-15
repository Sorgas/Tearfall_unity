using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.view.system.unit {
// System for animating units while they perform actions. When action performed, progress bar is shown over unit and animation is played.
// Attacks are animated directly through attack actions.
// TODO separate into 2 systems for progress bar and animations respectively
class UnitActionVisualSystem : IEcsRunSystem {
    public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent>.Exclude<UnitProgressBarComponent> createProgressBarFilter;
    public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent, UnitProgressBarComponent> updateProgressBarFilter;
    public EcsFilter<UnitVisualComponent, UnitProgressBarComponent>.Exclude<UnitCurrentActionComponent> removeProgressBarFilter;

    public EcsFilter<UnitVisualComponent, UnitCurrentAnimatedActionComponent>.Exclude<UnitAnimationComponent> createAnimationFilter;
    public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent, UnitAnimationComponent> updateAnimationFilter;
    public EcsFilter<UnitVisualComponent, UnitAnimationComponent>.Exclude<UnitCurrentActionComponent> removeAnimationFilter;
    public bool debug = false;

    public void Run() {
        foreach (int i in createProgressBarFilter) {
            createProgressBar(createProgressBarFilter.GetEntity(i), createProgressBarFilter.Get1(i), createProgressBarFilter.Get2(i));
        }
        foreach (int i in updateProgressBarFilter) {
            updateProgressBar(updateProgressBarFilter.Get1(i), updateProgressBarFilter.Get2(i));
        }
        foreach (int i in removeProgressBarFilter) {
            removeProgressBarFilter.Get1(i).handler.toggleProgressBar(false);
            removeProgressBarFilter.GetEntity(i).Del<UnitProgressBarComponent>();
        }
        foreach (int i in createAnimationFilter) {
            createAnimation(createAnimationFilter.GetEntity(i), createAnimationFilter.Get2(i));
        }
        foreach (int i in updateAnimationFilter) {
            updateVisuals(updateAnimationFilter.Get1(i), updateAnimationFilter.Get2(i), ref updateAnimationFilter.GetEntity(i).takeRef<UnitAnimationComponent>());
        }
        foreach (int i in removeAnimationFilter) {
            removeAnimationFilter.GetEntity(i).Del<UnitAnimationComponent>();
        }
    }

    private void createProgressBar(EcsEntity entity, UnitVisualComponent visual, UnitCurrentActionComponent actionComponent) {
        if (actionComponent.action.displayProgressBar) {
            log("creating progress bar");
            visual.handler.toggleProgressBar(true);
        }
        entity.Replace(new UnitProgressBarComponent());
    }

    private void updateProgressBar(UnitVisualComponent visual, UnitCurrentActionComponent currentAction) {
        log("updating");
        if (currentAction.action == null) {
            Debug.LogError("Trying to update unit action visual progress bar when unit action is null!");
            return;
        }
        if (currentAction.action.maxProgress == 0) {
            visual.handler.setProgress(0);
        } else {
            float progress = currentAction.action.progress / currentAction.action.maxProgress;
            visual.handler.setProgress(progress);
        }
    }

    private void createAnimation(EcsEntity entity, UnitCurrentAnimatedActionComponent action) {
        UnitAnimationComponent component = new UnitAnimationComponent {
            animationName = action.animationName,
            animationDelay = 0, animationDelayMax = 1
        };
        entity.Replace(component);
    }

    private void updateVisuals(UnitVisualComponent visual, UnitCurrentActionComponent currentAction, ref UnitAnimationComponent animationComponent) {
        if (animationComponent.animationDelayMax > 0) {
            animationComponent.animationDelay += Time.deltaTime;
            if (animationComponent.animationDelay >= animationComponent.animationDelayMax) {
                animationComponent.animationDelay %= animationComponent.animationDelayMax;
                visual.handler.actionAnimator.Play(animationComponent.animationName);
            }
        }
    }

    private void log(string message) {
        if (debug) Debug.Log("[UnitActionProgressBarUpdateSystem]: " + message);
    }
}
}