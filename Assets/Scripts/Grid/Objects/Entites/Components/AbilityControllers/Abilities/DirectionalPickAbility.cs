using System;
using System.Collections.Generic;

using UnityEngine;

using ShadowWithNoPast.Utils;

namespace ShadowWithNoPast.Entities.Abilities
{

    [CreateAssetMenu(fileName = "CustomAbility", menuName = "Abilities/Directional Pick Ability", order = 1)]
    class DirectionalPickAbility : Ability
    {
        public override AbilityTargetType Type => AbilityTargetType.Directional;


        public override AbilityTargets AvailableTargets(WorldPos executionPos)
        {
            var targets = new List<WorldPos>();
            foreach (Direction dir in CoordinateUtils.AllDirections())
            {
                var vectorDir = CoordinateUtils.GetVectorFromDirection(dir);
                for (int i = 1; i <= DistanceConstraint; i++)
                {
                    targets.Add(new WorldPos(executionPos.World, executionPos.Vector + vectorDir * i));
                }
            }
            return new AbilityTargets(Type, targets);
        }

        public override AbilityTargets AvailableAttackPoints(WorldPos target)
        {
            throw new NotImplementedException();
        }
    }
}
