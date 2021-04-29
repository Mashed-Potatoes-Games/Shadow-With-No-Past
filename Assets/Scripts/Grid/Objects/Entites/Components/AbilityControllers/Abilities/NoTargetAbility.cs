using ShadowWithNoPast.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities.Abilities
{
    [CreateAssetMenu(fileName = "CustomAbility", menuName = "Abilities/No Target Ability", order = 1)]
    public class NoTargetAbility : Ability
    {
        public override AbilityTargetType Type => AbilityTargetType.OnSelf;
        public override AbilityTargets AvailableTargets(WorldPos executionPos)
        {
            return new AbilityTargets(Type, null);
        }

        public override AbilityTargets AvailableAttackPoints(WorldPos target)
        {
            return AvailableTargets(target);
        }
    }
}