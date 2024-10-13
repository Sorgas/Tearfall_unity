using System.Collections.Generic;
using game.model.component;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using util.lang.extension;

namespace game.model.container {
// Stores factions present on localMap; Faction units are combined into groups. Groups can have different missions. 
// Player units are stored in this container, but all share same group without mission.
// UnitTaskAssignmentSystem uses group missions to create tasks for units. 
public class FactionContainer : LocalModelContainer {
    public readonly Dictionary<string, LocalMapFaction> factions = new(); // factionName -> faction
    
    public FactionContainer(LocalModel model) : base(model) { }

    // When unit appears on map it should be added to faction container.
    public void addUnit(EcsEntity unit) {
        ref FactionComponent factionComponent = ref unit.takeRef<FactionComponent>();
        string faction = factionComponent.name;
        string groupName = factionComponent.unitGroup;
        string mission = factionComponent.mission;
        if (!factions.ContainsKey(faction)) factions.Add(faction, new LocalMapFaction(faction));
        factions[faction].addUnit(unit);
    }

    public void removeUnit(EcsEntity unit) {
        string faction = unit.take<FactionComponent>().name;
        factions[faction].removeUnit(unit);
    }

    public List<EcsEntity> getUnitsOfHostileFactions(EcsEntity unit) {
        List<EcsEntity> result = new();
        ref FactionComponent factionComponent = ref unit.takeRef<FactionComponent>();
        foreach (var pair in GameModel.get().worldModel.factionRelations[factionComponent.name]) {
            if (pair.Value == -100 && factions.ContainsKey(pair.Key)) {
                foreach (var group in factions[pair.Key].groups.Values) {
                    result.AddRange(group.units);
                }
            }
        }
        return result;
    }
}

// All units on local map belong to some faction
public class LocalMapFaction {
    public readonly string factionName;
    public readonly Dictionary<string, UnitGroup> groups = new();
    private const string defaultGroupName = "main";
    private Faction faction;
    
    public LocalMapFaction(string factionName) {
        this.factionName = factionName;
        faction = GameModel.get().worldModel.factions[factionName];
    }

    // adds unit to faction. Can create new group with default mission for unit. If unit has mission, its mission will be set to group. 
    public void addUnit(EcsEntity unit) {
        ref FactionComponent factionComponent = ref unit.takeRef<FactionComponent>();
        string groupName = factionComponent.unitGroup ?? defaultGroupName;
        if(!groups.ContainsKey(groupName)) groups.Add(groupName, new UnitGroup(groupName, faction));
        groups[groupName].units.Add(unit);
        if (factionComponent.mission != null) {
            groups[groupName].mission = factionComponent.mission;
        } else {
            factionComponent.mission = groups[groupName].mission;
        }
        factionComponent.unitGroup = groupName;
    }

    // Removes unit from faction. Should be called on unit's death and before leaving map.
    public void removeUnit(EcsEntity unit) {
        ref FactionComponent factionComponent = ref unit.takeRef<FactionComponent>();
        UnitGroup group = groups[factionComponent.unitGroup];
        group.units.Remove(unit);
        if(group.units.Count == 0) groups.Remove(factionComponent.unitGroup);
    }
}

// Groups non-player units into group. Group members will receive tasks depending on group behaviour
public class UnitGroup {
    public string name;
    public Faction faction;
    public string mission;
    public List<EcsEntity> units = new();
    // private float lossRatio = 0.5f;

    public UnitGroup(string name, Faction faction, string mission) {
        this.name = name;
        this.faction = faction;
        this.mission = mission;
    }
    
    public UnitGroup(string name, Faction faction) : this (name, faction, faction.defaultMission) { }

    // when something happens with unit, group strategy can change
    public void handleEvent(EcsEntity unit, string eventName) {
        // TODO when losses sustained, change strategy to retreat
    }

    private void interruptActions() {
        foreach (var unit in units) {
            if (unit.Has<TaskComponent>()) {
                GameModel.get().currentLocalModel.taskContainer.removeTask(unit.take<TaskComponent>().task, TaskStatusEnum.FAILED);
            }
        }
    }
}
}