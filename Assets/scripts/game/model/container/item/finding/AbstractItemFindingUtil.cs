using game.model.localmap;
using Leopotam.Ecs;
using UnityEngine;

namespace game.model.container.item.finding {
public class AbstractItemFindingUtil : ItemContainerPart {
    public AbstractItemFindingUtil(LocalModel model, ItemContainer container) : base(model, container) {
        
    }
    
    protected float fastDistance(EcsEntity item, Vector3Int position) => (container.getItemAccessPosition(item) - position).sqrMagnitude;
}
}