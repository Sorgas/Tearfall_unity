using game.model.container;
using types;

namespace generation.worldgen.generators {
// Generates factions present in the world. Factions have relations to player.
public class WorldFactionGenerator : AbstractWorldGenerator {
    
    protected override void generateInternal() {
        container.factions.Add("player", new FactionDescriptor("player", 0, ""));
        container.factions.Add("raider", new FactionDescriptor("raider", -100, FactionBehaviourTypes.HOSTILE_AGGRESSIVE));
        // TODO add other random factions
    }
}
}