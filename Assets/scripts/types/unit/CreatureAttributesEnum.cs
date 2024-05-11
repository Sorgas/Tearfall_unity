using System;
using static types.unit.CreatureAttributesEnum;
using Random = UnityEngine.Random;

namespace types.unit {
public enum CreatureAttributesEnum {
    STRENGTH,
    AGILITY,
    ENDURANCE,
    INTELLIGENCE,
    SPIRIT,
    CHARISMA
}

public static class CreatureAttributesUtil {
    private static readonly CreatureAttributesEnum[] array = { STRENGTH, AGILITY, ENDURANCE, INTELLIGENCE, SPIRIT, CHARISMA };

    public static CreatureAttributesEnum getRandom() {
        return array[Random.Range(0, array.Length)];
    }

    // attributes relate to each other
    public static CreatureAttributesEnum getRandomRelated(CreatureAttributesEnum source) {
        return source switch {
            STRENGTH => selectRandomFromArray(SPIRIT, ENDURANCE),
            AGILITY => selectRandomFromArray(CHARISMA, ENDURANCE),
            ENDURANCE => selectRandomFromArray(STRENGTH, AGILITY),
            INTELLIGENCE => selectRandomFromArray(SPIRIT, CHARISMA),
            SPIRIT => selectRandomFromArray(STRENGTH, INTELLIGENCE),
            CHARISMA => selectRandomFromArray(INTELLIGENCE, AGILITY),
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, null)
        };
    }

    private static CreatureAttributesEnum selectRandomFromArray(params CreatureAttributesEnum[] args) {
        return args[Random.Range(0, args.Length)];
    }
}
}