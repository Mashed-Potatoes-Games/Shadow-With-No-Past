using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowWithNoPast.Entities
{
    public class PlayerAbilityPickListenState : PlayerTurnState
    {
        public PlayerAbilityPickListenState(GridEntity player, PlayerStateMachine stateMachine) : base(player, stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();

            stateMachine.IsActiveTurn = true;
            stateMachine.Controls.AbilityUsage.Enable();
        }

        private void Ability1Used(InputAction.CallbackContext obj)
        {
            Debug.Log("EventFired!!!!");
        }
    }
}
