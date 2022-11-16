using game.model.component.unit;
using Leopotam.Ecs;
using TMPro;
using util.lang.extension;

public class UnitMenuHealthInfoHandler : UnitMenuTab {
    public TextMeshProUGUI statusText;
    public ProgressBarHandler sleepNeed;
    public ProgressBarHandler hunger;
    public ProgressBarHandler thirst;
    
    // TODO lists for injures

    public override void initFor(EcsEntity unit) {
        HealthComponent component = unit.take<HealthComponent>();
        UnitNeedComponent needs = unit.take<UnitNeedComponent>();
        statusText.text = component.overallStatus;
        sleepNeed.setValue(needs.rest);
        hunger.setValue(needs.hunger);
        thirst.setValue(needs.thirst);
    }
}