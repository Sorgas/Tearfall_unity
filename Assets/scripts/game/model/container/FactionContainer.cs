using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using TreeEditor;
using types;
using types.action;
using util;
using util.lang;
using util.lang.extension;

namespace game.model.container {
// Stores faction present on localMap; Faction units are combined into groups. Group has behaviour strategy. 
// Player units are stored in this container, but all share same group without strategy.
// UnitTaskAssignmentSystem uses behaviour strategies to create tasks for units. 
public class FactionContainer : LocalModelContainer {
    public readonly MultiValueDictionary<string, EcsEntity> units = new(); // factionName -> units
    public readonly Dictionary<string, Faction> factions = new(); // factionName -> faction
    public readonly MultiValueDictionary<string, UnitGroup> factionToGroups = new(); // factionName -> groups
    public readonly Dictionary<string, UnitGroup> groups = new(); // groupName -> group

    // When unit appears on map it should be added to faction container.
    public void addUnit(EcsEntity unit) {
        ref FactionComponent factionComponent = ref unit.takeRef<FactionComponent>();
        string faction = factionComponent.name;
        string groupName = factionComponent.unitGroup;
        units.add(faction, unit);
        UnitGroup group = getGroup(groupName, faction);
        factionComponent.unitGroup = group.name;
        group.units.Add(unit);
    }

    private UnitGroup getGroup(string groupName, string faction) {
        if (groupName != null) {
            return !groups.ContainsKey(groupName) ? createGroup(groupName, faction) : groups[groupName];
        }
        return factionToGroups.ContainsKey(faction) ? factionToGroups[faction][0] : createGroup(faction + "_main", faction);
    }
    
    private UnitGroup createGroup(string groupName, string factionName) {
        UnitGroup group = new UnitGroup(groupName, factionName);
        factionToGroups.add(factionName, group);
        groups.Add(group.name, group);
        return group;
    }

    public void removeUnit(EcsEntity unit) {
        string faction = unit.take<FactionComponent>().name;
        units.remove(faction, unit);
        factionToGroups[faction][0].units.Remove(unit);
        if (factionToGroups[faction][0].units.Count <= 0) {
            factionToGroups.Remove(faction);
        }
    }

    public FactionContainer(LocalModel model) : base(model) { }
}

// Groups non-player units into group. Group members will receive tasks depending on group behaviour
public class UnitGroup {
    public string name;
    public string faction;
    public List<EcsEntity> units = new();
    public string strategy;
    private float lossRation = 0.5f;

    public UnitGroup(string name, string faction) {
        this.name = name;
        this.faction = faction;
        strategy = selectStrategyByFaction(faction);
    }

    // when something happens with unit, group strategy can change
    public void handleEvent(EcsEntity unit, string eventName) {
        
    }

    private void interruptActions() {
        foreach (var unit in units) {
            if (unit.Has<TaskComponent>()) {
                GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
            }
        }
    }

    private string selectStrategyByFaction(string faction) {
        if (faction.Equals("player")) return null;
        if(GameModel.get().worldModel.factions[faction].strategy.Equals(FactionBehaviourTypes.HOSTILE_AGGRESSIVE)) {
            return "attack";
        }
        throw new GameException($"Unsupported faction {faction}");
    }
}
}