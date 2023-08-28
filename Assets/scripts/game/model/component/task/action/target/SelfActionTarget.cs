using System.Collections.Generic;
using game.model.localmap;
using Leopotam.Ecs;
using types.action;
using UnityEngine;

namespace game.model.component.task.action.target {
/**
    * Target for actions with no particular target.
    *
    * @author Alexander on 09.02.2020
    */
public class SelfActionTarget : ActionTarget {
    public SelfActionTarget() : base(ActionTargetTypeEnum.EXACT) { }

    public override Vector3Int pos => Vector3Int.back;
    public override List<Vector3Int> positions { get; }

    public override void init() { }

    public override ActionTargetStatusEnum check(EcsEntity performer, LocalModel model) {
        return ActionTargetStatusEnum.READY;
    }

    public override List<Vector3Int> getAcceptablePositions(LocalModel model) {
        throw new System.NotImplementedException();
    }
}
}