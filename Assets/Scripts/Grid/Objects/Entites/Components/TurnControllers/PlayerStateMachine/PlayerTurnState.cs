namespace ShadowWithNoPast.Entities
{
    public abstract class PlayerTurnState
    {
        protected GridEntity player;
        protected PlayerStateMachine stateMachine;

        public PlayerTurnState(GridEntity player, PlayerStateMachine stateMachine)
        {
            this.player = player;
            this.stateMachine = stateMachine;
        }

        public virtual void EnterState()
        {
        }

        public virtual void LeaveState()
        {
        }

    }
}
