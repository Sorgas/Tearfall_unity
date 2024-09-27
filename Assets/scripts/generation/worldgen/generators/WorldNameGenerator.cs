using UnityEngine;

namespace generation.worldgen.generators {
public class WorldNameGenerator : AbstractWorldGenerator {
    private string[] adjectives = {
        "red", "blue", "black", "white", "green", "grey", "pale", "shady", "vast", "dark",
        "even"
    };
    private string[] nouns = {
        "planes", "mountains", "forests", "sands", "moons", "islands", "notches", "winds", "skies",
        "oceans", "seas", "reach"
    };

    protected override void generateInternal() {
        container.worldName = getRandomFromArray(adjectives) + " " + getRandomFromArray(nouns);
    }
    
    private string getRandomFromArray(string[] array) {
        return array[random(0, array.Length)];
    }
}
}