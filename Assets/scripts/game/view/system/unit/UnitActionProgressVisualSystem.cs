using game.model.component.unit;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

// controls progress bar for current action of unit
// plays animation for action
namespace game.view.system.unit {
    class UnitActionVisualSystem : IEcsRunSystem {
        public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent>.Exclude<UnitAnimationComponent> createFilter;
        public EcsFilter<UnitVisualComponent, UnitCurrentActionComponent, UnitAnimationComponent> filter;
        public EcsFilter<UnitVisualComponent, UnitAnimationComponent>.Exclude<UnitCurrentActionComponent> removeFilter;
        public bool debug = false;

        public void Run() {
            foreach(int i in createFilter) {
                enableVisuals(createFilter.GetEntity(i), createFilter.Get1(i), createFilter.Get2(i));
            }
            foreach(int i in filter) {
                updateVisuals(filter.Get1(i), filter.Get2(i), ref filter.GetEntity(i).takeRef<UnitAnimationComponent>());
            }
            foreach(int i in removeFilter) {
                disableVisuals(removeFilter.GetEntity(i), removeFilter.Get1(i));
            }
        }

        private void enableVisuals(EcsEntity entity, UnitVisualComponent visual, UnitCurrentActionComponent actionComponent) {
            log("creating");
            visual.handler.toggleProgressBar(true);
            UnitAnimationComponent component = new UnitAnimationComponent { animationName = actionComponent.action.animation, 
                animationDelay = 0, animationDelayMax = 1};
            entity.Replace(component);
        }

        private void updateVisuals(UnitVisualComponent visual, UnitCurrentActionComponent currentAction, ref UnitAnimationComponent animationComponent) {
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
            updateAnimation(visual, ref animationComponent);
        }

        private void updateAnimation(UnitVisualComponent visual, ref UnitAnimationComponent animationComponent) {
            if (animationComponent.animationDelayMax > 0) {
                animationComponent.animationDelay += Time.deltaTime;
                if (animationComponent.animationDelay < animationComponent.animationDelayMax) return;
                animationComponent.animationDelay %= animationComponent.animationDelayMax;
            }
            visual.handler.actionAnimator.Play(animationComponent.animationName);
        }
        
        private void disableVisuals(EcsEntity entity, UnitVisualComponent visual) {
            log("deleting");
            visual.handler.toggleProgressBar(false);
            entity.Del<UnitAnimationComponent>();
        }

        private void log(string message) {
            if(debug) Debug.Log("[UnitActionProgressBarUpdateSystem]: " + message);
        }
    }
}