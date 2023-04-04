using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.localmap;
using game.model.util.validation;
using generation.zone;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry.bounds;
using util.lang;
using util.lang.extension;

namespace game.model.container {
    public class ZoneContainer : LocalModelContainer {
        public HashSet<EcsEntity> zoneSet = new();
        public Dictionary<Vector3Int, EcsEntity> zones = new();
        private ZoneGenerator zoneGenerator = new();
        private bool debug = true;
        public readonly StockpileInitializer stockpileInitializer = new();

        private List<int> freeNumbers = new();
        private int nextNumber = 1;

        public ZoneContainer(LocalModel model) : base(model) { }

        // creates zone and overwrites other zones
        public void createZone(IntBounds3 bounds, ZoneTypeEnum type) {
            PositionValidator validator = ZoneTypes.get(type).positionValidator;
            EcsEntity zone = zoneGenerator.generate(bounds, type, getFreeNumber(), model.createEntity(), model);
            List<Vector3Int> tiles = zone.Get<ZoneComponent>().tiles;
            zone.Replace(new ZoneUpdatedComponent { tiles = new List<Vector3Int>(tiles) });
            zone.Replace(new ZoneVisualUpdatedComponent { tiles = new List<Vector3Int>(tiles) });
            foreach (Vector3Int tile in tiles) {
                if (validator.validate(tile, model)) {
                    removeTileFromZone(tile); // remove from previous zone
                    zones.Add(tile, zone);
                }
            }
            log("zone " + type + " created");
        }

        // removes tiles of zones, if all tiles are removed from zone it is deleted
        public void eraseZones(IntBounds3 bounds) {
            MultiValueDictionary<EcsEntity, Vector3Int> affectedZones = new();
            bounds.iterate(position => {
                removeTileFromZone(position);
            });
            log("zones erased");
        }
        
        // TODO cancel all tasks of zone
        private void deleteZone(EcsEntity zone) {
            ZoneComponent component = zone.take<ZoneComponent>();
            foreach (Vector3Int tile in component.tiles) {
                zones.Remove(tile);
            }
            freeNumbers.Add(component.number);
            zone.Replace(new ZoneDeletedComponent());
        }

        public void addTilesToZone(EcsEntity zone, IntBounds3 bounds) {
            ZoneComponent zoneComponent = zone.take<ZoneComponent>();
            bounds.iterate(tile => {
                if (zones.ContainsKey(tile)) {
                    removeTileFromZone(tile);
                }
                zones.Add(tile, zone);
                zoneComponent.tiles.Add(tile);
                addTileToBeUpdated(zone, tile);
            });
        }

        // removes tile from its current zone, deletes zone if it was last tile
        public void removeTileFromZone(Vector3Int tile) {
            if (!zones.ContainsKey(tile)) return;
            EcsEntity zone = zones[tile];
            ZoneComponent component = zone.take<ZoneComponent>();
            component.tiles.Remove(tile); // TODO cancel tasks related to this tile
            zones.Remove(tile);
            addTileToBeUpdated(zone, tile);
            if (component.tiles.Count == 0) deleteZone(zone);
        }

        private void addTileToBeUpdated(EcsEntity zone, Vector3Int tile) {
            if (!zone.Has<ZoneUpdatedComponent>()) zone.Replace(new ZoneUpdatedComponent { tiles = new() });
            zone.Get<ZoneUpdatedComponent>().tiles.Add(tile);
            if (!zone.Has<ZoneVisualUpdatedComponent>()) zone.Replace(new ZoneVisualUpdatedComponent { tiles = new() });
            zone.Get<ZoneVisualUpdatedComponent>().tiles.Add(tile);
        }

        public EcsEntity getZone(Vector3Int position) {
            return zones.ContainsKey(position) ? zones[position] : EcsEntity.Null;
        }

        public HashSet<EcsEntity> getZones(IntBounds3 bounds) {
            HashSet<EcsEntity> set = new();
            bounds.iterate(position => {
                if (zones.ContainsKey(position)) set.Add(zones[position]);
            });
            return set;
        }

        // cancels task which locked given tile.
        private void cancelZoneTask(EcsEntity zone, Vector3Int tile) {
            var tracking = zone.take<ZoneTrackingComponent>();
            if (tracking.locked.ContainsKey(tile)) {
                tracking.locked[tile].Replace(new TaskFinishedComponent { status = TaskStatusEnum.CANCELED });
            }
        }
        
        private int getFreeNumber() {
            int value;
            if (freeNumbers.Count != 0) {
                value = freeNumbers[0];
                freeNumbers.RemoveAt(0);
            } else {
                value = nextNumber;
                nextNumber++;
            }
            return value;
        }
        
        private void log(string message) {
            Debug.Log("[ZoneContainer]: " + message);
        }
    }
}