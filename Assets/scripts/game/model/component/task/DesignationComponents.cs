using enums;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.component.task {
    public class DesignationComponents {

        public struct DesignationComponent {
            public DesignationType type;
        }

        public struct OpenDesignation {
        }

        public struct DesignationFinished {
        }

        public struct DesignationVisualComponent {
            public SpriteRenderer spriteRenderer;
        }
    }
}