using game.model.component;

namespace generation.unit {
    class UnitNameGenerator {

        public NameComponent generate(SettlerData data) {
            return new NameComponent {name = data.name};
        }
    }
}