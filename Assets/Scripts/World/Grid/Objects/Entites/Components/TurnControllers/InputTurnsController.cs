using ShadowWithNoPast.Algorithms;
using ShadowWithNoPast.Entities.Abilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridEntity))]
    public class InputTurnsController : MonoBehaviour, ITurnController
    {

        public event Action TurnPassed;
        
        [HideInInspector] public bool MadeMove;
        [HideInInspector] public bool IsActiveTurn { get; set; }

        public GridEntity Entity;

        public IAbilitiesController Abilities;
        public IMovementController Movement;
        public ITelegraphController Telegraph;

        protected PlayerTurnState playerState;

        public TurnPriority Priority { get; set; } = TurnPriority.Player;

        private void Awake()
        {
            //TODO Rewrite
            Entity = GetComponent<GridEntity>();
            Abilities = GetComponent<IAbilitiesController>();
            Movement = GetComponent<IMovementController>();
            Telegraph = GetComponent<ITelegraphController>();

            Abilities.AbilityUsed += OnAbilityUse;
        }
        public void SetState(PlayerTurnState state)
        {
            if (playerState != null)
            {
                playerState.LeaveState();
            }
            playerState = state;
            state.EnterState();
        }

        public IEnumerator MoveAndTelegraphAction()
        {
            yield break;
        }

        public IEnumerator ExecuteMove()
        {
            SetState(new PlayerMoveListenState(Entity, this));
            yield return ListenForInput();
            Telegraph.ClearAvalableMoves();
        }


        

        public IEnumerator ListenForInput()
        {
            IsActiveTurn = true;
            while (IsActiveTurn)
            {
                yield return null;
            }
        }

        private void OnAbilityUse(AbilityInstance abilityInstance)
        {
            if(abilityInstance.EndsTurn)
            {
                EndTurn();
            }
        }

        public void EndTurn()
        {
            IsActiveTurn = false;

            TurnPassed?.Invoke();

            SetState(new PlayerIdleState(Entity, this));
        }

        public bool EngageCombat()
        {
            return true;
        }

        public bool ReadyToExecute()
        {
            return true;
        }
    }
}
