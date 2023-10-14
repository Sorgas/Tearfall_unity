using Leopotam.Ecs;

namespace game.view.ui.tooltip {
public class InfoTooltipData {
    // for entity tooltips
    public readonly string type;
    public readonly EcsEntity entity;
    public readonly string entityType;

    // for text tooltip
    public readonly string icon;
    public readonly string title;
    public readonly string text;
    
    public InfoTooltipData(EcsEntity entity, string entityType) {
        type = "entity";
        this.entity = entity;
        this.entityType = entityType;
    }

    public InfoTooltipData(string type) {
        this.type = type;
    }

    public InfoTooltipData(string icon, string title, string text) {
        this.icon = icon;
        this.title = title;
        this.text = text;
    }
}
}