using ShadowWithNoPast.Algorithms;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ShadowWithNoPast.Entities
{
    class InputTurnsController : MonoBehaviour, ITurnController
    {
        public TurnPriority Priority { get; set; } = TurnPriority.Player;

        private GridEntity entity;
        private WorldManagement world;
        private IMovementController movement;
        private ITelegraphController telegraph;

        private bool IsCurrentMove = false;

        private float delayInSecBetweenCellsMove = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            entity = GetComponent<GridEntity>();
            world = entity.WorldGrid;
            movement = GetComponent<IMovementController>();
            telegraph = GetComponent<ITelegraphController>();
        }

        public IEnumerator PrepareAndTelegraphMove()
        {
            yield break;
        }

        public IEnumerator ExecuteMove()
        {
            telegraph.OnClick -= OnAvailableMoveClick;
            telegraph.OnClick += OnAvailableMoveClick;
            telegraph.TelegraphAvailableMove(0.5f);
            yield return ListenForInput();
            telegraph.ClearAvalableMoves();
        }

        private void OnAvailableMoveClick(Vector2Int pos)
        {
            //TODO: EXECUTE MOVEMENT
            var path = movement.GetPath(pos);
            StartCoroutine(MoveAndEndTurn(path));
        }

        private IEnumerator MoveAndEndTurn(Queue<Vector2Int> path)
        {
            yield return MoveWithDelay(path);
            IsCurrentMove = false;
        }

        public IEnumerator ListenForInput()
        {
            IsCurrentMove = true;
            while (IsCurrentMove)
            {
                //Check for unputes to move or attack adjacent fields.
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Up))
                    {
                        IsCurrentMove = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Right))
                    {
                        IsCurrentMove = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Down))
                    {
                        IsCurrentMove = false;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Left))
                    {
                        IsCurrentMove = false;
                    }
                }

                yield return null;
            }
        }

        public IEnumerator MoveWithDelay(Queue<Vector2Int> path)
        {
            int movesLeft = entity.MoveDistance;
            while (path.Count > 0 && movesLeft > 0)
            {
                Vector2Int pos = path.Dequeue();
                if (!movement.TryInstantMoveTo(pos))
                {
                    yield break;
                }
                else
                {
                    yield return new WaitForSeconds(delayInSecBetweenCellsMove);
                    movesLeft--;
                }
            }
        }
    }
}
