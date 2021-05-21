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
            InputControls.CancelButton.AddInterrupting(CancelAbility);
        }

        private bool CancelAbility()
        {
            abilityInstance.Cancel();
            if(stateMachine.MadeMove)
            {
                stateMachine.SetState(new PlayerAbilityPickListenState(player, stateMachine));
            }
            else
            {
                stateMachine.SetState(new PlayerMoveListenState(player, stateMachine));
            }
            return true;
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
            InputControls.CancelButton.Remove(CancelAbility);
        }
    }
}
