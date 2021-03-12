using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ShadowWithNoPast.Entities
{
    public interface ITelegraphController
    {
        public event Action<Vector2Int> OnClick;
        public void TelegraphAvailableMove(float opacity);
        public void ClearAvalableMoves();
        public void TelegraphAttack(Vector2Int target, float opacity);
        public void ClearAttack();
    }
}