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
        public void TelegraphAbility(WorldPos target, AbilityInstance abilityInstance, bool showAttackValue, PointerActions actions = null);
        public void ClearAbility();
        public void TelegraphAvailableAttacks(AbilityTargets targets, float opacity, PointerActions actions = null);
        public void ClearAvailableAttacks();
        void ClearAll();
        void RepaintAbility(AbilityInstance instance);
    }
}