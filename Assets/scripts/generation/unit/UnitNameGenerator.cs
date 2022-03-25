using game.model.component.unit;

namespace generation.unit {
    class UnitNameGenerator {

        public string generate(string race, string sex) {
            return "mock name"; 
        }

        public UnitNameComponent generate() {
            return new UnitNameComponent {name = "mockName"};
        }
    }
}