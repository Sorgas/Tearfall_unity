using System.Collections.Generic;
using game.model.component;
using game.model.component.task;
using game.model.localmap;
using generation.zone;
using Leopotam.Ecs;
using types;
using types.action;
using UnityEngine;
using util.geometry.bounds;
using util.lang.extension;

namespace game.model.container {

    // stores all zones. performs operations on zones.
    public class ZoneContainer : LocalModelContainer {
        private readonly HashSet<EcsEntity> allZones = new();
        private readonly Dictionary<Vector3Int, EcsEntity> zones = new();
        private ZoneGenerator zoneGenerator = new();
        // public readonly StockpileInitializer stockpileInitializer = new();
        private List<int> freeNumbers = new();
        private int nextNumber = 1;
        private bool debug = true;

        public ZoneContainer(LocalModel model) : base(model) { }

        // creates zone and overwrites other zones
        public void createZone(IntBounds3 bounds, ZoneTypeEnum type) {
            EcsEntity zone = zoneGenerator.generate(bounds, type, getFreeNumber(), model);
            if (zone != EcsEntity.Null) {
                List<Vector3Int> tiles = zone.Get<ZoneComponent>().tiles;
                foreach (Vector3Int tile in tiles) {
                    removeTileFromZone(tile); // remove from previous zone
                    zones.Add(tile, zone);
                    model.updateUtil.updateTile(tile);
                }
                allZones.Add(zone);
                zone.Replace(new ZoneVisualUpdateComponent { tiles = new List<Vector3Int>(tiles) });
                log("zone " + type + " created, size " + tiles.Count);
            } else {
                log("zone " + type + " not created, no valid tiles in bounds");
            }
        }

        // removes tiles of zones, if all tiles are removed from zone it is deleted
        public void eraseZones(IntBounds3 bounds) {
            bounds.iterate(removeTileFromZone);
            log("zones erased");
        }

        public void deleteZone(EcsEntity zone) {
            ZoneComponent component = zone.take<ZoneComponent>();
            foreach (Vector3Int tile in component.tiles) {
                removeTileFromZone(tile);
            }
            allZones.Remove(zone);
            freeNumbers.Add(component.number);
            ZoneTrackingComponent tracking = zone.take<ZoneTrackingComponent>();
            foreach (EcsEntity task in tracking.totalTasks) {
                cancelZoneTask(task, "zone deleted");
                log("deleted zone task canceled " + task.name());
            }
            zone.Destroy();
        }

        public void addTilesToZone(EcsEntity zone, IntBounds3 bounds) {
            ZoneComponent zoneComponent = zone.take<ZoneComponent>();
            bounds.iterate(tile => {
                if (zones.ContainsKey(tile)) {
                    removeTileFromZone(tile);
                }
                zones.Add(tile, zone);
                zoneComponent.tiles.Add(tile);
                updateTile(zone, tile);
            });
        }

        // removes tile from its current zone, deletes zone if it was last tile
        public void removeTileFromZone(Vector3Int tile) {
            if (!zones.ContainsKey(tile)) return; // tile is not in the zone
            EcsEntity entity = zones[tile];
            ZoneComponent zone = entity.take<ZoneComponent>();
            ZoneTrackingComponent tracking = entity.take<ZoneTrackingComponent>();

            zones.Remove(tile); // remove tile zone from container
            zone.tiles.Remove(tile); // remove tile from zone
            if (tracking.locked.ContainsKey(tile)) {
                cancelZoneTask(tracking.locked[tile], "tile removed from zone"); // cancel task of this tile
                log("zone locked task canceled " + tile);
            }
            tracking.locked.Remove(tile); // remove tile locking
            tracking.tilesToTask.Remove(tile); // remove tile job
            
            updateTile(entity, tile); // only visual is efficient
            if (zone.tiles.Count == 0) deleteZone(entity);
        }

        // performs logical update and creates event for visual update
        private void updateTile(EcsEntity zone, Vector3Int tile) {
            model.updateUtil.updateTile(tile);
            zone.Get<ZoneVisualUpdateComponent>().add(tile);
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

        private void cancelZoneTask(EcsEntity task, string reason) {
            task.Replace(new TaskFinishedComponent { status = TaskStatusEnum.CANCELED, reason = reason });
            // task.Del<TaskZoneComponent>();
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