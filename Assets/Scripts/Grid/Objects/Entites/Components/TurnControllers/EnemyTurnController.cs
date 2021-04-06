using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    [RequireComponent(typeof(GridEntity))]
    public class EnemyTurnController : MonoBehaviour, ITurnController
    {
        public event Action TurnPassed;

        public TurnPriority Priority { get; set; } = TurnPriority.Normal;

        private GridEntity entity;
        private WorldManagement world;
        private IMovementController movement;

        public Queue<Vector2Int> MovementQueue = new Queue<Vector2Int>();

        // Start is called before the first frame update
        void Start()
        {
            entity = GetComponent<GridEntity>();
            world = entity.WorldGrid;
            movement = GetComponent<IMovementController>();
        }
        public IEnumerator MoveAndTelegraphAction()
        {
            GridEntity player = FindPlayer();
            if (player is null)
            {
                yield break;
            }

            Queue<Vector2Int> path = movement.GetPath(player.Pos, false);

            if (path != null)
            {
                yield return movement.MoveWithDelay(path);
            }
        }

        public IEnumerator ExecuteMove()
        {
            EndTurn();
            yield break;
        }
        private void EndTurn()
        {
            TurnPassed?.Invoke();
        }

        

        public GridEntity FindPlayer()
        {
            var entities = transform.parent.GetComponentsInChildren<GridEntity>();
            foreach (var entity in entities)
            {
                if (entity.CompareTag("Player"))
                {
                    return entity;
                }
            }

            return null;
        }
    }

}