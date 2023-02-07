using System.Collections.Generic;
using types;
using UnityEngine;
using util.lang;

namespace game.model.component {
    public struct ZoneComponent {
        public List<Vector3Int> tiles;
        public ZoneTypeEnum type;
    }

    public struct ZoneVisualComponent {
        public List<Vector3Int> tiles;
    }

    // present when zone is changed but changes not yet processed by visual system
    public struct ZoneUpdatedComponent {
        public List<Vector3Int> tiles;
    }
    
    public struct ZoneDeletedComponent {}

    // stores allowed itemTypes -> materials
    public struct StockpileComponent {
        public MultiValueDictionary<string, string> map;
        public string preset;
        public int priority;
        public bool paused; // new hauling tasks not generated when paused.
    }
}