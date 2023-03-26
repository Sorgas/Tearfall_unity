using System;
using System.Collections.Generic;
using System.Diagnostics;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;
using static types.ZoneTaskTypes;

namespace generation.zone {
    public class ZoneGenerator {
        public EcsEntity generate(IntBounds3 bounds, ZoneTypeEnum type, int number, EcsEntity entity, LocalModel model) {
            List<Vector3Int> tiles = new();
            bounds.iterate(position => {
                if (model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE) {
                    tiles.Add(position);
                }
            });
            entity.Replace(new ZoneComponent { tiles = tiles, type = type, number = number });
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
            ZoneTrackingComponent component = new() {locked = new(), tasks = new(), tiles = new()};
            string[] taskTypes = type switch {
                ZoneTypeEnum.STOCKPILE => STOCKPILE_TASKS,
                ZoneTypeEnum.FARM => FARM_TASKS,
                _ => throw new ArgumentException("Zone type unhandled in ZoneGenerator")
            };
            foreach (string taskType in taskTypes) {
                component.tasks.Add(taskType, new());
                component.tiles.Add(taskType, new());
            }
            return component;
        }
    }
}