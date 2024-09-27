using game.model.component;
using Newtonsoft.Json;
using UnityEngine;

namespace generation.unit {
class UnitNameGenerator : AbstractNameGenerator {
    private readonly System.Random random;
    private NamesDescriptor namesDescriptor_;
    private NamesDescriptor namesDescriptor {
        get {
            if (namesDescriptor_== null) {
                TextAsset file = Resources.Load<TextAsset>("data/nameGenerators/units");
                namesDescriptor_= JsonConvert.DeserializeObject<NamesDescriptor>(file.text);
            }
            return namesDescriptor_;
        }
    }

    public UnitNameGenerator(System.Random random) {
        this.random = random;
    }

    public NameComponent generate(bool male) {
        return new NameComponent { name = generateName(male) };
    }

    public NameComponent generate(UnitData data) {
        return new NameComponent { name = data.name };
    }

    public string generateName(bool male) {
        return getRandomFromArray(male ? namesDescriptor.male : namesDescriptor.female) + " " + getRandomFromArray(namesDescriptor.second);
    }

    private string getRandomFromArray(string[] array) {
        return array[random.Next(0, array.Length)];
    }
}

public class NamesDescriptor {
    public string[] male;
    public string[] female;
    public string[] second;
}
}