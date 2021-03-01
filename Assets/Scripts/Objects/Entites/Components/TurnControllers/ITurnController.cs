using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public interface ITurnController
    {
        TurnPriority Priority { get; set; }
        int MoveDistance { get; set; }

        IEnumerator PrepareAndTelegraphMove();
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