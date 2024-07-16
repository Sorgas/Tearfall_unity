namespace generation.localgen.generators {
public class LocalOresGeneration : LocalGenerator {
    public LocalOresGeneration(LocalMapGenerator generator) : base(generator) { }

    public override void generate() {
        // foreach (var layer in container.stoneLayers) {
        //     if (layer.material != "soil") {
        //         
        //     }
        // }
    }

    public override string getMessage() {
        return "generating ores";
    }
}
}