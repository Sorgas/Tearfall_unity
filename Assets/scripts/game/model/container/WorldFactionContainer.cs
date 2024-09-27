using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Reporting.Builders;

namespace game.model.container {
// Stores factions present on world map.
public class WorldFactionContainer {
    
    public Dictionary<string, FactionDescriptor> factions;
    
}

public class FactionDescriptor {
    public string name;
    public int relation; // diplomacy relations
    public string behaviour; // how members behave on local map

    public FactionDescriptor(string name, int relation, string behaviour) {
        this.name = name;
        this.relation = relation;
        this.behaviour = behaviour;
    }
}
}