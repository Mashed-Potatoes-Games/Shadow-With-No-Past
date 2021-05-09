namespace ShadowWithNoPast.Entities
{
    public abstract class PlayerTurnState
    {
        protected GridEntity player;
        protected InputTurnsController stateMachine;

        public PlayerTurnState(GridEntity player, InputTurnsController stateMachine)
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
