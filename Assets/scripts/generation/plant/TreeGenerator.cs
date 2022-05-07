using game.model;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.plant {
    public class TreeGenerator {

        public void generateTree(Vector3Int position) {
            EcsEntity entity = GameModel.get().createEntity();
            entity.Replace<PlantComponent>(new PlantComponent());
            entity.Replace<TreeComponent>(new TreeComponent());
        }
    }
}