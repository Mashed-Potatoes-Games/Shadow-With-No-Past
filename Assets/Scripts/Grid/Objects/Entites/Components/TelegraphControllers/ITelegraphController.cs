using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShadowWithNoPast.Entities
{
    public interface ITelegraphController
    {
        public void TelegraphAvailableMove(float opacity, Action<Vector2Int> OnClickAction);
        public void ClearAvalableMoves();
        public void TelegraphAttack(Vector2Int target, float opacity);
        public void ClearAttack();
    }
}