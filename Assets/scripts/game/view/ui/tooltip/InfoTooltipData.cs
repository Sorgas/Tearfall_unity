using Leopotam.Ecs;

namespace game.view.ui.tooltip {
public class InfoTooltipData {
    public readonly string type;
    public readonly EcsEntity entity;
    public readonly string entityType;

    public InfoTooltipData(EcsEntity entity, string entityType) {
        type = "entity";
        this.entity = entity;
        this.entityType = entityType;
    }
}
}