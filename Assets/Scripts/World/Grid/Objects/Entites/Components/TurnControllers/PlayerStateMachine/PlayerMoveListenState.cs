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

            var actions = new PointerActions()
            {
                OnClick = OnAvailableMoveClick,
                OnPointerEnter = (caller, target) =>
                {
                    caller.Highlight();
                },
                OnPointerLeave = (caller, target) =>
                {
                    caller.RemoveHighligh();
                }
            };

            stateMachine.Telegraph.TelegraphAvailableMove(actions);
        }

        public override void LeaveState()
        {
            base.LeaveState();

            stateMachine.Telegraph.ClearAvalableMoves();
        }

        private void OnAvailableMoveClick(TelegraphElement caller, WorldPos target)
        {
            var path = stateMachine.Movement.GetPath(target);
            stateMachine.StartCoroutine(MoveAndSwitchToAbilityListen(path));
        }
        private IEnumerator MoveAndSwitchToAbilityListen(Queue<WorldPos> path)
        {
            yield return stateMachine.Movement.MoveWithDelay(path);
            stateMachine.MadeMove = true;
            stateMachine.SetState(new PlayerAbilityPickListenState(player, stateMachine));
        }
    }
}
