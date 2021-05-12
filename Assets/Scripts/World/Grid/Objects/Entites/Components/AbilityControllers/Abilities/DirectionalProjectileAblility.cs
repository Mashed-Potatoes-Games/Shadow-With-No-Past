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

        public TelegraphElement ProjectileTracer;


        public override AbilityTargets AvailableTargets(WorldPos executionPos)
        {
            return ReturnTargets(executionPos);
        }

        public override AbilityTargets AvailableAttackPoints(WorldPos target)
        {
            return ReturnTargets(target, true);
        }

        public override IEnumerator Execute(AbilityArgs args)
        {
            args.Target = TargetToExecPos(args.Caller, args.Target);
            yield return base.Execute(args);
        }

        public override List<SingleTelegraphData> GetHelpingTelegraphs(GridEntity caller, WorldPos target)
        {
            Direction? dir = CoordinateUtils.GetDirectionFromVector(target.Vector - caller.Vector); if (dir == null)
            {
                Debug.LogError("Directional ability was called with non-directional target. There must be a mistake in which target was provided or picked!");
                return null;
            }
            List<SingleTelegraphData> helpTelegraphs = new List<SingleTelegraphData>();
            Vector2Int step = CoordinateUtils.GetVectorFromDirection(dir.Value);
            WorldPos pos = caller.Pos + step;
            while(pos != target)
            {
                helpTelegraphs.Add(new SingleTelegraphData(ProjectileTracer, pos, 0, dir));
                pos += step;
            }
            return helpTelegraphs;
        }

        public override WorldPos TargetToExecPos(GridEntity caller, WorldPos target)
        {
            Direction? dir = CoordinateUtils.GetDirectionFromVector(target.Vector - caller.Vector);
            if (dir == null)
            {
                Debug.LogError("Directional ability was called with non-directional target. There must be a mistake in which target was provided or picked!");
                return target;
            }
            Vector2Int dirVector = CoordinateUtils.GetVectorFromDirection(dir.Value);
            target = caller.Pos;
            for (int i = 1; i <= DistanceConstraint; i++)
            {
                target += dirVector;
                if (target.GetStatus() != CellStatus.Free)
                {
                    break;
                }
            }
            return target;
        }

        private AbilityTargets ReturnTargets(WorldPos executionPos, bool includeInbetween = false)
        {
            List<WorldPos> targets = new List<WorldPos>();
            foreach (Direction dir in CoordinateUtils.AllDirections())
            {
                int distanceLeft = DistanceConstraint - 1;
                Vector2Int dirVector = CoordinateUtils.GetVectorFromDirection(dir);
                var target = executionPos + dirVector;
                while (distanceLeft > 0 && target.GetStatus() == CellStatus.Free)
                {
                    if(includeInbetween)
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
        public override List<WorldPos> ChangesTargetOnMovement(WorldPos executionPos, WorldPos initialTarget)
        {
            List<WorldPos> targets = new List<WorldPos>();

            var dirVector = initialTarget.Vector - executionPos.Vector;
            dirVector.Clamp(-Vector2Int.one, Vector2Int.one);
            if(dirVector.magnitude != 1)
            {
                Debug.LogError("Target was not in line with the caller!");
            }

            int distanceLeft = DistanceConstraint - 1;
            var target = executionPos + dirVector;
            while (distanceLeft > 0 && target.GetStatus() == CellStatus.Free)
            {
                targets.Add(target);
                target += dirVector;
                distanceLeft--;
            }
            targets.Add(target);

            return targets;
        }
    }
}