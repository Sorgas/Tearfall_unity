using game.model.component;
using Newtonsoft.Json;
using UnityEngine;

namespace generation.unit {
class UnitNameGenerator {
    private NamesDescriptor names;

    public UnitNameGenerator() {
        TextAsset file = Resources.Load<TextAsset>("data/creatures/body_templates");
        names = JsonConvert.DeserializeObject<NamesDescriptor>(file.text);
    }

    public NameComponent generate(bool male) {
        return new NameComponent { name = generateName(male) };
    }

    public NameComponent generate(SettlerData data) {
        return new NameComponent { name = data.name };
    }

    public string generateName(bool male) {
        return getRandomFromArray(male ? names.male : names.female) + getRandomFromArray(names.second);
    }
    
    private string getRandomFromArray(string[] array) {
        return array[Random.Range(0, array.Length)];
    }
}

public class NamesDescriptor {
    public string[] male;
    public string[] female;
    public string[] second;
}
}