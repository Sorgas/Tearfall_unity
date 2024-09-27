using types.unit;
using UnityEngine;
using static types.unit.CreatureAttributesEnum;
using Random = UnityEngine.Random;

namespace generation.unit {
public class SettlerDataGenerator {
    private UnitNameGenerator nameGenerator = new(new System.Random(System.DateTime.Now.Millisecond));

    public UnitData generate(string faction) {
        UnitData result = new();
        result.male = Random.Range(0, 2) == 0;
        result.name = nameGenerator.generateName(result.male);
        result.age = Random.Range(16, 60);
        result.type = "human";
        CreatureType type = CreatureTypeMap.getType("human");
        result.bodyVariant = Random.Range(0, type.appearance.body[result.male ? 1 : 2]);
        result.headVariant = Random.Range(0, type.appearance.head[result.male ? 1 : 2]);
        result.statsData = generateStats(result);
        result.faction = faction;
        // TODO health conditions, skills, stats, pets, relations, deity
        return result;
    }

    private StatsData generateStats(UnitData settler) {
        StatsData data = new();
        int[] mods = getRandomModifiers();
        data.strength = 8 + mods[0];
        data.agility = 8 + mods[1];
        data.endurance = 8 + mods[2];
        data.intelligence = 8 + mods[3];
        data.spirit = 8 + mods[4];
        data.charisma = 8 + mods[5];
        CreatureAttributesEnum primary = CreatureAttributesUtil.getRandom();
        CreatureAttributesEnum secondary = CreatureAttributesUtil.getRandomRelated(primary);
        addModToStat(data, primary, Random.Range(1, 3));
        addModToStat(data, secondary, 1);
        return data;
    }

    private int[] getRandomModifiers() {
        int[] mods = new int[6];
        int totalMod = 5;
        while (totalMod > 4 || totalMod < -4) {
            totalMod = 0;
            for (int i = 0; i < 6; i++) {
                // mods[i] = Mathf.FloorToInt(Random.Range(-1.165f, 2.165f));
                mods[i] = Random.Range(-1, 2);
                totalMod = mods[i];
            }
        }
        return mods;
    }

    private void addModToStat(StatsData data, CreatureAttributesEnum stat, int mod) {
        switch (stat) {
            case STRENGTH:
                data.strength += mod;
                break;
            case AGILITY:
                data.agility += mod;
                break;
            case ENDURANCE:
                data.endurance += mod;
                break;
            case INTELLIGENCE:
                data.intelligence += mod;
                break;
            case SPIRIT:
                data.spirit += mod;
                break;
            case CHARISMA:
                data.charisma += mod;
                break;
        }
    }
}
}