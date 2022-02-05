using enums;
using UnityEngine;

namespace game.model.component.task {
    public class DesignationComponents {

        public struct DesignationComponent {
            public DesignationType type;
        }

        public struct DesignationVisualComponent {
            public SpriteRenderer spriteRenderer;
        }
    }
}