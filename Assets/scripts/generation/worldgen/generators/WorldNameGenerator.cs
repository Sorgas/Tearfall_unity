using UnityEngine;

namespace generation.worldgen.generators {
public class WorldNameGenerator : WorldGenerator {
    private string[] adjectives = {
        "red", "blue", "black", "white", "green", "grey", "pale", "shady", "vast", "dark",
        "even"
    };
    private string[] nouns = {
        "planes", "mountains", "forests", "sands", "moons", "islands", "notches", "winds", "skies",
        "oceans", "seas", "reach"
    };

    public override void generate() {
        container.worldName = getRandomFromArray(adjectives) + " " + getRandomFromArray(nouns);
    }
    
    private string getRandomFromArray(string[] array) {
        return array[Random.Range(0, array.Length)];
    }
}
}