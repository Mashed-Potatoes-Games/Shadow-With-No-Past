using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GridObjects
{
    [ExecuteAlways]
    public class PlayerEntity : BaseEntity
    {

        public override int MoveDistance => 3;
        public override TurnPriority Priority => TurnPriority.Player;

        public IEnumerator ListenToInputAndMakeAMove()
        {
            bool moveIsOver = false;
            while (!moveIsOver)
            {
                //TODO: Try to move this functionality to scriptable tiles!!
                if (Input.GetMouseButtonDown(0))
                {

                    Vector2Int targetPos = GridManagement.CellFromMousePos();

                    Queue<Vector2Int> path = GetPath(targetPos);

                    if(path != null)
                    {
                        yield return MoveWithDelay(path);
                        moveIsOver = true;
                    }

                }


                //Check for unputes to move or attack adjacent fields.
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if(TryInstantMoveTo(Direction.Up))
                    {
                        moveIsOver = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (TryInstantMoveTo(Direction.Right))
                    {
                        moveIsOver = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (TryInstantMoveTo(Direction.Down))
                    {
                        moveIsOver = true;
                    }
                }
                else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (TryInstantMoveTo(Direction.Left))
                    {
                        moveIsOver = true;
                    }
                }

                yield return null;
            }
        }

        //Those functions should never be executed;
        public override IEnumerator ExecuteMove()
        {
            throw new NotImplementedException();
        }

        public override IEnumerator PrepareAndTelegraphMove()
        {
            throw new NotImplementedException();
        }
    }

}
