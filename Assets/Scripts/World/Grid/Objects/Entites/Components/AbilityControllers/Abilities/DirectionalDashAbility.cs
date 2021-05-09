using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "CustomAbility", menuName = "Abilities/Directional Dash Ability", order = 1)]
    public class DirectionalDashAbility : Ability
    {
        public override AbilityTargetType Type => AbilityTargetType.Directional;

        public override AbilityTargets AvailableAttackPoints(WorldPos target)
        {
            throw new System.NotImplementedException();
        }

        public override AbilityTargets AvailableTargets(WorldPos executionPos)
        {
            throw new System.NotImplementedException();
        }
    }
}