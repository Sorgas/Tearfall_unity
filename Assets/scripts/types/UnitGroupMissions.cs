namespace types {
// all supported missions for units of different factions. This values are used by EventDirector and UnitTaskAssignmentSystem.
public class UnitGroupMissions {
    public const string HOSTILE_AGGRESSIVE = "hostile_aggerssive"; // will attack units of hostile factions
    public const string HOSTILE_DEFENSIVE = "hostile_defensive"; // will 
    public const string NEUTRAL = "neutral"; 
    // public const string WAIT = "wait";
    public const string ANIMAL_CARNIVORE = "animal_carnivore";
    public const string ANIMAL_HERBIVORE = "animal_herbivore";
    // TODO add other behaviours
}
}