using game.model;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;

namespace generation.plant {
    public class PlantGenerator {

        public EcsEntity generate(string typeName, Vector3Int position) {
            EcsEntity entity = GameModel.get().createEntity();
            entity.Replace<PlantComponent>(new PlantComponent { });
            return entity;
        }
    }
}