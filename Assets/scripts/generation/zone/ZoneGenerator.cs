using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using types;
using UnityEngine;
using util.geometry.bounds;

namespace generation.zone {
    public class ZoneGenerator {

        public EcsEntity generate(IntBounds3 bounds, ZoneTypeEnum type, EcsEntity entity, LocalModel model) {
            List<Vector3Int> tiles = new();
            bounds.iterate(position => {
                if (model.localMap.passageMap.getPassage(position) == PassageTypes.PASSABLE.VALUE) {
                    tiles.Add(position);
                }
            });
            entity.Replace(new ZoneComponent { tiles = tiles, type = type });
            if (type == ZoneTypeEnum.STOCKPILE) {
                entity.Replace(new StockpileComponent());
            }
            return entity;
        }
    }
}