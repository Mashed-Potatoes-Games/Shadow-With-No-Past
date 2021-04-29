using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ShadowWithNoPast.Utils;
using System;

namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "CustomAbility", menuName = "Abilities/Directional Projectile|Attack Ability", order = 1)]
    public class DirectionalProjectileAblility : Ability
    {
        public override AbilityTargetType Type => AbilityTargetType.Directional;

        public override AbilityTargets AvailableTargets(WorldPos executionPos)
        {
            return ReturnTargets(executionPos);
        }


        public override AbilityTargets AvailableAttackPoints(WorldPos target)
        {
            return ReturnTargets(target, true);
        }
        private AbilityTargets ReturnTargets(WorldPos executionPos, bool IncludeInbetween = false)
        {
            List<WorldPos> targets = new List<WorldPos>();
            foreach (Direction dir in CoordinateUtils.AllDirections())
            {
                int distanceLeft = DistanceConstraint - 1;
                Vector2Int dirVector = CoordinateUtils.GetVectorFromDirection(dir);
                var target = executionPos + dirVector;
                while (distanceLeft > 0 && target.GetStatus() == CellStatus.Free)
                {
                    if(IncludeInbetween)
                    {
                        targets.Add(target);
                    }
                    target += dirVector;
                    distanceLeft--;
                }
                targets.Add(target);
            }

            var abilityTarget = new AbilityTargets(Type, targets);
            return abilityTarget;
        }
    }
}