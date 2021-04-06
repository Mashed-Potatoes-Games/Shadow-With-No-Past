using ShadowWithNoPast.Entities.Abilities;

namespace ShadowWithNoPast.Entities
{
    public class PlayerAbilityUseListenState : PlayerAbilityPickListenState
    {
        private AbilityInstance abilityInstance;

        public PlayerAbilityUseListenState(GridEntity player, PlayerStateMachine stateMachine, AbilityInstance ability) 
            : base(player, stateMachine)
        {
            abilityInstance = ability;
        }
    }
}
