using System;
using System.Collections.Generic;
using game.model.component.unit;
using types.unit;

namespace generation.unit {
    public class UnitBodyComponentGenerator {
        private readonly Random random;

        public UnitBodyComponentGenerator(Random random) {
            this.random = random;
        }

        public BodyComponent generate(CreatureType type) {
            BodyComponent component = new BodyComponent {bodyParts = new List<string>()};
            foreach (var pair in type.bodyParts) {
                component.bodyParts.Add(pair.Key);
            }
            return component;
        }
    }
}