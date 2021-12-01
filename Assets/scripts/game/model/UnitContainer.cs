using System;
using System.Collections.Generic;
using Leopotam.Ecs;

namespace game.model {
    public class UnitContainer {
        public readonly Dictionary<int, EcsEntity> playerUnits = new Dictionary<int, EcsEntity>();
        public readonly List<EcsEntity> units = new List<EcsEntity>();
        private readonly HashSet<int> usedIds = new HashSet<int>();

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