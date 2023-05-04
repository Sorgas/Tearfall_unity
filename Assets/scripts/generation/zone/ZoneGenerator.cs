using System;
using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using game.model.util.validation;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;
using static types.ZoneTaskTypes;
using ArgumentException = System.ArgumentException;

namespace generation.zone {
    public class ZoneGenerator {

        public EcsEntity generate(IntBounds3 bounds, ZoneTypeEnum type, int number, LocalModel model) {
            List<Vector3Int> validTiles = getValidTiles(bounds, type, model);
            if (validTiles.Count <= 0) return EcsEntity.Null; // no valid tiles
            EcsEntity entity = model.createEntity();
            entity.Replace(new ZoneComponent { tiles = validTiles, type = type, number = number });
            entity.Replace(new NameComponent { name = generateName(type, number) });
            entity.Replace(new ZoneTasksComponent { priority = 5 });
            entity.Replace(createTrackingComponent(type));
            if (type == ZoneTypeEnum.STOCKPILE) entity.Replace(new StockpileComponent { map = new() });
            if (type == ZoneTypeEnum.FARM) entity.Replace(new FarmComponent());
            return entity;
        }

        public string generateName(ZoneTypeEnum type, int number) {
            return type switch {
                ZoneTypeEnum.STOCKPILE => "Stockpile #" + number,
                ZoneTypeEnum.FARM => "Farm #" + number,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }

        private ZoneTrackingComponent createTrackingComponent(ZoneTypeEnum type) {
            ZoneTrackingComponent tracking = new() {
                locked = new(), tilesToTask = new(), totalTasks = new()
            };
            string[] taskTypes = type switch {
                ZoneTypeEnum.STOCKPILE => STOCKPILE_TASKS,
                ZoneTypeEnum.FARM => FARM_TASKS,
                _ => throw new ArgumentException("Zone type unhandled in ZoneGenerator")
            };
            return tracking;
        }

        private List<Vector3Int> getValidTiles(IntBounds3 bounds, ZoneTypeEnum type, LocalModel model) {
            PositionValidator validator = ZoneTypes.get(type).positionValidator;
            List<Vector3Int> validTiles = new();
            bounds.iterate(position => {
                if (validator.validate(position, model)) {
                    validTiles.Add(position);
                }
            });
            return validTiles;
        }
    }
}