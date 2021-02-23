using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShadowWithNoPast.GridObjects
{
    [ExecuteAlways]
    public class PlayerEntity : BaseEntity
    {

        protected override void Update()
        {
            base.Update();
            //Look for mouse click to move.
            //TODO: Try to move this functionality to scriptable tiles!!
            if (Input.GetMouseButtonDown(0))
            {
                Vector2Int targetPos = GridManagement.CellFromMousePos();

                TryMoveTo(targetPos);

            }

            //Check for unputes to move or attack adjacent fields.
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                TryMoveTo(Direction.Up);
            }
            else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                TryMoveTo(Direction.Right);
            }
            else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                TryMoveTo(Direction.Down);
            }
            else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                TryMoveTo(Direction.Left);
            }
        }

        //Figure out the way to stop movement queue, to wait for the user input!
        public override void MakeTurn()
        {
            throw new NotImplementedException();
        }
    }

}
