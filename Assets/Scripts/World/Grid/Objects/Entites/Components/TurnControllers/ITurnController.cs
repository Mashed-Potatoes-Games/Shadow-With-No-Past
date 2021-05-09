using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public interface ITurnController
    {
        public event Action TurnPassed;

        TurnPriority Priority { get; set; }

        IEnumerator MoveAndTelegraphAction();
        IEnumerator ExecuteMove();
    }
    public enum TurnPriority
    {
        Player,
        First,
        High,
        Normal,
        Low,
        Last,
    }
}