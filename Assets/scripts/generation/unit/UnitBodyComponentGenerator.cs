using System.Collections.Generic;
using enums.unit;
using game.model.component.unit;

namespace generation.unit {
    public class UnitBodyComponentGenerator {

        public BodyComponent generate(CreatureType type) {
            BodyComponent component = new BodyComponent {bodyParts = new List<string>()};
            foreach (var pair in type.bodyParts) {
                component.bodyParts.Add(pair.Key);
            }
            return component;
        }
    }
}