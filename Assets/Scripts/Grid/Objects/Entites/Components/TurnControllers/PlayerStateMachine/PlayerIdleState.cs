namespace ShadowWithNoPast.Entities
{
    public class PlayerIdleState : PlayerTurnState
    {
        public PlayerIdleState(GridEntity player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            stateMachine.MadeMove = false;
            stateMachine.IsActiveTurn = false;
        }
    }
}
