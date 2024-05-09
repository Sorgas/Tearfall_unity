using System;
using System.Collections.Generic;
using game.model.system.unit;
using Leopotam.Ecs;

namespace game.model.container {
    public class UnitContainer {
        public readonly Dictionary<int, EcsEntity> playerUnits = new();
        public readonly List<EcsEntity> units = new();
        private readonly HashSet<int> usedIds = new();
        public readonly UnitStatusEffectUtility statusEffectUtility = new();

        public void addNewPlayerUnit(EcsEntity unit) {
            playerUnits.Add(getFreeId(), unit);
        }

        private int getFreeId() {
            for (int i = 0; i < Int32.MaxValue; i++) {
                if (!usedIds.Contains(i)) return i;
            }
            return -1;
        }
    }
}