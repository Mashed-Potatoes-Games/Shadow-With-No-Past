using ShadowWithNoPast.Entities.Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    public class PlayerMoveListenState : PlayerAbilityPickListenState
    {
        public PlayerMoveListenState(GridEntity player, InputTurnsController stateMachine) : base(player, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            stateMachine.MadeMove = false;


            stateMachine.Telegraph.TelegraphAvailableMove(0.5f, OnAvailableMoveClick);

        }

        public override void LeaveState()
        {
            base.LeaveState();

            stateMachine.Telegraph.ClearAvalableMoves();
        }

        private void OnAvailableMoveClick(TargetPos target)
        {
            var path = stateMachine.Movement.GetPath(target.Pos);
            stateMachine.StartCoroutine(MoveAndEndTurn(path));
        }
        private IEnumerator MoveAndEndTurn(Queue<Vector2Int> path)
        {
            yield return stateMachine.Movement.MoveWithDelay(path);
            stateMachine.EndTurn();
        }
    }
}
