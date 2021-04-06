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
    class InputTurnsController : PlayerStateMachine, ITurnController
    {
        public event Action TurnPassed;

        public TurnPriority Priority { get; set; } = TurnPriority.Player;

        private GridEntity entity;
        private IMovementController movement;
        private ITelegraphController telegraph;
        private IAbilitiesController abilities;

        protected override void Awake()
        {
            base.Awake();

            entity = GetComponent<GridEntity>();
            movement = GetComponent<IMovementController>();
            telegraph = GetComponent<ITelegraphController>();
            abilities = GetComponent<IAbilitiesController>();

            abilities.AbilityUsed += OnAbilityUse;
        }

        public IEnumerator MoveAndTelegraphAction()
        {
            yield break;
        }

        public IEnumerator ExecuteMove()
        {
            SetState(new PlayerMoveListenState(entity, this));
            telegraph.TelegraphAvailableMove(0.5f, OnAvailableMoveClick);
            yield return ListenForInput();
            telegraph.ClearAvalableMoves();
        }

        private void OnAvailableMoveClick(Vector2Int pos)
        {
            var path = movement.GetPath(pos);
            StartCoroutine(MoveAndEndTurn(path));
        }

        private IEnumerator MoveAndEndTurn(Queue<Vector2Int> path)
        {
            yield return movement.MoveWithDelay(path);
            EndTurn();
        }

        public IEnumerator ListenForInput()
        {
            IsActiveTurn = true;
            while (IsActiveTurn)
            {
                yield return null;
            }
        }

        public void OnFirstAbilityUse()
        {
            Debug.Log("First ability used!");
            EndTurn();
        }

        

        private void OnAbilityUse(AbilityInstance abilityInstance)
        {
            if(abilityInstance.EndsTurn)
            {
                EndTurn();
            }
        }

        private void EndTurn()
        {
            IsActiveTurn = false;

            TurnPassed?.Invoke();
        }
    }
}
