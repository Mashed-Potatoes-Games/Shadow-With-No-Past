using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShadowWithNoPast.Entities
{
    public interface ITelegraphController
    {
        public void TelegraphAvailableMove(PointerActions actions = null);
        public void ClearAvalableMoves();
        public void TelegraphAbility(TargetPos target, AbilityInstance abilityInstance, bool showAttackValue, PointerActions actions = null);
        public void ClearAbility();
        public void TelegraphAvailableAttacks(List<TargetPos> targets, float opacity, PointerActions actions = null);
        public void ClearAvailableAttacks();
    }
}