namespace game.model {
// Represents faction of the world. 
public class Faction {
    public readonly string name;
    public int relation; // TODO have map of relations to each other faction.
    public string strategy; // default local map behaviour strategy for unis
    
    public Faction(string name) {
        this.name = name;
    }
}
}