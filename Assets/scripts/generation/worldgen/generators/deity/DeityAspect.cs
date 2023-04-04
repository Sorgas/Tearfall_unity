namespace generation.worldgen.generators.deity {
    //  
    public abstract class DeityAspect {
        public readonly string name;
        public readonly DeityBonus[] bonuses;
    }

    // 
    public class DeityBonus {
        public string name;
        public string description;
        public int devotionLevel;
    }
}