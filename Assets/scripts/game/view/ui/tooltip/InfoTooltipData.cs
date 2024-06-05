using Leopotam.Ecs;

namespace game.view.ui.tooltip {
public class InfoTooltipData {
    public readonly string type;
    
    // for entity tooltips
    public readonly EcsEntity entity;
    public readonly string entityType;

    // for text tooltip
    public readonly string icon;
    public readonly string title;
    public readonly string text;

    public readonly object o;
    
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

    public InfoTooltipData(string type, object o) {
        this.type = type;
        this.o = o;
    }
}
}