namespace ShadowWithNoPast.Entities
{
    public class PlayerMoveListenState : PlayerAbilityPickListenState
    {
        private bool MoveIsOver;

        public PlayerMoveListenState(GridEntity player, PlayerStateMachine stateMachine) : base(player, stateMachine)
        {
        }

        public override void EnterState()
        {
            base.EnterState();

            stateMachine.MadeMove = false;
        }
    }
}
