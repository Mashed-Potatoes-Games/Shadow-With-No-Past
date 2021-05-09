using ShadowWithNoPast.Entities.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowWithNoPast.Entities
{
    public class PlayerAbilityPickListenState : PlayerTurnState
    {
        public PlayerAbilityPickListenState(GridEntity player, InputTurnsController stateMachine) : base(player, stateMachine) { }

        public override void EnterState()
        {
            base.EnterState();

            stateMachine.Abilities.AbilityUsedWithNoTarget += OnAbilityPicked;
        }

        public override void LeaveState()
        {
            base.LeaveState();

            stateMachine.Abilities.AbilityUsedWithNoTarget -= OnAbilityPicked;
        }

        private void OnAbilityPicked(AbilityInstance abilityInstance)
        {
            stateMachine.SetState(new PlayerAbilityUseListenState(player, stateMachine, abilityInstance));
        }
    }
}
