using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using game.view.system.mouse_tool;
using generation;
using Leopotam.Ecs;
using UnityEngine;
using util.geometry.bounds;
using util.lang;
using util.lang.extension;

namespace game.model.container {
    public class ZoneContainer : LocalMapModelComponent {
        public HashSet<EcsEntity> zoneSet = new();
        public Dictionary<Vector3Int, EcsEntity> zones = new();
        private ZoneGenerator zoneGenerator = new();
        private bool debug = true;
        
        public ZoneContainer(LocalModel model) : base(model) { }

        public void createZone(IntBounds3 bounds, ZoneTypeEnum type) {
            EcsEntity zone = zoneGenerator.generate(bounds, type, model.createEntity(), model);
            zone.Replace(new ZoneVisualUpdatedComponent{tiles = new List<Vector3Int>()});
            log("zone " + type + " created");
        }

        // deletes only tiles of zones
        public void eraseZones(IntBounds3 bounds) {
            MultiValueDictionary<EcsEntity, Vector3Int> affectedZones = new();
            bounds.iterate(position => {
                if (zones.ContainsKey(position)) {
                    affectedZones.add(zones[position], position);
                    zones[position].take<ZoneComponent>().tiles.Remove(position);
                    zones.Remove(position);
                }
            });
            foreach (EcsEntity zone in affectedZones.Keys) {
                zone.Replace(new ZoneVisualUpdatedComponent { tiles = new List<Vector3Int>(affectedZones[zone]) });
            }
            log("zones erased");
        }

        public void deleteZone(EcsEntity zone) {
            
        }
        
        public EcsEntity getZone(Vector3Int position) {
            return zones.ContainsKey(position) ? zones[position] : EcsEntity.Null;
        }

        private void log(string message) {
            Debug.Log("[ZoneContainer]: " + message);
        }
    }
}