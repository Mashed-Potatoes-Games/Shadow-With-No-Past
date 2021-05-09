using ShadowWithNoPast.Entities.Abilities;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowWithNoPast.Entities
{
    public class PlayerAbilityUseListenState : PlayerAbilityPickListenState
    {
        private AbilityInstance abilityInstance;

        public PlayerAbilityUseListenState(GridEntity player, InputTurnsController stateMachine, AbilityInstance ability) 
            : base(player, stateMachine)
        {
            abilityInstance = ability;
        }

        public override void EnterState()
        {
            base.EnterState();

            PointerActions actions = new PointerActions()
            {
                OnClick = (caller, target) => AbilityExecuted(target),

                OnPointerEnter = (caller, target) =>
                    stateMachine.Telegraph.TelegraphAbility(target, abilityInstance, true),

                OnPointerLeave = (caller, target) =>
                    stateMachine.Telegraph.ClearAbility()
            };

            stateMachine.Telegraph.TelegraphAvailableAttacks(abilityInstance.AvailableTargets(), 1, actions);
            InputControls.Instance.Enable();
            InputControls.Instance.InGameMenu.Cancel.performed += CancelAbility;
        }

        private void CancelAbility(InputAction.CallbackContext context)
        {
            abilityInstance.Cancel();
            if(stateMachine.MadeMove)
            {
                stateMachine.SetState(new PlayerAbilityPickListenState(player, stateMachine));
                return;
            }
            stateMachine.SetState(new PlayerMoveListenState(player, stateMachine));
        }

        private void AbilityExecuted(WorldPos pos)
        {
            stateMachine.StartCoroutine(abilityInstance.UseAbility(pos));
        }

        public override void LeaveState()
        {
            base.LeaveState();

            stateMachine.Telegraph.ClearAvailableAttacks();
            stateMachine.Telegraph.ClearAbility();
            InputControls.Instance.InGameMenu.Cancel.performed -= CancelAbility;
        }
    }
}
