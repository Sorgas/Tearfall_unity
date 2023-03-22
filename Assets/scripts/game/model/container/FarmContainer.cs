using System.Collections.Generic;
using game.model.localmap;
using UnityEngine;

namespace game.model.container {
    // Stores positions of hoed tiles
    // On changes, adds position for visual update, see TileUpdateVisualSystem
    public class FarmContainer : LocalModelUpdateComponent {
        private readonly Dictionary<Vector3Int, int> farms = new();

        public FarmContainer(LocalModel model) : base(model) { }

        public void addFarm(Vector3Int position) {
            farms.Add(position, 100);
            addPositionForUpdate(position);
        }

        public void removeFarm(Vector3Int position) {
            if (farms.ContainsKey(position)) {
                farms.Remove(position);
                addPositionForUpdate(position);
            }
        }

        public bool isFarm(Vector3Int position) {
            return farms.ContainsKey(position);
        }
    }
}