﻿using System;
using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;

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
            if (type == ZoneTypeEnum.STOCKPILE) {
                entity.Replace(new StockpileComponent());
            }
            return entity;
        }

        public string generateName(ZoneTypeEnum type, int number) {
            switch (type) {
                case ZoneTypeEnum.STOCKPILE:
                    return "Stockpile #" + number;
                case ZoneTypeEnum.FARM:
                    return "Farm #" + number;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}