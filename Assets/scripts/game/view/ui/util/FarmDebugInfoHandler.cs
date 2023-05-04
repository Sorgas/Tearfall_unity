using System.Collections.Generic;
using game.model.component;
using Leopotam.Ecs;
using TMPro;
using UnityEngine;
using util.lang.extension;

namespace game.view.ui.util {
    public class FarmDebugInfoHandler : MonoBehaviour {
        public TextMeshProUGUI debugText;
        public EcsEntity entity;

        public void Update() {
            if (debugText.gameObject.activeInHierarchy && entity != EcsEntity.Null) {
                ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();
                
                debugText.text = "locked: "; 
                foreach (Vector3Int tile in tracking.locked.Keys) {
                    debugText.text += tile + " ";
                }
                debugText.text += " \ntiles:";
                foreach (KeyValuePair<Vector3Int,string> pair in tracking.tilesToTask) {
                    debugText.text += "\n    " + pair.Key + " " + pair.Value;
                    
                }
                // foreach (var taskType in tracking.tiles.Keys) {
                //     if (tracking.tiles[taskType].Count > 0) {
                //         debugText.text += "\n    " + taskType + " " + tracking.tiles[taskType].Count;
                //     }
                // }
            }
        }
    }
}