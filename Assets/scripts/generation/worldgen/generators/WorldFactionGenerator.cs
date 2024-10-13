using game.model;
using types;

namespace generation.worldgen.generators {
// Generates factions present in the world. Factions have relations to player.
public class WorldFactionGenerator : AbstractWorldGenerator {
    private string[] names1 = { "alliance", "order", "community", "state", "society"};
    private string[] names2 = { "beauty", "chaos", "keepers", "trust"};
    
    protected override void generateInternal() {
        addFaction(new Faction("player", 0, ""));
        addFaction(new Faction("raider", -100, UnitGroupMissions.HOSTILE_AGGRESSIVE));
        addFaction(new Faction("carnivore", 0, UnitGroupMissions.ANIMAL_CARNIVORE));
        addFaction(new Faction("herbivore", 0, UnitGroupMissions.ANIMAL_HERBIVORE));
        addFaction(generateFaction(-100));
        addFaction(generateFaction(0));
    }

    private void addFaction(Faction faction) {
        container.factions.Add(faction.name, faction);
    }

    private Faction generateFaction(int relationToPlayer) {
        string name = selectRandomString(names1) + " of " + selectRandomString(names2);
        string mission = relationToPlayer < 0 ? UnitGroupMissions.HOSTILE_DEFENSIVE : UnitGroupMissions.NEUTRAL;
        return new Faction(name, relationToPlayer, mission);
    }

    private string selectRandomString(string[] array) {
        return array[numberGenerator.Next(array.Length)];
    }
}
}