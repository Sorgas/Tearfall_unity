using System;
using System.Collections.Generic;

namespace game.model {
// contains WorldMap and Ecs infrastructure for world entities
public class WorldModel {
    public string worldName;
    public WorldMap worldMap;
    public Dictionary<string, Faction> factions = new();
    public Dictionary<string, Dictionary<string, int>> factionRelations = new();

    public void update(int ticks) {
        
    }
    
    public void setFactionRelations(string faction1, string faction2, int value) {
        factionRelations[faction1][faction2] = value;
        factionRelations[faction2][faction1] = value;
    }

    public void initRelations() {
        foreach (Faction faction in factions.Values) {
            factionRelations.Add(faction.name, new Dictionary<string, int>());
            foreach (Faction faction2 in factions.Values) {
                if (faction2 == faction) continue;
                factionRelations[faction.name].Add(faction2.name, Math.Min(faction.relation, faction2.relation));
            }
        }
    }
}
}