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
        bool IsActiveTurn { get; set; }

        bool EngageCombat();
        IEnumerator MoveAndTelegraphAction();
        IEnumerator ExecuteMove();

        bool ReadyToExecute();
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