using System.Collections.Generic;

namespace types.unit {
// Lists all jobs for units in game. Each task has job. Task can be assigned only to settlers with enabled job.
// Some jobs use use skill of a unit (UnitSkills). 
public static class Jobs {
    public static readonly Job NONE = new("none", null, "none");
    // resources
    public static readonly Job MINER = new("miner", "mining", "miner");
    public static readonly Job WOODCUTTER = new("woodcutter", "woodcutting", "woodcutter");
    public static readonly Job FARMER = new("farmer", "farming", "farmer");
    public static readonly Job FORAGER = new("forager", "foraging", "herbalist");
    public static readonly Job FISHER = new("fisher", "fishing", "fisher");
    public static readonly Job HUNTER = new("hunter", "hunting", "hunter");
    // crafting
    public static readonly Job CARPENTER = new("carpenter", "carpentry", "carpenter");
    public static readonly Job MASON = new("mason", "masonry", "mason");
    public static readonly Job SMITH = new("smith", "smithing", "smith");
    public static readonly Job MECHANIC = new("mechanic", "mechanics", "mechanic");
    public static readonly Job JEWELER = new("jeweler", "jewelry", "jeweler");
    public static readonly Job COOK = new("cook", "cooking", "cook");
    public static readonly Job BREWER = new("brewer", "brewing", "brewer");
    public static readonly Job TAILOR = new("tailor", "tailoring", "tailor");
    public static readonly Job LEATHERWORKER = new("leatherworker", "leatherworking", "leatherworker");
    public static readonly Job ALCHEMIST = new("alchemist", "alchemy", "alchemist");
    // other
    public static readonly Job TRADER = new("trader", "trading", "trader");
    public static readonly Job DOCTOR = new("doctor", "medicine", "doctor");
    public static readonly Job SCHOLAR = new("scholar", "scholarship", "scholar");
    public static readonly Job PERFORMER = new("performer", "performance", "performer");
    public static readonly Job BUILDER = new("builder", "builder");
    public static readonly Job HAULER = new("hauler", "hauler");

    // public static readonly Job ANIMAL_CARETAKER = new("animal caretaker", "animal handling", "animalCaretaker");
    // public static readonly Job BOOKMAKER = new("bookmaker", "bookmaking", "bookmaker");

    public static readonly Job[] jobs = {
        MINER, MASON, WOODCUTTER, CARPENTER, SMITH, JEWELER, MECHANIC, FARMER, FORAGER, FISHER,
        HUNTER, COOK, BREWER, TAILOR, LEATHERWORKER, TRADER, ALCHEMIST, DOCTOR, SCHOLAR, PERFORMER, BUILDER, HAULER
    };
    public static readonly Job[] all = {
        NONE, MINER, MASON, WOODCUTTER, CARPENTER, SMITH, JEWELER, MECHANIC, FARMER, FORAGER, FISHER,
        HUNTER, COOK, BREWER, TAILOR, LEATHERWORKER, TRADER, ALCHEMIST, DOCTOR, SCHOLAR, PERFORMER, BUILDER, HAULER
    };
    public static readonly Dictionary<string, Job> jobMap = new();
    public static readonly Dictionary<string, Job> jobsBySkill = new();

    static Jobs() {
        foreach (var job in all) {
            jobMap.Add(job.name, job);
            if (job.skill != null) {
                jobsBySkill.Add(job.skill, job);
            }
        }
    }

    public static Job getByName(string name) {
        foreach (var job1 in all) {
            if (job1.name == name) return job1;
        }
        return NONE;
    }
}

public class Job {
    public readonly string name;
    public readonly string skill;
    public readonly string iconName;

    public Job(string name, string iconName) : this(name, null, iconName) { }

    public Job(string name, string skill, string iconName) {
        this.name = name;
        this.skill = skill;
        this.iconName = iconName;
    }
}
}