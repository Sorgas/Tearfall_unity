using System.Collections.Generic;
using game.view.system.mouse_tool;
using UnityEngine;

namespace game.model.component {
    public struct ZoneComponent {
        public List<Vector3Int> tiles;
        public ZoneTypeEnum type;
    }

    public struct ZoneVisualComponent {
        public List<Vector3Int> tiles;
    }

    // present when zone is changed but changes not yet processed by visual system
    public struct ZoneVisualUpdatedComponent {
        public List<Vector3Int> tiles;
    }
    
    public struct ZoneDeletedComponent {}
}