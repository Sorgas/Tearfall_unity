using Leopotam.Ecs;
using Tearfall_unity.Assets.scripts.enums;
using UnityEngine;

namespace Tearfall_unity.Assets.scripts.game.view.system.unit {
    // updates visual movement component by movement component
    public class UnitOrientationSystem : IEcsRunSystem {
        public EcsFilter<VisualMovementComponent, MovementComponent> filter;

        // update scene position by model movement progress
        public void Run() {
            foreach (int i in filter) {
                MovementComponent movement = filter.Get2(i);
                VisualMovementComponent visual = filter.Get1(i);
                if (movement.position != visual.lastPosition) {
                    visual.lastPosition = movement.position;
                    if (movement.path.Count > 0) {
                        Vector3Int direction = movement.path[0] - movement.position;
                        if (direction.x != 0 || direction.y != 0) {
                            visual.orientation = getOrientation(direction);
                        }
                    }
                }
            }
        }

        private OrientationEnum getOrientation(Vector3Int directionVector) {
            if (directionVector.x < 0) return OrientationEnum.W;
            if (directionVector.x > 0) return OrientationEnum.E;
            return directionVector.y < 0 ? OrientationEnum.S : OrientationEnum.N;
        }
    }
}