using game.model.component.unit;
using Leopotam.Ecs;
using TMPro;
using util.lang.extension;

namespace game.view.ui.unit_menu {
public class UnitSkillsInfoHandler : UnitMenuTab {
    public TextMeshProUGUI strengthAttributeText;
    public TextMeshProUGUI agilityAttributeText;
    public TextMeshProUGUI enduranceAttributeText;
    public TextMeshProUGUI intelligenceAttributeText;
    public TextMeshProUGUI willAttributeText;
    public TextMeshProUGUI charismaAttributeText;

    public override void showUnit(EcsEntity unit) {
        UnitAttributesComponent attributes = unit.take<UnitAttributesComponent>();
        strengthAttributeText.text = attributes.strength.ToString();
        agilityAttributeText.text = attributes.agility.ToString();
        enduranceAttributeText.text = attributes.endurance.ToString();
        intelligenceAttributeText.text = attributes.intelligence.ToString();
        willAttributeText.text = attributes.will.ToString();
        charismaAttributeText.text = attributes.charisma.ToString();
    }

    protected override void updateView() { }
}
}