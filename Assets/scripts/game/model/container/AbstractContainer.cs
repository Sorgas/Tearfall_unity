using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;
using util.lang;
using util.lang.extension;

namespace game.model.container {
    // model-aware component
    public abstract class LocalModelContainer {
        protected readonly LocalModel model;
        protected readonly ToggleableLogger logger = new ("LocalModelContainer");
        
        protected LocalModelContainer(LocalModel model) => this.model = model;
    }

    // for containers which can update positions
    public abstract class LocalModelUpdateContainer : LocalModelContainer {
        protected EcsEntity updateEntity;

        protected LocalModelUpdateContainer(LocalModel model) : base(model) {
            updateEntity = model.createEntity();
            updateEntity.Replace(new TileUpdateComponent { tiles = new() });
            updateEntity.Replace(new TileVisualUpdateComponent { tiles = new() });
        }

        public void addPositionForUpdate(Vector3Int position) {
            model.updateUtil.updateTile(position);
            // updateEntity.take<TileUpdateComponent>().tiles.Add(position);
            updateEntity.take<TileVisualUpdateComponent>().tiles.Add(position);
        }
    }
}