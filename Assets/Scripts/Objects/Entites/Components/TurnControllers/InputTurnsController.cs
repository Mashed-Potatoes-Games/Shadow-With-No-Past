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
        public int MoveDistance { get; set; } = 3;

        private GridEntity entity;
        private WorldManagement world;
        private IMovementController movement;

        private float delayInSecBetweenCellsMove = 0.1f;

        // Start is called before the first frame update
        void Start()
        {
            entity = GetComponent<GridEntity>();
            world = entity.WorldGrid;
            movement = GetComponent<IMovementController>();
        }

        public IEnumerator PrepareAndTelegraphMove()
        {
            yield break;
        }

        public IEnumerator ExecuteMove()
        {
            bool moveIsOver = false;
            while (!moveIsOver)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    Vector2Int targetPos = WorldManagement.CellFromMousePos();

                    Queue<Vector2Int> path = movement.GetPath(targetPos);

                    if (path != null && MoveDistance >= path.Count)
                    {
                        yield return MoveWithDelay(path);
                        moveIsOver = true;
                    }

                }


                //Check for unputes to move or attack adjacent fields.
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Up))
                    {
                        moveIsOver = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Right))
                    {
                        moveIsOver = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Down))
                    {
                        moveIsOver = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (movement.TryInstantMoveTo(Direction.Left))
                    {
                        moveIsOver = true;
                    }
                }

                yield return null;
            }
        }
        public IEnumerator MoveWithDelay(Queue<Vector2Int> path)
        {
            int movesLeft = MoveDistance;
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
