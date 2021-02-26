using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GridObjects {
    public class MeleeEnemy : BaseEntity
    {
        public override TurnPriority Priority => TurnPriority.High;
        public override int MoveDistance => 2;

        public override IEnumerator PrepareAndTelegraphMove()
        {
            PlayerEntity player = FindPlayer();

            Queue<Vector2Int> path = GetPath(player.CurrentPos, false);

            if(path != null)
            {
                yield return MoveWithDelay(path);
                FaceTo(player);
            }
        }

        public override IEnumerator ExecuteMove()
        {
            yield break;
        }

        public PlayerEntity FindPlayer()
        {
            return FindObjectOfType<PlayerEntity>();
        }
    }

}
