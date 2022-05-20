using game.model.component;

namespace generation.unit {
    class UnitNameGenerator {

        public string generate(string race, string sex) {
            return "mock name"; 
        }

        public NameComponent generate() {
            return new NameComponent {name = "mockName"};
        }
    }
}