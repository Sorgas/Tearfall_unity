namespace game.model {
// Represents faction of the world. 
public class Faction {
    public readonly string name;
    public readonly string defaultMission; // default local map behaviour strategy for unis
    public readonly int relation; // relation to player TODO implement actual relations between factions

    public Faction(string name, int relations, string defaultMission) {
        this.name = name;
        relation = relations;
        this.defaultMission = defaultMission;
    }
}
}