using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowWithNoPast.Entities
{
    public abstract class PlayerStateMachine : MonoBehaviour
    {
        [HideInInspector] public bool MadeMove;
        [HideInInspector] public bool IsActiveTurn;

        public GameControls Controls;
        protected PlayerTurnState playerState;

        protected virtual void Awake()
        {
            Controls = new GameControls();
        }

        public virtual void Start()
        {
        }

        public void SetState(PlayerTurnState state)
        {
            if(playerState != null)
            {
                playerState.LeaveState();
            }
            playerState = state;
            state.EnterState();
        }
    }
}
