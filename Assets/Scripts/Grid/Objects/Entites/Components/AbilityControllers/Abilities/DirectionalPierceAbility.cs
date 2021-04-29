using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "CustomAbility", menuName = "Abilities/Directional Pierce Ability", order = 1)]
    public class DirectionalPierceAbility : Ability
    {
        public override AbilityTargetType Type => AbilityTargetType.Directional;

        public override AbilityTargets AvailableTargets(WorldPos executionPos)
        {
            throw new NotImplementedException();
        }

        public override AbilityTargets AvailableAttackPoints(WorldPos target)
        {
            throw new NotImplementedException();
        }
    }
}