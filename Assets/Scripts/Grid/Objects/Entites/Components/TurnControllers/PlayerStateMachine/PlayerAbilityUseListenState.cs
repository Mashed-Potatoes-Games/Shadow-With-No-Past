using ShadowWithNoPast.Entities.Abilities;
using UnityEngine;

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

            stateMachine.Telegraph.TelegraphAttack(abilityInstance.AvailableTargets(), 1, AbilityExecuted);
        }

        private void AbilityExecuted(TargetPos pos)
        {
            stateMachine.StartCoroutine(abilityInstance.UseAbility(pos));
        }

        public override void LeaveState()
        {
            base.LeaveState();

            stateMachine.Telegraph.ClearAttack();
        }
    }
}
