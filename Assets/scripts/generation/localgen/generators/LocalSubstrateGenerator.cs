using game.model.localmap;
using generation.localgen;

public class LocalSubstrateGenerator : LocalGenerator {
    public LocalSubstrateGenerator(LocalMapGenerator generator) : base(generator) {
    }

    public override void generate() {
        LocalMap map = container.map;
        int mapHeight = map.bounds.maxZ;
        map.bounds.iterate(position => {
            if(position.z != mapHeight) {
                int blockType = map.blockType.get(position);
                // if(blockType != )
            }
        });
        // put substrate to tiles
    }

    public override string getMessage() {
        return "generating grass ...";
    }
}