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
        public void TelegraphAvailableMove(float opacity, Action<TargetPos> OnClickAction);
        public void ClearAvalableMoves();
        public void TelegraphAttack(List<TargetPos> targets, float opacity, Action<TargetPos> OnClickAction);
        public void ClearAttack();
    }
}